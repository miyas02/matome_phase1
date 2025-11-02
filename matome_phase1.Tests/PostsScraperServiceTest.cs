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
        private const string configPath = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\testConfig.json";
        private const string targetHtml = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\targetHtml.html";
        private const string expectPath = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\expectList.json";
        [Fact]
        public void FactoryCreateTest() {
            
        }
        [Fact]
        public void DocParseItemsTest() {
            //Arrange
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(configPath);
            AbstractScraperConfig AConfig = ScraperFactory.Create(config);

            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            PostsScraperService service = (PostsScraperService)ScraperFactory.Create(AConfig);
            
            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            // actualの作成
            List<Object> Items = service.DocParseItems(AConfig, doc);
            List<Post> actualItems = Items.Cast<Post>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            //string json = JsonSerializer.Serialize(Items, options);
            //string filePath = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\output.json";
            //File.WriteAllText(filePath, json);

            //expectedの作成
            string expect = File.ReadAllText(expectPath);
            var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            Assert.Equal(expectList, actualItems);
        }
    }
}