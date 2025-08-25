using matome_phase1.scraper.Configs.NavigatePages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public abstract class AbstractScraperConfig {
        public abstract string URL {
            get; set;
        }
        public abstract string SITE_NAME {
            get; set;
        }
        public abstract string LOGIC {
            get; set;
        }
        public List<NavigatePagesConfig> NAVIGATE_PAGES {
       get; set;
        }
    }
}
