using System;
using System.Collections.Generic;

namespace AuctionHouse.Core.Collections
{
	public static class DictionaryExtensions
	{
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
			TKey key, Func<TValue> valueCreator)
		{
			TValue value;

			if (dictionary.TryGetValue(key, out value))
			{
				return value;
			}

			value = valueCreator();
			dictionary.Add(key, value);
			return value;
		}

		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
			TKey key, TValue value)
		{
			return dictionary.GetOrAdd(key, () => value);
		}
	}
}