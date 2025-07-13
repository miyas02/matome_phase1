using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    public abstract class AbstractScraperConfig {
        public abstract void Scraping();

        public abstract string URL {
            get; set;
        }
        public abstract string SITE_NAME {
            get; set;
        }
        public abstract string LOGIC {
            get; set;
        }
    }
}
