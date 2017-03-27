using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.Messages.Events.UserMessaging;
using AuctionHouse.Persistence.Shared;
using Bogus;
using JetBrains.Annotations;

namespace AuctionHouse.FakeDataGeneratorLauncher
{
	public class FakeDataGenerator
	{
		private const string PredefinedUserName = "konrad.powazka";
		private readonly ITimeProvider _timeProvider;
		private readonly IEventsDatabase _eventsDatabase;

		public FakeDataGenerator(ITimeProvider timeProvider, IEventsDatabase eventsDatabase)
		{
			_timeProvider = timeProvider;
			_eventsDatabase = eventsDatabase;
		}

		public async Task GenerateFakeData()
		{
			var hardcodedAuctionTemplates = GetHardcodedAuctionTemplates();

			var userNames =
				Enumerable.Range(0, 17)
					.Select(i => new Person().UserName)
					.Concat(new[] {PredefinedUserName})
					.Concat(hardcodedAuctionTemplates.Select(a => a.CreatedByUserName))
					.Distinct()
					.ToList();

			var generatedAuctionTemplates = userNames.SelectMany(GenerateAuctionTemplates);
			var auctionTemplates = hardcodedAuctionTemplates.Concat(generatedAuctionTemplates).ToList();

			foreach (var auctionTemplate in auctionTemplates)
			{
				var auctionCreatedEvent = MapAuctionTemplateToAuctionCreatedEvent(auctionTemplate);

				await
					_eventsDatabase.AppendToStream(StreamNameGenerator.GenerateAuctionStreamName(auctionCreatedEvent.Id),
						WrapIntoEnvelopeCollection(auctionCreatedEvent), ExpectedStreamVersion.NotExisting);
			}

			var userMessageSentEvents = userNames.SelectMany(u => GenerateUserMessageSentEvents(u, userNames));

			foreach (var userMessageSentEvent in userMessageSentEvents)
			{
				await
					_eventsDatabase.AppendToStream($"SentUserMessage-{userMessageSentEvent.MessageId}",
						WrapIntoEnvelopeCollection(userMessageSentEvent), ExpectedStreamVersion.NotExisting);
			}
		}

		private IEnumerable<AuctionTemplate> GenerateAuctionTemplates(string userName)
		{
			var auctionTemplateFaker = new Faker<AuctionTemplate>()
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
					.RuleFor(a => a.EndDate, s => s.Date.Between(_timeProvider.Now.AddDays(1), _timeProvider.Now.AddDays(21)))
					.RuleFor(a => a.StartingPrice, s => s.Random.Bool() ? decimal.Round(s.Random.Decimal(1, 10000), 2) : 0)
					.RuleFor(a => a.BuyNowPrice,
						(s, a) => s.Random.Bool() ? decimal.Round(s.Random.Decimal(a.StartingPrice, 20000)) : (decimal?)null)
					.RuleFor(a => a.CreatedByUserName, s => userName);

			var randomizer = new Randomizer();
			var numberOfAuctions = randomizer.Number(userName == PredefinedUserName ? 30 : 0, 50);
			return auctionTemplateFaker.Generate(numberOfAuctions);
		}

		private AuctionCreatedEvent MapAuctionTemplateToAuctionCreatedEvent(AuctionTemplate auctionTemplate)
		{
			return new AuctionCreatedEvent
			{
				CreatedByUserName = auctionTemplate.CreatedByUserName,
				Id = Guid.NewGuid(),
				BuyNowPrice = auctionTemplate.BuyNowPrice,
				Description = auctionTemplate.Description,
				EndDateTime = auctionTemplate.EndDate,
				MinimalPriceForNextBidder = Math.Max(auctionTemplate.StartingPrice, 0.01m),
				StartingPrice = auctionTemplate.StartingPrice,
				Title = auctionTemplate.Title
			};
		}

		private IEnumerable<EventEnvelope<TEvent>> WrapIntoEnvelopeCollection<TEvent>(TEvent @event) where TEvent : IEvent
		{
			return new [] {  new EventEnvelope<TEvent>(@event, Guid.NewGuid()) };
		}

		private IReadOnlyCollection<AuctionTemplate> GetHardcodedAuctionTemplates()
		{
			return new[]
			{
				new AuctionTemplate("The Dark Side of the Moon 1973 vinyl",
					"A well-preserved original release of a timeless classic album from Pink Floyd.", _timeProvider.Now.AddMinutes(1),
					20, 80, "vinyl_shop"),
				new AuctionTemplate("Kyuss - Kyuss (Welcome to Sky Valley)",
					"I would like to sell an original 1994 release of the self-titled third album by Kyuss.",
					_timeProvider.Now.AddHours(2), 0, null, PredefinedUserName),
				new AuctionTemplate("Fiat Multipla 1.9 Multijet Eleganza 5dr",
					"The car may be ugly, but with only 100 000 kilometers of mileage it's a bargain!",
					_timeProvider.Now.AddDays(3).AddMinutes(77), 800, 1500, "family_guy")
			};
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

		private class AuctionTemplate
		{
			public AuctionTemplate()
			{
			}

			public AuctionTemplate(string title, string description, DateTime endDate, decimal startingPrice,
				decimal? buyNowPrice, string createdByUserName)
			{
				Title = title;
				Description = description;
				EndDate = endDate;
				StartingPrice = startingPrice;
				BuyNowPrice = buyNowPrice;
				CreatedByUserName = createdByUserName;
			}

			[UsedImplicitly]
			public string Title { get; set; }

			[UsedImplicitly]
			public string Description { get; set; }

			[UsedImplicitly]
			public DateTime EndDate { get; set; }

			[UsedImplicitly]
			public decimal StartingPrice { get; set; }

			[UsedImplicitly]
			public decimal? BuyNowPrice { get; set; }

			[UsedImplicitly]
			public string CreatedByUserName { get; set; }
		}
	}
}