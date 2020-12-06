using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using AZV3CleanArchitecture.ApplicationInsightsInitializers;
using AZV3CleanArchitecture.Extensions;
using AZV3CleanArchitecture.Options;
using AZV3CleanArchitecture.Providers;
using AZV3CleanArchitecture.Services;
using CleanArchitecture.Core.Implementation;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.ApplicationInsightsInitializers;
using CleanArchitecture.Infrastructure.DelegatingHandlers;
using CleanArchitecture.Infrastructure.Options;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;

[assembly: FunctionsStartup(typeof(AZV3CleanArchitecture.Startup))]

namespace AZV3CleanArchitecture
{
	public class Startup : FunctionsStartup
    {
        private const string ToDoItemsService = "ToDoItemsService";
        private const string MessageService = "MessageService";
        private const string Authorization = "Authorization";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts =>
            {
                opts.SpecVersion = OpenApiSpecVersion.OpenApi2_0;
                opts.AddCodeParameter = true;
                opts.PrependOperationWithRoutePrefix = true;
                opts.Documents = new[]
                {
                    new SwaggerDocument
                    {
                        Name = "v1",
                        Title = "Swagger document",
                        Description = "Swagger test document",
                        Version = "v2"
                    }
                };
                opts.Title = "Swagger Test";
                //opts.OverridenPathToSwaggerJson = new Uri("http://localhost:7071/api/Swagger/json");
                opts.ConfigureSwaggerGen = (x =>
                {
                    x.CustomOperationIds(apiDesc =>
                    {
                        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)
                            ? methodInfo.Name
                            : new Guid().ToString();
                    });

                    x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. 
                                        Enter 'Bearer' [space] and then your token in the text input below.
                                        Example: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                });
            });

            builder.Services.Replace(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(OptionsFactory<>)));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ITelemetryInitializer, CorrelationTelemetryInitializer>();
            builder.Services.AddSingleton<ITelemetryInitializer, ApplicationNameTelemetryInitializer>();

            builder.Services.AddOptions<ToDoItemsServiceOptions>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection(ToDoItemsService).Bind(settings);
                            });

            builder.Services.AddOptions<AuthorizationOptions>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection(Authorization).Bind(settings);
                            }).ValidateDataAnnotations();

            var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            builder.Services.AddTransient<CorrelationDelegatingHandler>();
            builder.Services.AddTransient<HeaderLoggingDelegatingHandler>();
            builder.Services.AddScoped<ICorrelationProvider, CorrelationProvider>();
            builder.Services.AddHttpClient<IToDoItemsService, ToDoItemsService>(c =>
            {
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            })
            .AddPolicyHandler(RetryPolicies.GetTooManyRequestsRetryPolicy(exponentialBackoffRetry: false))
            .AddHttpMessageHandler<CorrelationDelegatingHandler>();

            builder.Services.AddOptions<MessageServiceOptions>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection(MessageService).Bind(settings);
                            });
            builder.Services.AddSingleton<IMessageService, MessageService>();
        }
    }
}
