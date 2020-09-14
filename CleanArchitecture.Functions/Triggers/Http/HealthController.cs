using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Functions.Triggers.Http
{
    public class HealthController
    {
        [FunctionName("HealthCheck")]
        public async Task<IActionResult> GetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req, ILogger log)
        {
            return await Task.FromResult(new StatusCodeResult((int)HttpStatusCode.OK)); // all good
        }
    }
}
