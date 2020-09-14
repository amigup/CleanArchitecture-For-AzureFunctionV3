using AZV3CleanArchitecture.Extensions;
using AZV3CleanArchitecture.Mappers;
using AZV3CleanArchitecture.Models.Requests;
using AZV3CleanArchitecture.Providers;
using AZV3CleanArchitecture.Services;
using CleanArchitecture.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZV3CleanArchitecture.Triggers.Http
{
    public class ToDoItemsController
    {
        private readonly IToDoItemsService toDoItemsService;
        private readonly ICorrelationProvider correlationProvider;

        public ToDoItemsController(IToDoItemsService toDoItemsService, ICorrelationProvider correlationProvider)
        {
            this.toDoItemsService = toDoItemsService;
            this.correlationProvider = correlationProvider;
        }

        [FunctionName("GetToDo")]
        public async Task<IActionResult> GetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req, int id, ILogger log)
        {
            using (log.BeginScope(new Dictionary<string, object>()
            {
                [Constants.FunctionName] = "GetToDo",
                ["Outer"] = "Outer",
            }))
            {
                log.LogInformation(EventConstants.GetTodoEventId, $"Fetching to do {{{Constants.TodoItemId}}}", id);

                using (log.BeginScope(new Dictionary<string, object>()
                {
                    [Constants.CorrelationIdHeader] = correlationProvider.GetCorrelationId(),
                    ["Inner"] = "Inner",
                }))
                {
                    log.LogInformation(EventConstants.GetTodoEventId, $"This should have both CorrelationIdHeader and function name Fetching to do {{{Constants.TodoItemId}}}", id);
                    var todoItem = await toDoItemsService.GetToDoItem(id);
                    return new OkObjectResult(todoItem);
                }
            }
        }

        [FunctionName("CreateToDo")]
        public async Task<IActionResult> PostAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            using (log.BeginScope(new Dictionary<string, object>()
            {
                [Constants.CorrelationIdHeader] = correlationProvider.GetCorrelationId()
            }))
            {
                var inputRequest = await req.GetBodyAsync<ToDoItemCreateRequest>();
                if (!inputRequest.IsValid)
                {
                    var errorMessage = $"Model is invalid: {string.Join(", ", inputRequest.ValidationResults.Select(s => s.ErrorMessage).ToArray())}";
                    log.LogInformation(errorMessage);
                    var error = new { Error = errorMessage };
                    return new BadRequestObjectResult(error);
                }

                var itemToCreate = ToDoItemRequestMapper.MapToDoItemRequest(inputRequest.Request);
                var item = await toDoItemsService.CreateToDoItem(itemToCreate);
                return new CreatedResult($"/todo/{item.Id}", item);
            }
        }
    }
}
