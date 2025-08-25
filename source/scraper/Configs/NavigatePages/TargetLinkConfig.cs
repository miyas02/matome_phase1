using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs.NavigatePages {
    public class TargetLinkConfig {
        [JsonPropertyName("SELECTOR")]
        public NodeSelector Selector {
            get; set;
        } = new NodeSelector();
        [JsonPropertyName("TEXT")]
        public string? Text {
            get; set;
        }
    }
}
