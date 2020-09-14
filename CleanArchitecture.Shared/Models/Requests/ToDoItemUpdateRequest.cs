using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace AZV3CleanArchitecture.Models.Requests
{
    public class ToDoItemUpdateRequest : ToDoItemCreateRequest
    {
        [JsonProperty("id")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
