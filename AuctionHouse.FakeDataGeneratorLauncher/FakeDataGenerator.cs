using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.Messages.Events.UserMessaging;
using AuctionHouse.Persistence.Shared;
using Bogus;

namespace AuctionHouse.FakeDataGeneratorLauncher
{
	public class FakeDataGenerator
	{
		private const string PredefinedUserName = "konrad.powazka";
		private readonly IEventsDatabase _eventsDatabase;
		private readonly ITimeProvider _timeProvider;

		public FakeDataGenerator(ITimeProvider timeProvider, IEventsDatabase eventsDatabase)
		{
			_timeProvider = timeProvider;
			_eventsDatabase = eventsDatabase;
		}

		public async Task GenerateFakeData()
		{
			//var hardcodedAuctionTemplates = GetHardcodedAuctionTemplates();

			var userNames =
				Enumerable.Range(0, 17)
					.Select(i => new Person().UserName)
					.Concat(new[] {PredefinedUserName})
					//.Concat(hardcodedAuctionTemplates.Select(a => a.CreatedByUserName))
					.Distinct()
					.ToList();

			var generatedAuctions = GenerateAuctions(userNames);

			foreach (var generatedAuction in generatedAuctions)
			{
				await
					_eventsDatabase.AppendToStream(StreamNameGenerator.GenerateAuctionStreamName(generatedAuction.Id),
						WrapIntoEnvelopeCollection(generatedAuction.Events.AsEnumerable()), ExpectedStreamVersion.NotExisting);
			}

			var userMessageSentEvents = userNames.SelectMany(u => GenerateUserMessageSentEvents(u, userNames));

			foreach (var userMessageSentEvent in userMessageSentEvents)
			{
				await
					_eventsDatabase.AppendToStream($"SentUserMessage-{userMessageSentEvent.MessageId}",
						WrapIntoEnvelopeCollection(userMessageSentEvent), ExpectedStreamVersion.NotExisting);
			}
		}

		private IEnumerable<GeneratedAuction> GenerateAuctions(IReadOnlyCollection<string> allUserNames)
		{
			var randomizer = new Randomizer();

			return
				allUserNames.SelectMany(
					currentUserName =>
						Enum.GetValues(typeof(GeneratedAuctionCharacteristic))
							.Cast<GeneratedAuctionCharacteristic>()
							.SelectMany(
								generatedAuctionCharacteristic =>
									GenerateAuctionsHavingCharactersistic(generatedAuctionCharacteristic, currentUserName, allUserNames, randomizer)));
		}

		private IEnumerable<GeneratedAuction> GenerateAuctionsHavingCharactersistic(
			GeneratedAuctionCharacteristic generatedAuctionCharacteristic, string currentUserName,
			IReadOnlyCollection<string> allUserNames, Randomizer randomizer)
		{
			var minNumberOfAuctions = currentUserName == PredefinedUserName ? 15 : 0;
			var numberOfAuctions = randomizer.Int(minNumberOfAuctions, 40);

			return
				Enumerable.Range(0, numberOfAuctions)
					.Select(i => GenerateAuction(currentUserName, generatedAuctionCharacteristic, allUserNames, randomizer));
		}

