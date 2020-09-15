# CleanArchitecture-For-AzureFunctionV3
This solution structure is a reference implementation of [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html "Clean Architecture") using Azure Functions V3 as the host.

## Overview
Following features are covered in the reference implementation.

1.	[**Dependency Injection**](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection) – abstracts to decouple volatile dependencies from their implementation. A volatile dependency is a class or module that, among other things, can contain nondeterministic behaviour or in general is something we which to be able to replace or intercept which provides components segregation and testability of components. 

2.	[**Input data validation**](https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api) – validates the input for your API function against schema restrictions (e.g. Length restriction, Type restrictions, Required/Optional restrictions etc.) so that your core business logic class do not need to deal with that. With the right input data validation, the over and under data posting can be saved which makes the application more secure. Also, having input data validation at first line of défense gives to create lesser downstream objects (and their lifecycle management in GC).

3.	[**Logging and scope**](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection#logging-services) – Scope defines so that all the logging at the nth invocation have the required tracing. For example, instead of passing CorrelationId to every method, the logging scope can be defined with CorrelationId at entry method which makes sure that all the logs in chain will have this property logged automatically. We can also utilize the nested scopes to capture/represent information at various levels within a request lifecycle.

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

4.	[**Application insights initializer**](https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling#addmodify-properties-itelemetryinitializer) – Adds the global properties in each trace/request/exception. Since, all the applications uses same GEO specific application insights so any application specific property would assist to filter to appropriate logs. For example, if you want to know which application uses most logging by common components.
5.	[**Correlation provider**](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.delegatinghandler?view=netcore-3.1) – Provides the capability to get the corelation Id for downstream methods.
6.	[**HttpContext accessor**](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-3.1) – to access the HttpContext of a request in different layers of application.
7.	[**Typed HttpClient**](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests) – Provides capability to send a http request without managing life cycle of http client. We can register multiple HttpClient instances against same/different URLs based on your requirements and can use Dependency Injection to inject the instance in different application layers.
8.	[**Retry policy**](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly) – logic to add retries for a request in cased of expected failures from a downstream system. Current implementation uses [Polly](https://github.com/App-vNext/Polly) for reference purpose.
9.	[**Integration and Unit Tests**] - like any good application, we need to have unit test cases for core business logic and integration tests for CI validations.
10.	[**Separation of layers**] - Clean architecture layers are defined to keep stuff where it belongs.
