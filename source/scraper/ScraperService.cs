
using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Script;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace matome_phase1.scraper {
    public abstract class ScraperService : IScraperService {
        /// <summary>
        /// urlを渡してDriverを取得するメソッド
        /// 具象クラスのGetItems()メソッドで使用される
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>Driver</returns>
        private IWebDriver GetDriver(string url) {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // コンソール非表示
            service.SuppressInitialDiagnosticInformation = true; // DevToolsなどのログ非表示

            IWebDriver driver = new ChromeDriver(service, options);
            driver.Navigate().GoToUrl(url);
            return driver;
        }
        private IWebDriver NavigateToPage(IWebDriver driver, AbstractScraperConfig AConfig) {
            //NavigatePagesConfig nullチェック
            if (AConfig.NAVIGATE_PAGES == null || AConfig.NAVIGATE_PAGES.Count == 0) {
                throw new Exception(Constants.NavigateToPagesIsNull);
            }
            //NavigatePagesConfigのListの各要素を取り出す
            foreach (var navi in AConfig.NAVIGATE_PAGES) {
                if (navi.Type == Configs.NavigatePages.NavigatePageTypes.pagination_search) {
                    driver.FindElement(By.XPath(navi.TargetLink.Selector.NODE));
                }

                if (navi.Type == Configs.NavigatePages.NavigatePageTypes.search) {

                }
            }

            return driver;
        }

        public List<System.Object> GetItems(AbstractScraperConfig AConfig) {

            IWebDriver driver = GetDriver(AConfig.URL);
            driver = NavigateToPage(driver, AConfig);

            string html = driver.PageSource;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            driver.Quit(); // ドライバーを閉じる
            return DocParseItems(AConfig, doc);
        }

        protected abstract List<System.Object> DocParseItems(AbstractScraperConfig AConfig, HtmlDocument doc);
    }
}
