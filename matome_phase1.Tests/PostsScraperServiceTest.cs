using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs.Base;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.services;
using OpenQA.Selenium.DevTools.V136.Overlay;
using System.Printing;
using System.Security.Policy;
using System.Text.Json;

namespace matome_phase1.Tests.PostsScraperServiceTest {
    internal class DocsPaths {
        internal string targetHtml { get ; init; }
        internal string ConfigPath { get; init; }
        internal string DocParseItems_expectPath { get; init; }
        internal string GetItems_expectPath { get ; init; }
        internal string outputFilePath { get; init; }
        internal DocsPaths(string type, string site, string target) {
            targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
            ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
            DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
            GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            outputFilePath = @$"..\..\..\log\{site}_GetItems_actual.json";
        }
    }
    public class PostsScraperServcieTest {
        private AbstractScraperConfig CreateAConfigAndService(string configPath, out PostsService service) {
            AbstractScraperConfig AConfig;
            string config = File.ReadAllText(configPath);
            AConfig = ScraperFactory.Create(config);
            service = (PostsService)ScraperFactory.Create(AConfig);
            return AConfig;
        }

        [Fact]
        public void ch5_GetItemsTest() {
            string type = "Post";
            string site = "5ch";
            string target = "splatoon";
            DocsPaths docs = new(type, site, target);
            AbstractScraperConfig AConfig;
            AConfig = CreateAConfigAndService(docs.ConfigPath, out PostsService service);

            //expectedの作成
            string expect = File.ReadAllText(docs.GetItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            //Act
            List<Object> Items = service.GetItems(AConfig);
            List<Post> actualItems = Items.Cast<Post>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(docs.outputFilePath, json); //書き出し

            //Assert
            Assert.Equal(expectList.Count, Items.Count);

        }
        [Fact]
        public void zawazawa_GetItemsTest() {
            string type = "Post";
            string site = "zawazawa";
            string target = "雑談";
            //Arrange
            DocsPaths docs = new(type, site, target);
            AbstractScraperConfig AConfig;
            PostsService service;
            string config = File.ReadAllText(docs.ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

            //expectedの作成
            string expect = File.ReadAllText(docs.GetItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            //Act
            List<Object> Items = service.GetItems(AConfig);
            List<Post> actualItems = Items.Cast<Post>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(docs.outputFilePath, json); //書き出し

            //Assert
            Assert.Equal(expectList.Count, Items.Count);

        }
        [Fact]
        public void ch5_DocParseItemsTest() {
            string type = "Post";
            string site = "5ch";
            string target = "splatoon";
            
            //Arrange
            DocsPaths docs = new(type, site, target);
            AbstractScraperConfig AConfig;
            PostsService service;
            string config = File.ReadAllText(docs.ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(docs.targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(docs.DocParseItems_expectPath);
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
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(docs.outputFilePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
        [Fact]
        public void zawazawa_DocParseItemsTest() {
            string type = "Post";
            string site = "zawazawa";
            string target = "雑談";
            //Arrange
            DocsPaths docs = new(type, site, target);
            AbstractScraperConfig AConfig;
            PostsService service;
            string config = File.ReadAllText(docs.ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (PostsService)ScraperFactory.Create(AConfig);

            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(docs.targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(docs.DocParseItems_expectPath);
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
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(docs.outputFilePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
    }
}
