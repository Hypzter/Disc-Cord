using System.Text.Json.Serialization;

namespace Disc_Cord.Models
{
    public class Forum
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        public List<Models.Subforum>? SubForums { get; set; }
    }
}
