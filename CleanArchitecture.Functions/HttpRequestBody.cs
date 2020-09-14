using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AZV3CleanArchitecture.Models.Requests
{
    internal class HttpRequestBody<T>
    {
        public bool IsValid { get; set; }

        public T Request { get; set; }

        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}
