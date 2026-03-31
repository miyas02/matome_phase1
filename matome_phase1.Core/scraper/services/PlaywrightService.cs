using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace matome_phase1.scraper.services {
    public class PlaywrighService : IScraperService {
        public async Task<List<Dictionary<string, string>>> GetItems(ScraperConfig scraperConfig) {

            var browser = await GetPlaywright();

            var page = await browser.NewPageAsync();
            await page.GotoAsync(scraperConfig.URL);

            string html = await page.ContentAsync();

            HtmlDocument document = new();
            document.LoadHtml(html);
            return DocParseItems(scraperConfig, document);
        }

        private async Task<IBrowser> GetPlaywright() {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {
                Headless = true
            });
            return browser;
        }
        private IWebDriver GetDriver(string url) {
            var options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-features=VizDisplayCompositor");
            options.AddArgument("--disable-quic");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument($"--user-data-dir={Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())}");

            // CHROMEDRIVER_DIR 環境変数でシステムのchromedriverを直接指定可能（Docker/Linux用）
            string? chromedriverDir = Environment.GetEnvironmentVariable("CHROMEDRIVER_DIR");
            var service = chromedriverDir != null
                ? ChromeDriverService.CreateDefaultService(chromedriverDir)
                : ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            IWebDriver driver = new ChromeDriver(service, options);
            driver.Navigate().GoToUrl(url);
            return driver;
        }

        private IWebDriver NavigateToPage(IWebDriver driver, ScraperConfig scraperConfig) {
            if (scraperConfig.NAVIGATE_PAGES == null || scraperConfig.NAVIGATE_PAGES.Count == 0) {
                return driver;
            }
            foreach (NavigatePage navi in scraperConfig.NAVIGATE_PAGES) {
                if (navi.TYPE == NavigatePageTypes.pagination_search) {
                    string text = navi.TARGET_LINK.NODE + "//*[contains(text(),'" + navi.TARGET_LINK.TEXT + "')]";
                    var n = driver.FindElements(By.XPath(text));
                    if (n.Count > 0) {
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

        public List<Dictionary<string, string>> DocParseItems(ScraperConfig scraperConfig, HtmlDocument doc) {
            Dictionary<string, ExtractDef> extractDict = scraperConfig.EXTRACT;
            var items = new List<Dictionary<string, string>>();
            foreach (var (key, extractDef) in extractDict) {
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

        private static string GetInnerText(HtmlNode postNode, string node) {
            var bodyNode = postNode.SelectSingleNode(node);
            return bodyNode?.InnerText.Trim() ?? "";
        }

        private static string GetAttributeValue(HtmlNode postNode, string node, string attribute) {
            var bodyNode = postNode.SelectSingleNode(node);
            return bodyNode?.GetAttributeValue(attribute, "").Trim() ?? "";
        }

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
