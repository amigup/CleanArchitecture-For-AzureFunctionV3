using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Infrastructure.Options;
using CleanArchitecture.Shared.Contracts;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Implementation
{
	public class MessageService : IMessageService
	{
		private readonly MessageServiceOptions messageServiceOptions;
		private readonly ILogger<MessageService> logger;

		private CloudStorageAccount cloudStorageAccount;

		public MessageService(IOptions<MessageServiceOptions> toDoItemsServiceOptions, ILogger<MessageService> logger)
		{
			this.messageServiceOptions = toDoItemsServiceOptions.Value;
			this.logger = logger;
			this.cloudStorageAccount = this.CreateStorageAccount(this.messageServiceOptions.MessageServiceStorageConnectionString);
		}

		public async Task<TableEntity> InsertOrMergeAsync<T>(T record) where T : IMessageTable, ITableEntity
		{
			var table = await CreateTableAsync((record as IMessageTable).TableName);
			TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(record as ITableEntity);
			TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
			return result.Result as TableEntity;
		}

		public async Task CleanupAsync(string entityName, string partition)
		{
			var table = await CreateTableAsync(entityName);
			var messages = table.CreateQuery<MessageTable>()
				.Where(x => x.PartitionKey == partition)
				.Select(x => new MessageTable() { PartitionKey = partition, RowKey = x.RowKey });

			TableBatchOperation tableOperations = new TableBatchOperation();
			foreach (var message in messages)
			{
				tableOperations.Add(TableOperation.Delete(message));
			}

			await table.ExecuteBatchAsync(tableOperations);
		}

		private CloudStorageAccount CreateStorageAccount(string connectionString)
		{
			CloudStorageAccount storageAccount;
			try
			{
				storageAccount = CloudStorageAccount.Parse(connectionString);
			}
			catch (FormatException formatException)
			{
				this.logger.LogError(formatException, "Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file.");
				throw;
			}
			catch (ArgumentException argumentException)
			{
				this.logger.LogError(argumentException, "Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file.");
				throw;
			}

			return storageAccount;
		}

		private async Task<CloudTable> CreateTableAsync(string tableName)
		{
			// Create a table client for interacting with the table service
			CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

			// Create a table client for interacting with the table service 
			CloudTable table = tableClient.GetTableReference(tableName);
			if (await table.CreateIfNotExistsAsync())
			{
				this.logger.LogInformation("Created Table named: {0}", tableName);
			}
			
			return table;
		}
	}
}
