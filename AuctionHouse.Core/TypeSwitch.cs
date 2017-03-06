using System;

namespace AuctionHouse.Core
{
	// Based on https://blogs.msdn.microsoft.com/jaredpar/2008/05/16/switching-on-types/
	// TODO: To fluent API
	public static class TypeSwitch
	{
		public static void Do(object source, params CaseInfo[] cases)
		{
			var type = source.GetType();

			foreach (var entry in cases)
			{
				if (entry.IsDefault || entry.Target.IsAssignableFrom(type))
				{
					entry.Action(source);

					break;
				}
			}
		}

		public static CaseInfo Case<T>(Action action)
		{
			return new CaseInfo
			{
				Action = x => action(),
				Target = typeof(T)
			};
		}

		public static CaseInfo Case<T>(Action<T> action)
		{
			return new CaseInfo
			{
				Action = x => action((T) x),
				Target = typeof(T)
			};
		}

		public static CaseInfo Default(Action action)
		{
			return new CaseInfo
			{
				Action = x => action(),
				IsDefault = true
			};
		}

		public class CaseInfo
		{
			public bool IsDefault { get; set; }

			public Type Target { get; set; }

			public Action<object> Action { get; set; }
		}
	}
}