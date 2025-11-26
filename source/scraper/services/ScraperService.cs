
using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Configs.EC;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Script;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace matome_phase1.scraper.services {
    public abstract class ScraperService : IScraperService {
        public List<System.Object> GetItems(AbstractScraperConfig AConfig) {

            IWebDriver driver = GetDriver(AConfig.URL);
            driver = NavigateToPage(driver, AConfig);
            //スクロールして読み込み
            EnsureLoadedItems(driver, AConfig);
            string html = driver.PageSource;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            driver.Quit(); // ドライバーを閉じる
            return DocParseItems(AConfig, doc);
        }

        internal abstract List<System.Object> DocParseItems(AbstractScraperConfig AConfig, HtmlDocument doc);
        internal abstract IReadOnlyCollection<IWebElement> EnsureLoadedItems(IWebDriver driver, AbstractScraperConfig AConfig);

        /// <summary>
        /// urlを渡してDriverを取得するメソッド
        /// 具象クラスのGetItems()メソッドで使用される
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>Driver</returns>
        private IWebDriver GetDriver(string url) {
            var options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-features=VizDisplayCompositor");
            options.AddArgument($"--user-data-dir={Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())}");

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
                return driver;
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

        private IWebDriver Pagination(IWebDriver driver, Configs.NAVIGATEPAGE navi) {
            while(true) {
                //configのNodeとリンクテキストを検索
                string text = navi.TARGET_LINK.NODE + "[contains(text(),'" + navi.TARGET_LINK.TEXT + "')]";
                var nodes = driver.FindElements(By.XPath(text));
                if (nodes.Count > 0) {
                    nodes[0].Click();
                    return driver;
                }
                if (nodes.Count == 0) {
                    var paginationNodes = driver.FindElements(By.XPath(navi.PAGINATION.NODE ?? throw new ConfigException(ScraperExceptionType.ContentNodeIsNull)));
                    if(paginationNodes == null || paginationNodes.Count == 0) {
                        throw new ConfigException(ScraperExceptionType.NavigateToPagesIsNull,null, "paginationNode is Null");
                    }
                    paginationNodes[0].Click();
                }
                
            }
        }
        /// <summary>
        /// スクロールしてitemを全て読み込む
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="containerBy"></param>
        /// <param name="itemBy"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        //public static IReadOnlyCollection<IWebElement> EnsureLoadedItems(IWebDriver driver, By containerBy, By itemBy, TimeSpan timeout) {
        //    var end = DateTime.UtcNow + timeout;
        //    var container = driver.FindElement(containerBy);
        //    int previousCount = -1;

        //    while (DateTime.UtcNow < end) {
        //        var items = container.FindElements(itemBy);
        //        if (items.Count > previousCount) {
        //            previousCount = items.Count;
        //            var ret = ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'auto', block:'end'});", items.Last());
        //            Thread.Sleep(1000);
        //            continue;
        //        }
        //        return items;
        //    }
        //    return container.FindElements(itemBy);
        //}

    }
}
