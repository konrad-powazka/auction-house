using System;
using Bogus.DataSets;

namespace AuctionHouse.FakeDataGeneratorLauncher
{
	public static class BogusExtensions
	{
		public static DateTime BetweenUtc(this Date bogusDate, DateTime from, DateTime to)
		{
			var unspecifiedKindDate = bogusDate.Between(from.ToUniversalTime(), to.ToUniversalTime());
			return DateTime.SpecifyKind(unspecifiedKindDate, DateTimeKind.Utc);
		}
	}
}