﻿namespace AuctionHouse.Web.Models
{
	public class User
	{
		public User(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}