using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using matome_phase1.constants;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace matome_phase1.scraper.zawazawa
{
    internal class ZawazawaScraperController : IScraperController
    {
        public List<AbstractPost> GetPosts()
        {

            var html = GetHtml();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return posts;
        }

        private string GetHtml()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // コンソール非表示
            service.SuppressInitialDiagnosticInformation = true; // DevToolsなどのログ非表示

            using (IWebDriver driver = new ChromeDriver(service, options))
            {

                driver.Navigate().GoToUrl(Constants.chatLog3);
                return driver.PageSource;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc" class="HtmlDocument"></param>
        /// <returns value="List<AbstractPost>"> </returns>
        /// <exception cref="Exception"></exception>
        private List<AbstractPost> DocPasePosts(HtmlDocument doc)
        {
            var contentNode = doc.DocumentNode.SelectSingleNode(Constants.nodes);
            if (contentNode == null)
            {
                throw new Exception(Constants.contentNodeNotFound);
            }
            var postNodes = new List<HtmlNode>();
            // class='list-view-item'の全要素を取得
            if (contentNode.SelectNodes(Constants.postNode) != null)
            {
                postNodes.AddRange(contentNode.SelectNodes(Constants.postNode));
            }

            var posts = new List<AbstractPost>();
            foreach (var node in postNodes)
            {
                zawazawaPost post = new();
                var bodyNode = node.SelectSingleNode(Constants.textNode);
                post.Text = bodyNode?.InnerText.Trim() ?? "";
                post.Id = node.GetAttributeValue(Constants.date, null);

                // ユーザーID
                var userIdNode = node.SelectSingleNode(Constants.userIdNode)
                    ?? node.SelectSingleNode(Constants.userIdNode_2);
                if (userIdNode != null)
                {
                    post.UserId = Regex.Replace(userIdNode.InnerText, @"[\s\n]", "");
                }
                else
                {
                    post.UserId = "";
                }
                // 投稿日時
                var timestampNode = node.SelectSingleNode(".//*[contains(@class, 'comment-timestamp')]//a");
                if (timestampNode != null)
                {
                    var timestamp = timestampNode.InnerText.Trim();
                    if (timestamp.Contains("修正"))
                    {
                        timestamp = timestamp.Replace("修正", "");
                    }
                    var cleanedDateString = Regex.Replace(timestamp, @"\s*\(.\)\s*", " ");
                    // 日付フォーマットは必要に応じて調整
                    if (DateTime.TryParseExact(cleanedDateString.Trim(), "yyyy/MM/dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dt))
                    {
                        post.Date = dt;
                    }
                }

                // 返信ID
                var replyNode = node.SelectSingleNode(".//*[contains(@class, 'comment-parent-link') and contains(@class, 'comment-parent-link-muted')]");
                if (replyNode != null)
                {
                    post.Reply = Regex.Replace(replyNode.InnerText, @"\D", "");
                }

                // 画像URL
                var imageNode = node.SelectSingleNode(".//a[contains(@class, 'media-modal')]");
                if (imageNode != null)
                {
                    post.ImageUrl = imageNode.GetAttributeValue("href", "");
                }
                posts.Add(post);
            }
            return posts;
        }
    }
}
