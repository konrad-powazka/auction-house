using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Test;
using AuctionHouse.Persistence;
using Bogus;
using JetBrains.Annotations;

namespace AuctionHouse.Application.Test
{
	public class TestDataService : ICommandHandler<PopulateDatabaseWithTestDataCommand>
	{
		private readonly IRepository<Auction> _repository;
		private readonly ITimeProvider _timeProvider;

		public TestDataService(IRepository<Auction> repository, ITimeProvider timeProvider)
		{
			_repository = repository;
			_timeProvider = timeProvider;
		}

		public async Task Handle(CommandEnvelope<PopulateDatabaseWithTestDataCommand> commandEnvelope)
		{
			const int numberOfAuctionsToGenerate = 502;

			var auctionTemplates = GetHardcodedAuctionTemplates().Take(numberOfAuctionsToGenerate).ToList();

			if (auctionTemplates.Count < numberOfAuctionsToGenerate)
			{
				var auctionTemplateFaker = new Faker<AuctionTemplate>()
					.RuleFor(a => a.Title, s =>
					{
						var title = s.Commerce.Product();
						return title;
					})
					.RuleFor(a => a.Description, s =>
					{
						var numberOfParagraphs = s.Random.Int(1, 4);
						return s.Lorem.Paragraph(numberOfParagraphs);
					})
					.RuleFor(a => a.EndDate, s => s.Date.Between(_timeProvider.Now.AddMinutes(1), _timeProvider.Now.AddDays(21)))
					.RuleFor(a => a.StartingPrice, s => s.Random.Bool() ? s.Random.Decimal(1, 10000) : 0)
					.RuleFor(a => a.BuyNowPrice, (s, a) => s.Random.Bool() ? s.Random.Decimal(a.StartingPrice, 20000) : (decimal?) null)
					.RuleFor(a => a.CreatedByUserName, s =>
					{
						var separator = s.Random.Bool() ? "_" : string.Empty;
						return $"{s.Name.FirstName()}{separator}{s.Name.LastName()}".ToLower();
					});

				var randomizedAuctionTemplates = auctionTemplateFaker.Generate(numberOfAuctionsToGenerate - auctionTemplates.Count());
				auctionTemplates.AddRange(randomizedAuctionTemplates);
			}

			foreach (var auctionTemplate in auctionTemplates)
			{
				var auction = Auction.Create(Guid.NewGuid(), auctionTemplate.Title, auctionTemplate.Description,
					auctionTemplate.EndDate, auctionTemplate.StartingPrice, auctionTemplate.BuyNowPrice,
					auctionTemplate.CreatedByUserName, _timeProvider);

				// TODO: parallel
				await _repository.Create(auction, Guid.NewGuid().ToString());
			}
		}

		private IEnumerable<AuctionTemplate> GetHardcodedAuctionTemplates()
		{
			return new[]
			{
				new AuctionTemplate("The Dark Side of the Moon 1973 vinyl",
					"A well-preserved original release of a timeless classic album from Pink Floyd.", _timeProvider.Now.AddMinutes(1),
					20, 80, "vinyl_shop"),
				new AuctionTemplate("Kyuss - Kyuss (Welcome to Sky Valley)",
					"I would like to sell an original 1994 release of the self-titled third album by Kyuss.",
					_timeProvider.Now.AddHours(2), 0, null, "stoner_fan"),
				new AuctionTemplate("Fiat Multipla 1.9 Multijet Eleganza 5dr",
					"The car may be ugly, but with only 100 000 kilometers of mileage it's a bargain!",
					_timeProvider.Now.AddDays(3).AddMinutes(77), 800, 1500, "family_guy")
			};
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