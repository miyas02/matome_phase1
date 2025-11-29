using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.Interface;
using OpenQA.Selenium.DevTools.V136.Overlay;
using System.Printing;
using System.Text.Json;
using matome_phase1.scraper.services;

namespace matome_phase1.Tests {
    public class PostsScraperServcieTest {
        //書き換える
        private static string type = "Post";
        private static string site = "5ch";
        private static string target = "splatoon";
       
        private string targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
        private string ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
        private string DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
        private string GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";

        AbstractScraperConfig AConfig;
        PostsService service;
        public PostsScraperServcieTest() {
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

        }

        [Fact]
        public void GetItemsTest() {
            //Arrange
            //expectedの作成
            string expect = File.ReadAllText(GetItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            //Act
            List<Object> Items = service.GetItems(AConfig);
            List<Post> actualItems = Items.Cast<Post>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string filePath = @"..\..\..\log\Post_GetItems_actual.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList.Count, Items.Count);

        }
        [Fact]
        public void DocParseItemsTest() {
            //Arrange
            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(DocParseItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            //Act
            // actualの作成
            List<Object> Items = service.DocParseItems(AConfig, doc);
            List<Post> actualItems = Items.Cast<Post>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string filePath = @"..\..\..\log\Post_DocParseItems_actual.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
    }
}