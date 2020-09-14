using AZV3CleanArchitecture.Models;
using AZV3CleanArchitecture.Options;
using AZV3CleanArchitecture.Providers;
using AZV3CleanArchitecture.Services;
using CleanArchitecture.Core.UnitTests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Core.UnitTests
{
    public class ToDoItemsServiceTests
    {
        [Fact]
        public async Task Test_GetAllToDoItems_ValidInputs()
        {
            var expectedResult = new List<ToDoItem>();
            expectedResult.Add(new ToDoItem() { Id = 1, Completed = false, Title = "Test title", UserId = 1 });

            var mockHttpClient = HttpClientHelper.CreateHttpClient(expectedResult);
            var todoService = new ToDoItemsService(mockHttpClient, GetOptions(), new NullLogger<ToDoItemsService>());
            var output = await todoService.GetAllToDoItems(1);
            Assert.NotNull(output);
            Assert.Single(output);
            Assert.Equal(expectedResult, output, new ToDoItemComparer());
        }

        private IOptions<ToDoItemsServiceOptions> GetOptions()
        {
            var options = new ToDoItemsServiceOptions()
            {
                BaseUrl = "http://baseUrl"
            };

            return Options.Create(options);
        }
    }
}
