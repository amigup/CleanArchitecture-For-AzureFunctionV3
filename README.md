# CleanArchitecture-For-AzureFunctionV3
This solution structure is a reference implementation of [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html "Clean Architecture") using Azure Functions V3 as the host.

## Overview
Following features are covered in the reference implementation.

1.	[**Dependency Injection**](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection) – abstracts to decouple volatile dependencies from their implementation. A volatile dependency is a class or module that, among other things, can contain nondeterministic behaviour or in general is something we which to be able to replace or intercept which provides components segregation and testability of components. 

2.	**Input data validation** – validates the input for your API contract so that business logic only handles the business logic. With the right input data validation, the over and under data posting can be saved which leads to application more secure. Also, having input data validation at first line of défense gives to create lesser downstream objects (and their lifecycle management in GC).

3.	**Logging and scope** – Scope defines so that all the logging at the nth invocation have the required tracing. For example, instead of passing CorrelationId to every method, the logging scope can be defined with CorrelationId at entry method which makes sure that all the logs in chain will have this property logged automatically. We can also utilize the nested scopes to 

```CSharp
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
            
```

4.	Application insights initializer – Adds the global properties in each trace/request/exception. Since, all the applications uses same GEO specific application insights so any application specific property would assist to filter to appropriate logs. For example, if you want to know which application uses most logging by common components.
5.	Correlation provider – Provides the capability to get the corelation Id for downstream methods.
6.	HttpContext accessor – to access the HttpContext of request.
7.	Typed HttpClient – Provides capability to send a http request without managing life cycle of http client.
8.	Retry policy – logic to retries a request in cased of expected failures.
9.	Integration Tests
10.	Unit Tests
11.	Separation of layers
12.	Health Endpoints
