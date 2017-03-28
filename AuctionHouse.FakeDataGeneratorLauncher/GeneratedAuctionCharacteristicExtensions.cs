using System.Linq;

namespace AuctionHouse.FakeDataGeneratorLauncher
{
	public static class GeneratedAuctionCharacteristicExtensions
	{
		public static bool CheckIfUserIsSelling(this GeneratedAuctionCharacteristic generatedAuctionCharacteristic)
		{
			return CheckIfValueIsContainedInCollection(generatedAuctionCharacteristic,
				GeneratedAuctionCharacteristic.UserSellingAndFinishedWithBids,
				GeneratedAuctionCharacteristic.UserSellingAndFinishedWithoutBids,
				GeneratedAuctionCharacteristic.UserSellingAndInProgress);
		}

		public static bool CheckIfAuctionIsInProgress(this GeneratedAuctionCharacteristic generatedAuctionCharacteristic)
		{
			return CheckIfValueIsContainedInCollection(generatedAuctionCharacteristic,
				GeneratedAuctionCharacteristic.UserMadeBidsAndInProgress,
				GeneratedAuctionCharacteristic.UserSellingAndInProgress);
		}

		public static bool CheckIfCanHaveBids(this GeneratedAuctionCharacteristic generatedAuctionCharacteristic)
		{
			return !CheckIfValueIsContainedInCollection(generatedAuctionCharacteristic,
				GeneratedAuctionCharacteristic.UserSellingAndFinishedWithoutBids);
		}

		public static bool CheckIfFinishesWithBuy(this GeneratedAuctionCharacteristic generatedAuctionCharacteristic)
		{
			return CheckIfValueIsContainedInCollection(generatedAuctionCharacteristic,
				GeneratedAuctionCharacteristic.UserMadeBidsAndWon, GeneratedAuctionCharacteristic.UserSellingAndFinishedWithBids,
				GeneratedAuctionCharacteristic.UserMadeBidsAndNotWon);
		}

		private static bool CheckIfValueIsContainedInCollection(GeneratedAuctionCharacteristic value,
			params GeneratedAuctionCharacteristic[] collection)
		{
			return collection.Contains(value);
		}
	}
}