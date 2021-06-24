using Newtonsoft.Json;

namespace VkApi.Models
{
    public class RootModel<T>
    {
        [JsonProperty("response")]
        public T Response { get; set; }
    }
}