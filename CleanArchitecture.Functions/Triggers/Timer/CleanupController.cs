using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Providers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Functions.Triggers.Timer
{
	public class CleanupController
    {
		private readonly IMessageService messageService;

		public CleanupController(IMessageService messageService)
		{
			this.messageService = messageService;
		}

        [FunctionName("MessageCleanup")]
        public async Task MessageCleanupAsync([TimerTrigger("%TimerScheduleExpression%")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Timer trigger function executed at: {DateTime.Now}");
            await this.messageService.CleanupAsync("Messages", PartitionKeyProvider.GetPartitionKey(DateTime.UtcNow.AddDays(-1)));
        }
    }
}
