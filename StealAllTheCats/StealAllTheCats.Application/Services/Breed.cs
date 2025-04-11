using System.Text.Json.Serialization;

namespace StealAllTheCats.Application.Services
{
    public class Breed
    {

        [JsonPropertyName("temperament")]
        public string Temperament {  get; set; }
    }
}
