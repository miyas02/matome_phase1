using HtmlAgilityPack;
using matome_phase1.constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
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
        public string USER_ID_NODE {
            get; set;
        }
        public string TEXT_NODE {
            get; set;
        }
        public string DATE_NODE {
            get; set;
        }
        public string REPLY_NODE {
            get; set;
        }
        public string IMAGE_URL_NODE {
            get; set;
        }
        public string ID_KEY {
            get; set;
        }

        public List<Post> GetPosts() {
            string html = GetHtml(URL);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return DocParsePosts(doc);
        }

        private List<Post> DocParsePosts(HtmlDocument doc) {
            HtmlNode contentNode = doc.DocumentNode.SelectSingleNode(LIST_NODE);
            if (contentNode == null) {
                throw new Exception(Constants.ContentNodeNotFound);
            }
            var postNodes = new List<HtmlNode>();
            // class='list-view-item'の全要素を取得
            if (contentNode.SelectNodes(POST_NODE) != null) {
                postNodes.AddRange(contentNode.SelectNodes(POST_NODE));
            }

            var posts = new List<Post>();
            foreach (var node in postNodes) {
                Post post = new();
                var bodyNode = node.SelectSingleNode(TEXT_NODE);
                post.Text = bodyNode?.InnerText.Trim() ?? "";
                post.Id = node.GetAttributeValue(DATE_NODE, null);

                // ユーザーID
                var userIdNode = node.SelectSingleNode(USER_ID_NODE)
                    ?? node.SelectSingleNode(USER_ID_NODE);
                if (userIdNode != null) {
                    post.UserId = Regex.Replace(userIdNode.InnerText, @"[\s\n]", "");
                } else {
                    post.UserId = "";
                }
                // 投稿日時
                var timestampNode = node.SelectSingleNode(DATE_NODE);
                if (timestampNode != null) {
                    var timestamp = timestampNode.InnerText.Trim();
                    if (timestamp.Contains("修正")) {
                        timestamp = timestamp.Replace("修正", "");
                    }
                    var cleanedDateString = Regex.Replace(timestamp, @"\s*\(.\)\s*", " ");
                    // 日付フォーマットは必要に応じて調整
                    if (DateTime.TryParseExact(cleanedDateString.Trim(), "yyyy/MM/dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dt)) {
                        post.Date = dt;
                    }
                }

                // 返信ID
                var replyNode = node.SelectSingleNode(REPLY_NODE);
                if (replyNode != null) {
                    post.Reply = Regex.Replace(replyNode.InnerText, @"\D", "");
                }

                // 画像URL
                var imageNode = node.SelectSingleNode(IMAGE_URL_NODE);
                if (imageNode != null) {
                    post.ImageUrl = imageNode.GetAttributeValue("href", "");
                }
                posts.Add(post);
            }
            return posts;
        }
    }
}
