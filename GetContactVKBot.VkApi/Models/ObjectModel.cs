using Newtonsoft.Json;

namespace VkApi.Models
{
    public class ObjectModel
    {
        [JsonProperty("object_id")]
        public int ObjectId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}