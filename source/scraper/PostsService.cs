using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Models;
using OpenQA.Selenium.BiDi.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
[assembly: InternalsVisibleTo("matome_phase1.Tests")]

namespace matome_phase1.scraper {
    internal class PostsService : ScraperService {
        private PostConfig Config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal override List<System.Object> DocParseItems(AbstractScraperConfig AConfig, HtmlDocument doc) {
            try {
                Config = (PostConfig)AConfig;
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
                Post post = new();
                post.Text = GetValue(postNode, Config.TEXT);//GetInnerText(postNode, Config.TEXT.NODE);
                post.Id = GetValue(postNode, Config.POST_ID);//GetInnerText(postNode, Config.POST_ID.NODE);
                post.UserId = GetValue(postNode, Config.USER_ID);//GetInnerText(postNode, Config.USER_ID.NODE);
                post.Reply = GetValue(postNode, Config.REPLY);//GetInnerText(postNode, Config.REPLY.NODE);
                post.ImageUrl = GetValue(postNode, Config.IMAGE);//GetInnerText(postNode, Config.IMAGE.NODE);
                var timestamp = GetValue(postNode, Config.DATE);//GetInnerText(postNode, Config.DATE.NODE);
                var cleanedDateString = Regex.Replace(timestamp, @"\s*\(.\)\s*", " ");
                if (DateTime.TryParseExact(cleanedDateString.Trim(), "yyyy/MM/dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dt)) {
                    post.Date = dt;
                }
                posts.Add(post);
            }
            return posts;
        }

        private string GetValue(HtmlNode postNode, ConfigNodeBase configNode) {
            if (configNode.TYPE == "attribute") {
                return GetAttributeValue(postNode, configNode.NODE, configNode.ATTRIBUTE);
            } 
            if (configNode.TYPE == "text"){
                return GetInnerText(postNode, configNode.NODE);
            }
            throw new ConfigException(ScraperExceptionType.ContentNodeIsNull);
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
    }
}