using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Configs.EC;
using matome_phase1.scraper.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("matome_phase1.Tests")]

namespace matome_phase1.scraper.services {
    internal class ECService : ScraperService {
        private ECConfig Config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal override List<System.Object> DocParseItems(AbstractScraperConfig AConfig, HtmlDocument doc) {
            try {
                Config = (ECConfig)AConfig;
            }
            catch (InvalidCastException) {
                throw new ConfigException(ScraperExceptionType.InvalidLogicValue, AConfig);
            }
            HtmlNode? contentNode = doc.DocumentNode.SelectSingleNode(Config.LIST_NODE);

            if (contentNode == null) {
                throw new ConfigException(ScraperExceptionType.ContentNodeIsNull, Config);
            }

            var itemNodes = new List<HtmlNode>();
            // LIST_NODEの全要素を取得
            if (contentNode.SelectNodes(Config.ITEM_NODE) != null) {
                itemNodes.AddRange(contentNode.SelectNodes(Config.ITEM_NODE) ?? Enumerable.Empty<HtmlNode>());
            }

            var items = new List<System.Object>();
            foreach (var itemNode in itemNodes) {
                EC item = new();
                item.Name = GetValue(itemNode, Config.ITEM_NAME);
                item.Price = GetValue(itemNode, Config.PRICE);
                item.img = GetValue(itemNode, Config.IMAGE);
                items.Add(item);
            }
            return items;
        }

        private string GetValue(HtmlNode postNode, ConfigNodeBase configNode) {
            Debug.WriteLine(postNode.OuterHtml);
            Debug.WriteLine(configNode);
            if (configNode.TYPE == "attribute") {
                return GetAttributeValue(postNode, configNode.NODE, configNode.ATTRIBUTE);
            }
            if (configNode.TYPE == "text") {
                return GetInnerText(postNode, configNode.NODE);
            }
            throw new ConfigException(ScraperExceptionType.ContentNodeIsNull);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemNode"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static string GetInnerText(HtmlNode itemNode, string node) {
            Debug.WriteLine(itemNode.OuterHtml);
            Debug.WriteLine(node);
            var bodyNode = itemNode.SelectSingleNode(node);
            return bodyNode?.InnerText.Trim() ?? "";
        }
        private static string GetAttributeValue(HtmlNode itemNode, string node, string attribute) {
            Debug.WriteLine(itemNode.OuterHtml);
            Debug.WriteLine(node);
            var bodyNode = itemNode.SelectSingleNode(node);
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
            ECConfig config = (ECConfig)AConfig;
            var end = DateTime.UtcNow + timeout;
            var container = driver.FindElement(By.XPath("//*[contains(@id,'item-grid')]/ul"));
            int previousCount = -1;

            while (DateTime.UtcNow < end) {
                var items = container.FindElements(By.XPath(config.ITEM_NODE));
                if (items.Count > previousCount) {
                    previousCount = items.Count;
                    var ret = ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'auto', block:'end'});", items.Last());
                    Thread.Sleep(1000);
                    continue;
                }
                return items;
            }
            return container.FindElements(By.XPath(config.ITEM_NODE));
        }
    }
}
