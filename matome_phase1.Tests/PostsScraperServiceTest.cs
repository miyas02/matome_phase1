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

        [Fact]
        public void ch5_GetItemsTest() {
            string type = "Post";
            string site = "5ch";
            string target = "splatoon";

            string targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
            string ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
            string DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
            string GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            //Arrange
            AbstractScraperConfig AConfig;
            PostsService service;
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

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
            string filePath = @"..\..\..\log\5ch_GetItems_actual.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList.Count, Items.Count);

        }
        [Fact]
        public void zawazawa_GetItemsTest() {
            string type = "Post";
            string site = "zawazawa";
            string target = "雑談";

            string targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
            string ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
            string DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
            string GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            //Arrange
            AbstractScraperConfig AConfig;
            PostsService service;
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

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
            string filePath = @"..\..\..\log\zawazawa_GetItems_actual.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList.Count, Items.Count);

        }
        [Fact]
        public void ch5_DocParseItemsTest() {
        string type = "Post";
        string site = "5ch";
        string target = "splatoon";

        string targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
        string ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
        string DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
        string GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            //Arrange
            AbstractScraperConfig AConfig;
            PostsService service;
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

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
            string filePath = @"..\..\..\log\5ch_DocParseItems_actual.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
        [Fact]
        public void zawazawa_DocParseItemsTest() {
            string type = "Post";
            string site = "zawazawa";
            string target = "雑談";

            string targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
            string ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
            string DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
            string GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            //Arrange
            AbstractScraperConfig AConfig;
            PostsService service;
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

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
            string filePath = @"..\..\..\log\zawazawa_DocParseItems_actual.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
    }
}