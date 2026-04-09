using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KomorebiLyrs.Models;

public class TunaData
{
     [JsonPropertyName("data")]
     public TunaTrackData Data { get; set; } = new();

     public class TunaTrackData
    {
        [JsonPropertyName("title")] public string? Title { get; set; }
        [JsonPropertyName("artists")] public List<string>? Artists { get; set; }
        [JsonPropertyName("status")] public string? Status { get; set; } // "playing", "paused"
        [JsonPropertyName("progress")] public double Progress { get; set; }
        [JsonPropertyName("duration")] public double Duration { get; set; }
        [JsonPropertyName("cover_url")] public string? CoverUrl { get; set; }
        [JsonPropertyName("cover")] public string? CoverBase64 { get; set; }
        [JsonPropertyName("lyrics")] public Dictionary<string,List<string>>? Lyrics { get; set; }
     }

}