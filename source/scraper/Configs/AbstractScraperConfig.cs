using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public abstract class AbstractScraperConfig {
        /// <summary>
        /// urlを渡してDriverを取得するメソッド
        /// 具象クラスのGetItems()メソッドで使用される
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>Driver</returns>
        protected IWebDriver GetDriver(string url) {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // コンソール非表示
            service.SuppressInitialDiagnosticInformation = true; // DevToolsなどのログ非表示

            using (IWebDriver driver = new ChromeDriver(service, options)) {
                driver.Navigate().GoToUrl(url);
                return driver;
            }
        }

        protected void NavigateToPage(IWebDriver driver) {
            //NavigatePagesConfig nullチェック
            if (PAGES == null || PAGES.Count == 0) {
                return;
            }
            //NavigatePagesConfigのListの各要素を取り出す
            foreach (var pageConfig in PAGES) {
                string targetNode = pageConfig.TARGET_LINK.NODE;

                //PAGINATIONのページ数を取得
                int pageCount = pageConfig.PAGINATION.PageCount;
                //ページ数分ループ
                for (int i = 1; i <= pageCount; i++) {
                    //URLにページ数を追加してHTMLを取得
                    string urlWithPage = $"{targetNode}?page={i}";
                    string html = GetDriver(urlWithPage);
                    //HTMLを解析してアイテムを取得
                    //アイテムの取得はサブクラスで実装する
                }
            }
        }

        public abstract List<Object> GetItems();

        public abstract string URL {
            get; set;
        }
        public abstract string SITE_NAME {
            get; set;
        }
        public abstract string LOGIC {
            get; set;
        }
        public List<NavigatePagesConfig> PAGES {
       get; set;
        }
    }
}
