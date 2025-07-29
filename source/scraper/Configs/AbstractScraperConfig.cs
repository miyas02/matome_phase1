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
        /// <summary>
        /// urlを渡してHTMLを取得するメソッド
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>html</returns>
        protected string GetHtml(string url) {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // コンソール非表示
            service.SuppressInitialDiagnosticInformation = true; // DevToolsなどのログ非表示

            using (IWebDriver driver = new ChromeDriver(service, options)) {
                driver.Navigate().GoToUrl(url);
                return driver.PageSource;
            }
        }
        public abstract List<Object> GetItems();

        public abstract string URL {
            get; set;
        }
        public abstract string SITE_NAME {
            get; set;
        }
        public abstract string LOGIC {
            get; set;
        }
        public List<PageConfig> PAGES {
       get; set;
        }
    }
}
