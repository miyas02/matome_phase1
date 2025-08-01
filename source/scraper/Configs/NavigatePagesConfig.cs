using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public class NavigatePagesConfig {
        public TargetLinkConfig TARGET_LINK {
            get; set;
        }
        public PaginationConfig PAGINATION {
            get; set;
        }
    }
}
