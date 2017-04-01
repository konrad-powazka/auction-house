using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.Messages.Events.UserMessaging;
using AuctionHouse.Persistence.Shared;
using Bogus;
using Bogus.DataSets;

namespace AuctionHouse.FakeDataGeneratorLauncher
{
	public class FakeDataGenerator
	{
		private const string PredefinedUserName = "konrad.powazka";
		private readonly ICommandQueue _commandQueue;
		private readonly IEventsDatabase _eventsDatabase;
		private readonly ITimeProvider _timeProvider;

		public FakeDataGenerator(ITimeProvider timeProvider, IEventsDatabase eventsDatabase, ICommandQueue commandQueue)
		{
			_timeProvider = timeProvider;
			_eventsDatabase = eventsDatabase;
			_commandQueue = commandQueue;
		}

		public async Task GenerateFakeData()
		{
			var utcNow = _timeProvider.UtcNow;

			var userNames =
				Enumerable.Range(0, 17)
					.Select(i => new Person().UserName)
					.Concat(new[] {PredefinedUserName})
					.Distinct()
					.ToList();

			var generatedAuctions = GenerateAuctions(userNames, utcNow);

			foreach (var generatedAuction in generatedAuctions)
			{
				await
					_eventsDatabase.AppendToStream(StreamNameGenerator.GenerateAuctionStreamName(generatedAuction.Id),
						WrapIntoEnvelopeCollection(generatedAuction.Events.AsEnumerable()), ExpectedStreamVersion.NotExisting);

				var finishAuctionCommand = new FinishAuctionCommand
				{
					Id = generatedAuction.Id
				};

				await _commandQueue.QueueCommand(finishAuctionCommand, Guid.NewGuid(), "system", generatedAuction.EndDateTime);
			}

			var userMessageSentEvents = userNames.SelectMany(u => GenerateUserMessageSentEvents(u, userNames, utcNow));

			foreach (var userMessageSentEvent in userMessageSentEvents)
			{
				await
					_eventsDatabase.AppendToStream($"SentUserMessage-{userMessageSentEvent.MessageId}",
						WrapIntoEnvelopeCollection(userMessageSentEvent), ExpectedStreamVersion.NotExisting);
			}
		}

		private IEnumerable<GeneratedAuction> GenerateAuctions(IReadOnlyCollection<string> allUserNames, DateTime utcNow)
		{
			var randomizer = new Randomizer();
			var dateRandomizer = new Date();

			return
				allUserNames.SelectMany(
					currentUserName =>
						Enum.GetValues(typeof(GeneratedAuctionCharacteristic))
							.Cast<GeneratedAuctionCharacteristic>()
							.SelectMany(
								generatedAuctionCharacteristic =>
									GenerateAuctionsHavingCharactersistic(generatedAuctionCharacteristic, currentUserName, allUserNames, randomizer,
										dateRandomizer, utcNow)));
		}

		private IEnumerable<GeneratedAuction> GenerateAuctionsHavingCharactersistic(
			GeneratedAuctionCharacteristic generatedAuctionCharacteristic, string currentUserName,
			IReadOnlyCollection<string> allUserNames, Randomizer randomizer, Date dateRandomizer, DateTime utcNow)
		{
			var minNumberOfAuctions = currentUserName == PredefinedUserName ? 15 : 0;
			var numberOfAuctions = randomizer.Int(minNumberOfAuctions, 40);

			return
				Enumerable.Range(0, numberOfAuctions)
					.Select(
						i => GenerateAuction(currentUserName, generatedAuctionCharacteristic, allUserNames, randomizer, dateRandomizer, utcNow));
		}

		private GeneratedAuction GenerateAuction(string currentUserName,
			GeneratedAuctionCharacteristic generatedAuctionCharacteristic, IReadOnlyCollection<string> allUserNames,
			Randomizer randomizer, Date dateRandomizer, DateTime utcNow)
		{
			var otherUserNames = allUserNames.Except(new[] {currentUserName}).ToList();
			var hasBuyNowPrice = randomizer.Int(1, 3) > 1;
			var isEndDateFuture = generatedAuctionCharacteristic.CheckIfAuctionIsInProgress() || hasBuyNowPrice;
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
					return s.Lorem.Paragraphs(numberOfParagraphs, "\n");
				})
				.RuleFor(a => a.BuyNowPrice,
					(s, a) => hasBuyNowPrice ? decimal.Round(s.Random.Decimal(a.StartingPrice, 20000)) : (decimal?) null)
				.RuleFor(a => a.EndDateTime, s => isEndDateFuture
					? s.Date.BetweenUtc(utcNow.AddMinutes(3), utcNow.AddDays(21))
					: s.Date.BetweenUtc(utcNow.AddDays(-1000), utcNow.AddMinutes(-3)))
				.RuleFor(a => a.StartingPrice, s => s.Random.Bool() ? decimal.Round(s.Random.Decimal(1, 10000), 2) : 0)
				.RuleFor(a => a.CurrentPrice, (s, a) => a.StartingPrice)
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
				.RuleFor(a => a.MinimalPriceForNextBidder, (s, e) => Math.Max(e.StartingPrice, 0.01m))
				.RuleFor(a => a.CreatedDateTime,
					(s, a) => s.Date.BetweenUtc(utcNow.AddDays(-21), utcNow.AddHours(-1)));

