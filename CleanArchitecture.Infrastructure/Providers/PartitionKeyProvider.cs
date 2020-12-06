using System;

namespace CleanArchitecture.Infrastructure.Providers
{
	public static class PartitionKeyProvider
	{
		public static string GetPartitionKey(DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-dd");
		}
	}
}
