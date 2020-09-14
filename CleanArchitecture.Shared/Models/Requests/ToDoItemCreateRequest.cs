using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace AZV3CleanArchitecture.Models.Requests
{
    public class ToDoItemCreateRequest
    {
        [JsonProperty("userId")]
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [JsonProperty("title")]
        [Required]
        [StringLength(10)]
        public string Title { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }
    }
}
