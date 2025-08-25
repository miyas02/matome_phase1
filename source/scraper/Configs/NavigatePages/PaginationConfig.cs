using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs.NavigatePages {
    public class PaginationConfig {
        [JsonPropertyName("SELECTOR")]
        public NodeSelector Selector { get; set; } = new NodeSelector();
        [JsonPropertyName("ACTION")]
        public string Action { get; set; } = "click";
        [JsonPropertyName("MAX_LOOP")]
        public int? MaxLoop { get; set; }
    }
}
