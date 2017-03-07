using System;

namespace AuctionHouse.Domain.Money
{
	// TODO: Implement a real money type
	public static class MoneyValidator
	{
		public static void ValidateMoneyAmount(decimal moneyAmount)
		{
			if (decimal.Round(moneyAmount, 2) != moneyAmount)
			{
				throw new ArgumentOutOfRangeException($"{moneyAmount} has too many decimal places.");
			}
		}
	}
}