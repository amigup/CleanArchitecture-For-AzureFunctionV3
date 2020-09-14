using AZV3CleanArchitecture.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Functions.IntegrationTests
{
    public class GetToDoItemsTests
    {
        [Fact]
        public async Task Test_GetTodoItems_ValidInput()
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync("http://localhost:7071/api/todo/1");
                var item = JsonConvert.DeserializeObject<ToDoItem>(result);
                Assert.NotNull(item);
                Assert.Equal(1, item.Id);
            }
        }
    }
}
