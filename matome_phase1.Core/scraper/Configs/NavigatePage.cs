using System.Text.Json.Serialization;

namespace matome_phase1.scraper.Configs {
    public class ConfigNodeBase {
        [JsonPropertyName("TYPE")]
        public string? TYPE { get; set; }
        [JsonPropertyName("NODE")]
        public string? NODE { get; set; }
        [JsonPropertyName("ATTRIBUTE")]
        public string? ATTRIBUTE { get; set; }
        [JsonPropertyName("REGEX")]
        public string? REGEX { get; set; }
    }
    public class TARGETLINK : ConfigNodeBase {
        [JsonPropertyName("TEXT")]
        public string? TEXT { get; set; }
    }
    public class NavigatePage {
        [JsonPropertyName("TYPE")]
        public NavigatePageTypes TYPE { get; set; }

        [JsonPropertyName("TARGET_LINK")]
        public TARGETLINK? TARGET_LINK { get; set; }

        [JsonPropertyName("PAGINATION")]
        public Pagination? PAGINATION { get; set; }
    }
    public class Pagination {
        [JsonPropertyName("NODE")]
        public string? NODE { get; set; }

        [JsonPropertyName("ACTION")]
        public string? ACTION { get; set; }

        [JsonPropertyName("MAX_LOOP")]
        public int? MAX_LOOP { get; set; }
    }
}
