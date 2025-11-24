using matome_phase1.scraper.Configs;
using OpenQA.Selenium.DevTools.V136.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using matome_phase1.scraper.Models;

namespace matome_phase1.scraper.Interface {
    internal interface IScraperOwner {
        IScraperService ScraperService {get; set;}
        AbstractScraperConfig AConfig { get; set; }
        public ItemsVM LoadConfig(string configPath);
    }
}
