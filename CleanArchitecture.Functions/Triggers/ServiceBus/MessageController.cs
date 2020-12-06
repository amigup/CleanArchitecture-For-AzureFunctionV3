using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Infrastructure.Providers;
using CleanArchitecture.Shared.Models.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Functions.Triggers.ServiceBus
{
	public class MessageController
    {
		private readonly IMessageService messageService;

		public MessageController(IMessageService messageService)
		{
			this.messageService = messageService;
		}

        [FunctionName("MessageController")]
        public async Task MessageServiceAsync([ServiceBusTrigger("%ServiceBusQueueName%", Connection = "ServiceBusConnectionString")]string myQueueItem, ILogger log)
        { 
            var message = JsonConvert.DeserializeObject<MessageRequest>(myQueueItem);
            log.LogInformation($"InsertOrMegre the record MessageId: {message.Id}");
            await messageService.InsertOrMergeAsync(GetMessage(message));
        }

        private MessageTable GetMessage(MessageRequest message)
        {
            return new MessageTable(PartitionKeyProvider.GetPartitionKey(DateTime.UtcNow), message.Id.ToString()) { Message = message.Message };
        }
    }
}
