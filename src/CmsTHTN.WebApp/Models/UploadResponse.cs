using System.Text.Json.Serialization;

namespace CmsTHTN.WebApp.Models
{
    public class UploadResponse
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
