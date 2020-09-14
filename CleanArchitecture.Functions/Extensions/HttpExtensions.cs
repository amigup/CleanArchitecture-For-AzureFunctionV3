using AZV3CleanArchitecture.Models.Requests;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace AZV3CleanArchitecture.Extensions
{
    internal static class HttpExtensions
    {
        internal static async Task<HttpRequestBody<T>> GetBodyAsync<T>(this HttpRequest request)
        {
            var body = new HttpRequestBody<T>();
            var content = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                body.Request = JsonConvert.DeserializeObject<T>(content);

                var results = new List<ValidationResult>();
                body.IsValid = Validator.TryValidateObject(body.Request, new ValidationContext(body.Request, null, null), results, true);
                body.ValidationResults = results;
            }
            catch (JsonReaderException ex)
            {
                body.IsValid = false;
                body.ValidationResults = new List<ValidationResult>() { new ValidationResult($"Error occurred during parsing the body. Exception Message : {ex.Message}") };
            }

            return body;
        }
    }
}
