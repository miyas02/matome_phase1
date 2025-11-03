using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.Interface;
using OpenQA.Selenium.DevTools.V136.Overlay;
using System.Printing;
using System.Text.Json;

namespace matome_phase1.Tests {
    public class PostsScraperServcieTest {
        private const string ConfigPathExistNavi = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\PostConfigTest_Pagination.json";
        private const string ConfigPathNotExistNavi = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\PostConfigTest_NotPagination.json";
        private const string targetHtml = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\targetHtml.html";
        private const string expectPath = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\PostExpectList.json";
        AbstractScraperConfig AConfigExistNavi;
        AbstractScraperConfig AConfigNotExistNavi;
        PostsScraperService serviceExistNavi;
        PostsScraperService serviceNotExistNavi;
        public PostsScraperServcieTest() {
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(ConfigPathExistNavi);
            AConfigExistNavi = ScraperFactory.Create(config);
            if (AConfigExistNavi.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfigExistNavi);
            }
            serviceExistNavi = (PostsScraperService)ScraperFactory.Create(AConfigExistNavi);

            string notNavi = File.ReadAllText(ConfigPathNotExistNavi);
            AConfigNotExistNavi = ScraperFactory.Create(notNavi);
            if (AConfigNotExistNavi.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfigNotExistNavi);
            }
            serviceNotExistNavi = (PostsScraperService)ScraperFactory.Create(AConfigNotExistNavi);

        }

        [Fact]
        public void GetItemsTest() {
            //Arrange
            //expectedの作成
            string expect = File.ReadAllText(expectPath);
            var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            //Act
            List<Object> Items = serviceNotExistNavi.GetItems(AConfigNotExistNavi);

            //Assert
            Assert.Equal(expectList, Items);

        }
        [Fact]
        public void DocParseItemsTest() {
            //Arrange
            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(expectPath);
            var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            //Act
            // actualの作成
            List<Object> Items = serviceExistNavi.DocParseItems(AConfigExistNavi, doc);
            List<Post> actualItems = Items.Cast<Post>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //Assert
            Assert.Equal(expectList, actualItems);
        }
    }
}