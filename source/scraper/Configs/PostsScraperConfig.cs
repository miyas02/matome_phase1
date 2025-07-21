using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    internal class PostsScraperConfig : AbstractScraperConfig {
        public override string URL {
            get; set;
        }
        public override string SITE_NAME {
            get; set;
        }
        public override string LOGIC {
            get; set;
        }

        public string LIST_NODE {
            get; set;
        }
        public string POST_NODE {
            get; set;
        }
        public NodeSelector USER_ID {
            get; set;
        }
        public NodeSelector TEXT {
            get; set;
        }
        public NodeSelector DATE {
            get; set;
        }
        public NodeSelector REPLY {
            get; set;
        }
        public NodeSelector IMAGE {
            get; set;
        }
        public NodeSelector POST_ID {
            get; set;
        }

        public override List<Object> GetItems() {
            string html = GetHtml(URL);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return DocParsePosts(doc);
        }

        private string SelectorSwitch (HtmlNode postNode, NodeSelector selector) {
            if (selector.TYPE == "text") {
                var bodyNode = postNode.SelectSingleNode(selector.NODE);
                return bodyNode?.InnerText.Trim() ?? "";
            } else if (selector.TYPE == "attribute") {
                var bodyNode = postNode.SelectSingleNode(selector.NODE);
                return bodyNode.GetAttributeValue(selector.ATTRIBUTE, null).Trim();
            }
            throw new Exception($"Unsupported selector type: {selector.TYPE}");
        }

        private List<Object> DocParsePosts(HtmlDocument doc) {
            HtmlNode contentNode = doc.DocumentNode.SelectSingleNode(LIST_NODE);
            if (contentNode == null) {
                throw new Exception(Constants.ContentNodeNotFound);
            }
            var postNodes = new List<HtmlNode>();
            // LIST_NODEの全要素を取得
            if (contentNode.SelectNodes(POST_NODE) != null) {
                postNodes.AddRange(contentNode.SelectNodes(POST_NODE));
            }

            var posts = new List<Object>();
            foreach (var postNode in postNodes) {
                Post post = new();
                // 各NodeSelectorに基づいて値を取得
                post.Text = SelectorSwitch(postNode, TEXT);
                post.Id = SelectorSwitch(postNode, POST_ID);
                post.UserId = SelectorSwitch(postNode, USER_ID);
                post.Reply = SelectorSwitch(postNode, REPLY);
                post.ImageUrl = SelectorSwitch(postNode, IMAGE);
                var timestamp = SelectorSwitch(postNode, DATE);
                var cleanedDateString = Regex.Replace(timestamp, @"\s*\(.\)\s*", " ");
                if (DateTime.TryParseExact(cleanedDateString.Trim(), "yyyy/MM/dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dt)) {
                    post.Date = dt;
                }
                posts.Add(post);
            }
            return posts;
        }
    }
}
