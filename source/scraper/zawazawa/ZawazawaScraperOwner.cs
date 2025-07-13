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
    /// <summary>
    /// スクレイピングする処理を抽象化し定義したクラス
    /// サイト毎の具体の処理はIScraperLogicを実装したクラスで行う
    /// </summary>
    /// <returns></returns>
    internal class ZawazawaScraperOwner : IScraperOwner
    {

        public List<AbstractPost> GetPosts(AbstractScraperConfig scraperConfig)
        {

            var html = GetHtml(scraperConfig.URL);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            List<AbstractPost>posts = DocParsePosts(doc, scraperConfig);

            return posts;
        }

        private string GetHtml(string url)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // コンソール非表示
            service.SuppressInitialDiagnosticInformation = true; // DevToolsなどのログ非表示

            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                driver.Navigate().GoToUrl(url);
                return driver.PageSource;
            }
        }

        /// <summary>
        /// HtmlDocumentを解析して、Postのリストを返すメソッド
        /// 処理は抽象化し複数のサイトで共通化するために定義
        /// サイト毎の具体的な処理はIScraperLogicを実装したクラスで行う
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private List<AbstractPost> DocParsePosts(HtmlDocument doc, AbstractScraperConfig scraperConfig)
        {
            HtmlNode contentNode = doc.DocumentNode.SelectSingleNode(scraperConfig.LIST_NODE);
            if (contentNode == null)
            {
                throw new Exception(Constants.ContentNodeNotFound);
            }
            var postNodes = new List<HtmlNode>();
            // class='list-view-item'の全要素を取得
            if (contentNode.SelectNodes(scraperConfig.POST_NODE) != null)
            {
                postNodes.AddRange(contentNode.SelectNodes(scraperConfig.POST_NODE));
            }

            var posts = new List<AbstractPost>();
            foreach (var node in postNodes)
            {
                zawazawaPost post = new();
                var bodyNode = node.SelectSingleNode(scraperConfig.TEXT_NODE);
                post.Text = bodyNode?.InnerText.Trim() ?? "";
                post.Id = node.GetAttributeValue(scraperConfig.DATE_NODE, null);

                // ユーザーID
                var userIdNode = node.SelectSingleNode(scraperConfig.USER_ID_NODE)
                    ?? node.SelectSingleNode(scraperConfig.USER_ID_NODE);
                if (userIdNode != null)
                {
                    post.UserId = Regex.Replace(userIdNode.InnerText, @"[\s\n]", "");
                }
                else
                {
                    post.UserId = "";
                }
                // 投稿日時
                var timestampNode = node.SelectSingleNode(scraperConfig.DATE_NODE);
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
                var replyNode = node.SelectSingleNode(scraperConfig.REPLY_NODE);
                if (replyNode != null)
                {
                    post.Reply = Regex.Replace(replyNode.InnerText, @"\D", "");
                }

                // 画像URL
                var imageNode = node.SelectSingleNode(scraperConfig.IMAGE_URL_NODE);
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
