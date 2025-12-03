using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace matome_phase1.scraper.services {
    internal class NewsService : ScraperService {
        private NewsConfig Config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal override List<System.Object> DocParseItems(AbstractScraperConfig AConfig, HtmlDocument doc) {
            try {
                Config = (NewsConfig)AConfig;
            } catch (InvalidCastException) {
                throw new ConfigException(ScraperExceptionType.InvalidLogicValue, AConfig);
            }

            HtmlNode? contentNode = doc.DocumentNode.SelectSingleNode(Config.LIST_NODE);

            if (contentNode == null) {
                throw new ConfigException(ScraperExceptionType.ContentNodeIsNull, Config);
            }

            var postNodes = new List<HtmlNode>();
            // LIST_NODEの全要素を取得
            if (contentNode.SelectNodes(Config.POST_NODE) != null) {
                postNodes.AddRange(contentNode.SelectNodes(Config.POST_NODE) ?? Enumerable.Empty<HtmlNode>());
            }

            var posts = new List<System.Object>();
            foreach (var postNode in postNodes) {
                News news = new(
                    GetValue(postNode, Config.NAME),
                    GetValue(postNode, Config.LINK),
                    GetValue(postNode, Config.IMAGE)
                    );
                posts.Add(news);
            }
            return posts;
        }
        /// <summary>
        /// ノードから値を取得する
        /// </summary>
        /// <param name="postNode"></param>
        /// <param name="configNode"></param>
        /// <returns></returns>
        private string GetValue(HtmlNode postNode, ConfigNodeBase configNode) {
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
        internal override IReadOnlyCollection<IWebElement> EnsureLoadedItems(IWebDriver driver, AbstractScraperConfig AConfig) {
            var timeout = TimeSpan.FromSeconds(50);
            NewsConfig config = (NewsConfig)AConfig;
            var end = DateTime.UtcNow + timeout;
            var container = driver.FindElement(By.XPath(config.LIST_NODE));
            int previousCount = -1;

            while (DateTime.UtcNow < end) {
                var items = container.FindElements(By.XPath(config.POST_NODE));
                if (items.Count > previousCount) {
                    previousCount = items.Count;
                    var ret = ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'auto', block:'end'});", items.Last());
                    Thread.Sleep(1000);
                    continue;
                }
                return items;
            }
            return container.FindElements(By.XPath(config.POST_NODE));
        }
    }
}
