using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Models;
using OpenQA.Selenium.BiDi.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace matome_phase1.scraper {
    internal class PostsScraperService : ScraperService {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AConfig"></param>
        /// <param name="doc"></param>
        /// <returns List></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override List<System.Object> DocParseItems(AbstractScraperConfig AConfig, HtmlDocument doc) {
            switch (AConfig.LOGIC) {
                case ScraperLogics.Posts:
                    return DocParsePosts((PostConfig)AConfig, doc);
                // 他のロジックも追加
                default:
                    throw new NotImplementedException($"Logic '{AConfig.LOGIC}' is not implemented.");
            }

            // This method should be implemented in derived classes to parse items from the document
            throw new NotImplementedException("This method should be overridden in derived classes.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private List<System.Object> DocParsePosts(PostConfig Config, HtmlDocument doc) {
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
                // 各NodeSelectorに基づいて値を取得
                post.Text = GetInnerText(postNode, Config.TEXT.NODE);
                post.Id = GetInnerText(postNode, Config.POST_ID.NODE);
                post.UserId = GetInnerText(postNode, Config.USER_ID.NODE);
                post.Reply = GetInnerText(postNode, Config.REPLY.NODE);
                post.ImageUrl = GetInnerText(postNode, Config.IMAGE.NODE);
                var timestamp = GetInnerText(postNode, Config.DATE.NODE);
                var cleanedDateString = Regex.Replace(timestamp, @"\s*\(.\)\s*", " ");
                if (DateTime.TryParseExact(cleanedDateString.Trim(), "yyyy/MM/dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dt)) {
                    post.Date = dt;
                }
                posts.Add(post);
            }
            return posts;
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
    }
}