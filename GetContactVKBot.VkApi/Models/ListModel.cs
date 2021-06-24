using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkApi.Models
{
    public class ListModel<T>
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}