using System.Text.Json.Serialization;

namespace StealAllTheCats.Application.Services
{
    // take json attributes from url and then turn them to cat entity mostly due to breed
    public class CatApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("width")]
        public int width {  get; set; }
        [JsonPropertyName("url")]
        public string url { get; set; }
        [JsonPropertyName("height")]
        public int height { get; set; }
        [JsonPropertyName("breeds")]
        public List<Breed> Breeds { get; set; }

    }
}