		private GeneratedAuction GenerateAuction(string currentUserName,
			GeneratedAuctionCharacteristic generatedAuctionCharacteristic, IReadOnlyCollection<string> allUserNames,
			Randomizer randomizer)
		{
			var otherUserNames = allUserNames.Except(new[] {currentUserName}).ToList();
			var isEndDateFuture = generatedAuctionCharacteristic.CheckIfAuctionIsInProgress() || randomizer.Bool();
			var id = Guid.NewGuid();

			var auctionCreatedEventFaker = new Faker<AuctionCreatedEvent>()
				.RuleFor(a => a.Id, id)
				.RuleFor(a => a.Title, s =>
				{
					var title = s.Commerce.ProductName();
					return title;
				})
				.RuleFor(a => a.Description, s =>
				{
					var numberOfParagraphs = s.Random.Int(3, 10);
					return s.Lorem.Paragraphs(numberOfParagraphs);
				})
				.RuleFor(a => a.BuyNowPrice,
					(s, a) => s.Random.Int(1, 3) > 1 ? decimal.Round(s.Random.Decimal(a.StartingPrice, 20000)) : (decimal?) null)
				.RuleFor(a => a.EndDateTime, s => isEndDateFuture
					? s.Date.Between(_timeProvider.Now.AddMinutes(3), _timeProvider.Now.AddDays(21))
					: s.Date.Between(_timeProvider.Now.AddDays(-1000), _timeProvider.Now.AddMinutes(-3)))
				.RuleFor(a => a.StartingPrice, s => s.Random.Bool() ? decimal.Round(s.Random.Decimal(1, 10000), 2) : 0)
				.RuleFor(a => a.BuyNowPrice,
					(s, a) =>
					{
						var mustHaveBuyNowPrice = isEndDateFuture && generatedAuctionCharacteristic.CheckIfFinishesWithBuy();

						return mustHaveBuyNowPrice || s.Random.Bool()
							? decimal.Round(s.Random.Decimal(a.StartingPrice, 20000))
							: (decimal?) null;
					})
				.RuleFor(a => a.CreatedByUserName,
					s => generatedAuctionCharacteristic.CheckIfUserIsSelling() ? currentUserName : s.PickRandom(otherUserNames))
				.RuleFor(a => a.MinimalPriceForNextBidder, (s, e) => Math.Max(e.StartingPrice, 0.01m));

			Debug.Assert(auctionCreatedEventFaker.Validate());

			var auctionCreatedEvent = auctionCreatedEventFaker.Generate(1).Single();
			var bidMadeEvents = GenerateBidMadeEvents(auctionCreatedEvent, generatedAuctionCharacteristic, currentUserName,
				allUserNames, isEndDateFuture, randomizer);

			return new GeneratedAuction
			{
				Id = id,
				Events = new IEvent[] {auctionCreatedEvent}.Concat(bidMadeEvents).ToList()
			};
		}

		private IEnumerable<BidMadeEvent> GenerateBidMadeEvents(AuctionCreatedEvent auctionCreatedEvent,
			GeneratedAuctionCharacteristic generatedAuctionCharacteristic, string currentUserName,
			IReadOnlyCollection<string> allUserNames, bool isEndDateFuture, Randomizer randomizer)
		{
			if (!generatedAuctionCharacteristic.CheckIfCanHaveBids())
			{
				return Enumerable.Empty<BidMadeEvent>();
			}

			var minNumberOfBids = GetMinimumNumberOfBids(generatedAuctionCharacteristic);
			var numberOfBids = minNumberOfBids == 0 && randomizer.Bool() ? 0 : randomizer.Int(minNumberOfBids, 15);
			var canCurrentUserBid = !generatedAuctionCharacteristic.CheckIfUserIsSelling();
			var bidMadeEvents = new List<BidMadeEvent>();
			var userNamesNotSelling = allUserNames.Except(new[] {auctionCreatedEvent.CreatedByUserName}).ToArray();
			var userNamesNotSellingExceptCurrentUser = userNamesNotSelling.Except(new[] {currentUserName}).ToArray();
			var minBidPrice = auctionCreatedEvent.MinimalPriceForNextBidder;

			for (var bidNumber = 1; bidNumber <= numberOfBids; bidNumber++)
			{
				var isLastBid = bidNumber == numberOfBids;
				var currentUserMadeBids = bidMadeEvents.Any(e => e.BidderUserName == currentUserName);

				var mustInsertCurrentUserBidNow = (isLastBid &&
				                                   generatedAuctionCharacteristic ==
				                                   GeneratedAuctionCharacteristic.UserMadeBidsAndWon) ||
				                                  (bidNumber == numberOfBids - 1 &&
				                                   generatedAuctionCharacteristic ==
				                                   GeneratedAuctionCharacteristic.UserMadeBidsAndNotWon && !currentUserMadeBids) ||
				                                  (isLastBid &&
				                                   generatedAuctionCharacteristic ==
				                                   GeneratedAuctionCharacteristic.UserMadeBidsAndInProgress && !currentUserMadeBids);

				var canInsertCurrentUserBidNow =
					canCurrentUserBid &&
					!(isLastBid && generatedAuctionCharacteristic == GeneratedAuctionCharacteristic.UserMadeBidsAndNotWon);

				var bidderUserName = mustInsertCurrentUserBidNow
					? currentUserName
					: (canInsertCurrentUserBidNow
						? randomizer.ArrayElement(userNamesNotSelling)
						: randomizer.ArrayElement(userNamesNotSellingExceptCurrentUser));

				var mustBuy = generatedAuctionCharacteristic.CheckIfFinishesWithBuy() && isLastBid;
				var canBuy = !generatedAuctionCharacteristic.CheckIfFinishesWithBuy() || !isLastBid;
				var numberOfBidsLeft = numberOfBids - bidNumber;
				var maxBidPrice = auctionCreatedEvent.BuyNowPrice - numberOfBidsLeft*0.01M ?? 400000M;

				if (mustBuy)
				{
					if (isEndDateFuture)
					{
						if (!auctionCreatedEvent.BuyNowPrice.HasValue)
						{
							throw new ArgumentOutOfRangeException(nameof(generatedAuctionCharacteristic));
						}

						minBidPrice = auctionCreatedEvent.BuyNowPrice.Value;
						maxBidPrice = auctionCreatedEvent.BuyNowPrice.Value;
					}
					else
					{
						minBidPrice = minBidPrice + 0.01M;
					}
				}
				else if (!canBuy && auctionCreatedEvent.BuyNowPrice.HasValue)
				{
					maxBidPrice = Math.Min(maxBidPrice, auctionCreatedEvent.BuyNowPrice.Value - 0.01M);
				}

				var bidPrice = Math.Round(randomizer.Decimal(minBidPrice, maxBidPrice), 2);

				var minimalPriceForNextBidder = bidMadeEvents.Any()
					? bidMadeEvents.Max(e => e.BidPrice) + 0.01M
					: auctionCreatedEvent.StartingPrice;

				var bidMadeEvent = new BidMadeEvent
				{
					AuctionId = auctionCreatedEvent.Id,
					BidderUserName = bidderUserName,
					BidPrice = bidPrice,
					HighestBidderUserName = bidderUserName,
					MinimalPriceForNextBidder = minimalPriceForNextBidder,
					HighestBidPrice = bidPrice
				};

				bidMadeEvents.Add(bidMadeEvent);
				minBidPrice = bidPrice + 0.01M;
			}

			return bidMadeEvents;
		}

