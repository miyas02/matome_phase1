using matome_phase1.scraper.Configs;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    public interface IScraperService {
        public IWebDriver GetDriver(string url);
        //protected void NavigateToPage(IWebDriver driver, AbstractScraperConfig AConfig);
        public List<System.Object> GetItems(AbstractScraperConfig AConfig);
    }
}
