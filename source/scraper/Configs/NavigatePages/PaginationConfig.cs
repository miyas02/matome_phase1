using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs.NavigatePages {
    public class PaginationConfig {
        public NodeSelector Selector { get; set; } = new NodeSelector();
        public string Action { get; set; } = "click";
        public int? MaxLoop { get; set; }
    }
}
