using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public class ConfigNodeBase {
        [JsonPropertyName("TYPE")]
        public string? TYPE { get; set; }
        [JsonPropertyName("NODE")]
        public string? NODE {  get; set; }
        [JsonPropertyName("ATTRIBUTE")]
        public string? ATTRIBUTE { get; set; }
        [JsonPropertyName("REGEX")]
        public string? REGEX { get; set; }

    }
}
