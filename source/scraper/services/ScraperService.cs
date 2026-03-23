
using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
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
    public class ScraperService : IScraperService {
        public List<Dictionary<string, string>> GetItems(ScraperConfig scraperConfig) {

            IWebDriver driver = GetDriver(scraperConfig.URL);
            try {
                driver = NavigateToPage(driver, scraperConfig);
                //スクロールして読み込み
                EnsureLoadedItems(driver, scraperConfig);
                string html = driver.PageSource;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return DocParseItems(scraperConfig, doc);
            } finally {
                driver.Quit();
            }
        }

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
        private IWebDriver NavigateToPage(IWebDriver driver, ScraperConfig scraperConfig) {
            //NavigatePagesConfig nullチェック
            if (scraperConfig.NAVIGATE_PAGES == null || scraperConfig.NAVIGATE_PAGES.Count == 0) {
                return driver;
            }
            //NavigatePagesConfigのListの各要素を取り出す
            foreach (NavigatePage navi in scraperConfig.NAVIGATE_PAGES) {
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

        private IWebDriver Pagination(IWebDriver driver, NavigatePage navi) {
            for (int i = 0; i < 50; i++) {
                //configのNodeとリンクテキストを検索
                string text = navi.TARGET_LINK.NODE + "[contains(text(),'" + navi.TARGET_LINK.TEXT + "')]";
                var nodes = driver.FindElements(By.XPath(text));
                if (nodes.Count > 0) {
                    nodes[0].Click();
                    return driver;
                }
                if (nodes.Count == 0) {
                    var paginationNodes = driver.FindElements(By.XPath(navi.PAGINATION.NODE ?? throw new ConfigException(ScraperExceptionType.ContentNodeIsNull)));
                    if (paginationNodes == null || paginationNodes.Count == 0) {
                        throw new ConfigException(ScraperExceptionType.NavigateToPagesIsNull, null, "paginationNode is Null");
                    }
                    paginationNodes[0].Click();
                }
            }
            throw new ConfigException(ScraperExceptionType.NavigateToPagesIsNull, null, "paginationNode is Null");
        }

        internal List<Dictionary<string, string>> DocParseItems(ScraperConfig scraperConfig, HtmlDocument doc) {
            Dictionary<string, ExtractDef> extractDict = scraperConfig.EXTRACT;
            var items = new List<Dictionary<string, string>>();
            foreach (var (key, extractDef) in extractDict) { //key=posts, extractDef=ExtractDef

                HtmlNode? contentNode = doc.DocumentNode.SelectSingleNode(extractDef.CONTEXT) ?? throw new ConfigException(ScraperExceptionType.ContentNodeIsNull);
                List<HtmlNode> nodes = new List<HtmlNode>();
                if (contentNode.SelectNodes(extractDef.ITEM) != null) {
                    nodes.AddRange(contentNode.SelectNodes(extractDef.ITEM) ?? Enumerable.Empty<HtmlNode>());
                }

                foreach (var node in nodes) {
                    var item = new Dictionary<string, string>();
                    foreach (var (fieldKey, fieldValue) in extractDef.FIELDS) {
                        item.Add(fieldKey, GetValue(node, fieldValue));
                    }
                    items.Add(item);
                }
            }
            return items;
        }
        /// <summary>
        /// ノードから値を取得する
        /// </summary>
        /// <param name="postNode"></param>
        /// <param name="configNode"></param>
        /// <returns></returns>
        private string GetValue(HtmlNode postNode, ExtractDef configNode) {
            string value = null;
            if (configNode.TYPE == "attribute") {
                value = GetAttributeValue(postNode, configNode.NODE, configNode.ATTRIBUTE);
            }
            if (configNode.TYPE == "text") {
                value = GetInnerText(postNode, configNode.NODE);
            }
            if (configNode.REGEX != null) {
                value = GetInnerText(postNode, configNode.NODE);
                var match = Regex.Match(value, configNode.REGEX);
                if (match.Groups.Count > 0) {
                    value = match.Groups[0].Value.Trim();
                    return value;
                }
                return "";
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postNode"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static string GetInnerText(HtmlNode postNode, string node) {
            var bodyNode = postNode.SelectSingleNode(node);
            return bodyNode?.InnerText.Trim() ?? "";
        }
        private static string GetAttributeValue(HtmlNode postNode, string node, string attribute) {
            var bodyNode = postNode.SelectSingleNode(node);
            return bodyNode?.GetAttributeValue(attribute, "").Trim() ?? "";
        }
        /// <summary>
        /// スクロールしてitemを全て読み込む
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="containerBy"></param>
        /// <param name="itemBy"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        internal IReadOnlyCollection<IWebElement> EnsureLoadedItems(IWebDriver driver, ScraperConfig scraperConfig) {
            Dictionary<string, ExtractDef> extractDict = scraperConfig.EXTRACT;
            foreach (var (key, extractDef) in extractDict) {
                var timeout = TimeSpan.FromSeconds(50);
                var end = DateTime.UtcNow + timeout;
                var container = driver.FindElement(By.XPath(extractDef.CONTEXT));
                int previousCount = -1;


                while (DateTime.UtcNow < end) {
                    var items = container.FindElements(By.XPath(extractDef.ITEM));
                    if (items.Count > previousCount) {
                        previousCount = items.Count;
                        var ret = ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'auto', block:'end'});", items.Last());
                        Thread.Sleep(1000);
                        continue;
                    }
                    return items;
                }
                return container.FindElements(By.XPath(extractDef.ITEM));
            }
            throw new NotImplementedException();
        }
    }
}
