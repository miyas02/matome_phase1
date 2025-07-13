using OpenQA.Selenium.DevTools.V136.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    internal interface IScraperOwner {
        //List<AbstractPost> GetPosts(AbstractScraperConfig scraperConfig)
        //AbstractScraperConfig ScraperConfig { get; set; }
        public AbstractScraperConfig ScraperConfigFactory(string logic);
        public void LoadConfig(string configPath);
    }
}
