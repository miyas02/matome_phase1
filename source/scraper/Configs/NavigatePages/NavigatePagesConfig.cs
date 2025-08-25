using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs.NavigatePages {
    public class NavigatePagesConfig {
        [JsonPropertyName("TYPE")]
        public NavigatePageTypes Type { get; set; }
        [JsonPropertyName("TARGET_LINK")] 
        public TargetLinkConfig? TargetLink { get; set; } //null許容
        [JsonPropertyName("PAGINATION")]
        public PaginationConfig? Pagination { get; set; } //null許容
    }
}
