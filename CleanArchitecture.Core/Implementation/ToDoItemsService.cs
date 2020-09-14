using AZV3CleanArchitecture.Models;
using AZV3CleanArchitecture.Options;
using AZV3CleanArchitecture.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AZV3CleanArchitecture.Services
{
    public class ToDoItemsService : IToDoItemsService
    {
        private readonly HttpClient httpClient;
        private readonly ToDoItemsServiceOptions toDoItemsServiceOptions;
        private readonly ILogger<ToDoItemsService> logger;

        public ToDoItemsService(HttpClient httpClient, IOptions<ToDoItemsServiceOptions> toDoItemsServiceOptions, ILogger<ToDoItemsService> logger)
        {
            this.httpClient = httpClient;
            this.toDoItemsServiceOptions = toDoItemsServiceOptions.Value;
            this.logger = logger;
        }

        public async Task<ToDoItem> GetToDoItem(int id)
        {
            logger.LogInformation($"Retrieving item: {{{Constants.TodoItemId}}}", id);

            var getUrl = $"{this.toDoItemsServiceOptions.BaseUrl.TrimEnd('/')}/todos/{id}";
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, getUrl))
            {
                using (var response = await this.httpClient.SendAsync(requestMessage))
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    logger.LogWarning($"Retrieved item: {{{Constants.TodoItemId}}}. Logged as warning for demo.", id);
                    return JsonConvert.DeserializeObject<ToDoItem>(responseString);
                }
            }
        }

        public async Task<IEnumerable<ToDoItem>> GetAllToDoItems(int id)
        {
            logger.LogInformation($"Retrieving all todo items");
            var getUrl = $"{this.toDoItemsServiceOptions.BaseUrl.TrimEnd('/')}/todos";
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, getUrl))
            {
                using (var response = await this.httpClient.SendAsync(requestMessage))
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<ToDoItem>>(responseString);
                }
            }
        }

        public async Task<ToDoItem> CreateToDoItem(ToDoItem toDoItem)
        {
            // call service and return the output

            return await Task.FromResult(new ToDoItem() { Id = 1, UserId = 1, Title = "Some Dummy Title", Completed = true });
        }

        public Task<ToDoItem> UpdateToDoItem(ToDoItem toDoItem)
        {
            throw new System.NotImplementedException();
        }
    }
}