			Debug.Assert(auctionCreatedEventFaker.Validate());
			var auctionCreatedEvent = auctionCreatedEventFaker.Generate(1).Single();
			Debug.Assert(auctionCreatedEvent.CreatedDateTime < utcNow);

			var bidMadeEvents = GenerateBidMadeEvents(auctionCreatedEvent, generatedAuctionCharacteristic, currentUserName,
				allUserNames, isEndDateFuture, randomizer, dateRandomizer, utcNow).ToList();

			var allEvents = new IEvent[] {auctionCreatedEvent}.Concat(bidMadeEvents).ToList();

			var auctionFinishedDateTime = TryGetFinishedDateTime(auctionCreatedEvent, generatedAuctionCharacteristic,
				isEndDateFuture, bidMadeEvents);

			if (auctionFinishedDateTime.HasValue)
			{
				Debug.Assert(auctionFinishedDateTime.Value <= utcNow);

				var auctionFinishedEvent = new AuctionFinishedEvent
				{
					AuctionId = id,
					FinishedDateTime = auctionFinishedDateTime.Value
				};

				allEvents.Add(auctionFinishedEvent);
			}

			return new GeneratedAuction
			{
				Id = id,
				EndDateTime = auctionCreatedEvent.EndDateTime,
				Events = allEvents
			};
		}

		private DateTime? TryGetFinishedDateTime(AuctionCreatedEvent auctionCreatedEvent,
			GeneratedAuctionCharacteristic generatedAuctionCharacteristic, bool isEndDateFuture,
			IReadOnlyCollection<BidMadeEvent> bidMadeEvents)
		{
			if (!generatedAuctionCharacteristic.CheckIfFinishesWithBuy() && isEndDateFuture)
			{
				return null;
			}

			if (bidMadeEvents.Any())
			{
				var winningBid = bidMadeEvents.Last();

				if (auctionCreatedEvent.BuyNowPrice.HasValue && winningBid.BidPrice >= auctionCreatedEvent.BuyNowPrice.Value)
				{
					return winningBid.BidDateTime;
				}
			}

			if (isEndDateFuture)
			{
				throw new ArgumentException("Impossible data for given auction characteristic.",
					nameof(generatedAuctionCharacteristic));
			}

			return auctionCreatedEvent.EndDateTime;
		}

		private IEnumerable<BidMadeEvent> GenerateBidMadeEvents(AuctionCreatedEvent auctionCreatedEvent,
			GeneratedAuctionCharacteristic generatedAuctionCharacteristic, string currentUserName,
			IReadOnlyCollection<string> allUserNames, bool isEndDateFuture, Randomizer randomizer, Date dateRandomizer, DateTime utcNow)
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
			var minBidDateTime = auctionCreatedEvent.CreatedDateTime;

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

				var currentPrice = bidMadeEvents.Any()
					? bidMadeEvents.Max(e => e.BidPrice)
					: auctionCreatedEvent.StartingPrice;

				var minimalPriceForNextBidder = currentPrice + 0.01M;

				var bidMadeEvent = new BidMadeEvent
				{
					AuctionId = auctionCreatedEvent.Id,
					BidderUserName = bidderUserName,
					BidPrice = bidPrice,
					HighestBidderUserName = bidderUserName,
					MinimalPriceForNextBidder = minimalPriceForNextBidder,
					HighestBidPrice = bidPrice,
					CurrentPrice = currentPrice,
					BidDateTime = dateRandomizer.BetweenUtc(minBidDateTime, utcNow)
				};

				Debug.Assert(bidMadeEvent.BidDateTime <= utcNow);
				bidMadeEvents.Add(bidMadeEvent);
				minBidPrice = bidPrice + 0.01M;
				minBidDateTime = bidMadeEvent.BidDateTime;
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
			IEnumerable<string> allUserNames, DateTime utcNow)
		{
			var otherUserNames = allUserNames.Except(new[] {userName}).ToList();

			var userMessageSentEventFaker = new Faker<UserMessageSentEvent>()
				.RuleFor(e => e.MessageSubject, s => s.Lorem.Sentence(s.Random.Int(2, 10)))
				.RuleFor(e => e.MessageBody, s =>
				{
					var numberOfParagraphs = s.Random.Int(1, 4);
					return s.Lorem.Paragraphs(numberOfParagraphs, "\n");
				})
				.RuleFor(e => e.SentDateTime, s => s.Date.BetweenUtc(utcNow.AddDays(-500), utcNow))
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

			public DateTime EndDateTime { get; set; }

			public IReadOnlyCollection<IEvent> Events { get; set; }
		}
	}
}