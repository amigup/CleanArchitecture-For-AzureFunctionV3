using CleanArchitecture.Shared.Contracts;
using Microsoft.Azure.Cosmos.Table;

namespace CleanArchitecture.Core.Models
{
	public class MessageTable : TableEntity, IMessageTable
	{
		public MessageTable()
		{

		}

		public MessageTable(string partitionKey, string rowKey)
			: base (partitionKey, rowKey)
		{
		}

		public string TableName => "Messages";

		public string Message { get; set; }
	}
}
