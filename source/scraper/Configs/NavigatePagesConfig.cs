using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public class NavigatePagesConfig {
        public string Type { get; set; } = string.Empty;
        public TargetLinkConfig? TargetLink { get; set; }
        public PaginationConfig? Pagination { get; set; }
    }
}
