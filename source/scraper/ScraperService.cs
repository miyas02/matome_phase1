
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
                throw new ConfigException(ScraperExceptionType.NavigateToPagesIsNull,AConfig);
            }
            //NavigatePagesConfigのListの各要素を取り出す
            foreach (var navi in AConfig.NAVIGATE_PAGES) {
                if (navi.TYPE == NavigatePageTypes.pagination_search) {
                    
                    //configのNodeとリンクテキストを検索
                    string text = navi.TARGET_LINK.NODE + "//*[contains(text(),'" + navi.TARGET_LINK.TEXT + "')]";

                    var n = driver.FindElements(By.XPath(text));
                    if (n.Count > 0 ){
                        var node = n[0];
                        node.Click();
                    } else {
                        driver = Pagination(driver, navi);
                    }                  
                }
                if (navi.TYPE == NavigatePageTypes.search) {

                }
            }

            return driver;
        }

        private IWebDriver Pagination(IWebDriver driver, NAVIGATEPAGE navi) {
            while(true) {
                //configのNodeとリンクテキストを検索
                string text = navi.TARGET_LINK.NODE + "[contains(text(),'" + navi.TARGET_LINK.TEXT + "')]";
                var nodes = driver.FindElements(By.XPath(text));
                if (nodes.Count > 0) {
                    nodes[0].Click();
                    return driver;
                }
                if (nodes.Count == 0) {
                    var paginationNode = driver.FindElement(By.XPath(navi.PAGINATION.NODE ?? throw new ConfigException(ScraperExceptionType.ContentNodeIsNull)));
                    if(paginationNode == null) {
                        throw new ConfigException(ScraperExceptionType.ContentNodeIsNull,null, "paginationNode");
                    }
                    paginationNode.Click();
                }
                
            }
        }
    }
}
