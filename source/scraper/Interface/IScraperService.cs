using matome_phase1.scraper.Configs.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Interface {
    public interface IScraperService {
        //protected IWebDriver GetDriver(string url);
        //protected IWebDriver NavigateToPage(IWebDriver driver, AbstractScraperConfig AConfig);
        public List<object> GetItems(AbstractScraperConfig AConfig);
    }
}
