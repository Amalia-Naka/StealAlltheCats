using System.Text.Json.Serialization;

namespace StealAllTheCats.DAL.Models
{
    public class CatTag
    {

        public int CatId { get; set; }
        [JsonIgnore]
        public Cat Cat { get; set; }

        public int TagId { get; set; }
        [JsonIgnore]
        public Tag Tag { get; set; }
    }
}
