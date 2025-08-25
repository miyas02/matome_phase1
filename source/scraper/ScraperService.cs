using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Script;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace matome_phase1.scraper {
    public abstract class ScraperService : IScraperService {
        /// <summary>
        /// urlを渡してDriverを取得するメソッド
        /// 具象クラスのGetItems()メソッドで使用される
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>Driver</returns>
        public IWebDriver GetDriver(string url) {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // コンソール非表示
            service.SuppressInitialDiagnosticInformation = true; // DevToolsなどのログ非表示

            IWebDriver driver = new ChromeDriver(service, options);
            driver.Navigate().GoToUrl(url);
            return driver;
        }
        public void NavigateToPage(IWebDriver driver, AbstractScraperConfig AConfig) {
            //NavigatePagesConfig nullチェック
            if (AConfig.PAGES == null || AConfig.PAGES.Count == 0) {
                return;
            }
            //NavigatePagesConfigのListの各要素を取り出す
            foreach (var pageConfig in AConfig.PAGES) {

                ////PAGINATIONのページ数を取得
                //int pageCount = pageConfig.PAGINATION.PageCount;
                ////ページ数分ループ
                //for (int i = 1; i <= pageCount; i++) {
                //    //URLにページ数を追加してHTMLを取得
                //    string urlWithPage = $"{targetNode}?page={i}";
                //    string html = GetDriver(urlWithPage);
                //    //HTMLを解析してアイテムを取得
                //    //アイテムの取得はサブクラスで実装する
                //}
            }
        }

        public List<System.Object> GetItems(AbstractScraperConfig AConfig) {

            IWebDriver driver = GetDriver(AConfig.URL);
            //TODO navigateToPage()を呼び出す

            string html = driver.PageSource;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            driver.Quit(); // ドライバーを閉じる
            return DocParseItems(AConfig, doc);
        }

        protected abstract List<System.Object> DocParseItems(AbstractScraperConfig AConfig, HtmlDocument doc);
    }
}
