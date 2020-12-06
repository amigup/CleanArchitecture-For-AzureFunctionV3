using CleanArchitecture.Shared.Contracts;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
	public interface IMessageService
	{
		Task<TableEntity> InsertOrMergeAsync<T>(T record) where T : IMessageTable, ITableEntity;

		Task CleanupAsync(string entityName, string partition);
	}
}