		private int GetMinimumNumberOfBids(GeneratedAuctionCharacteristic generatedAuctionCharacteristic)
		{
			switch (generatedAuctionCharacteristic)
			{
				case GeneratedAuctionCharacteristic.UserMadeBidsAndWon:
				case GeneratedAuctionCharacteristic.UserMadeBidsAndInProgress:
				case GeneratedAuctionCharacteristic.UserSellingAndFinishedWithBids:
					return 1;
				case GeneratedAuctionCharacteristic.UserMadeBidsAndNotWon:
					return 2;
				case GeneratedAuctionCharacteristic.UserSellingAndInProgress:
				case GeneratedAuctionCharacteristic.UserSellingAndFinishedWithoutBids:
					return 0;
				default:
					throw new ArgumentOutOfRangeException(nameof(generatedAuctionCharacteristic));
			}
		}

		private IEventEnvelope<TEvent> WrapIntoEnvelope<TEvent>(TEvent @event) where TEvent : IEvent
		{
			return new EventEnvelope<TEvent>(@event, Guid.NewGuid());
		}

		private IEnumerable<IEventEnvelope<TEvent>> WrapIntoEnvelopeCollection<TEvent>(TEvent @event) where TEvent : IEvent
		{
			return new[] {WrapIntoEnvelope(@event)};
		}

		private IEnumerable<IEventEnvelope<IEvent>> WrapIntoEnvelopeCollection(IEnumerable<IEvent> events)
		{
			return events.Select<IEvent, IEventEnvelope<IEvent>>(WrapIntoEnvelope);
		}

		private IEnumerable<UserMessageSentEvent> GenerateUserMessageSentEvents(string userName,
			IEnumerable<string> allUserNames)
		{
			var otherUserNames = allUserNames.Except(new[] {userName}).ToList();

			var userMessageSentEventFaker = new Faker<UserMessageSentEvent>()
				.RuleFor(e => e.MessageSubject, s => s.Lorem.Sentence(s.Random.Int(2, 10)))
				.RuleFor(e => e.MessageBody, s =>
				{
					var numberOfParagraphs = s.Random.Int(1, 4);
					return s.Lorem.Paragraphs(numberOfParagraphs);
				})
				.RuleFor(e => e.SentDateTime, s => s.Date.Between(_timeProvider.Now.AddDays(-500), _timeProvider.Now))
				.RuleFor(e => e.MessageId, s => Guid.NewGuid())
				.RuleFor(e => e.SenderUserName, s => userName)
				.RuleFor(e => e.RecipientUserName, s => s.PickRandom(otherUserNames));

			userMessageSentEventFaker.Validate();
			var randomizer = new Randomizer();
			var numberOfMessages = randomizer.Number(userName == PredefinedUserName ? 30 : 0, 50);
			return userMessageSentEventFaker.Generate(numberOfMessages);
		}

		private class GeneratedAuction
		{
			public Guid Id { get; set; }

			public IReadOnlyCollection<IEvent> Events { get; set; }
		}
	}
}