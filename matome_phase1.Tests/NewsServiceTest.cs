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

namespace matome_phase1.Tests.NewsScraperServiceTest {
    internal class DocsPaths {
        internal string targetHtml { get; init; }
        internal string ConfigPath { get; init; }
        internal string DocParseItems_expectPath { get; init; }
        internal string GetItems_expectPath { get; init; }
        internal string outputFilePath { get; init; }
        internal DocsPaths(string type, string site, string target) {
            targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
            ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
            DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
            GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            outputFilePath = @$"..\..\..\log\{site}_GetItems_actual.json";
        }
    }
    public class NewsScraperServcieTest {
        private AbstractScraperConfig CreateAConfigAndService(string configPath, out NewsService service) {
            AbstractScraperConfig AConfig;
            string config = File.ReadAllText(configPath);
            AConfig = ScraperFactory.Create(config);
            service = (NewsService)ScraperFactory.Create(AConfig);
            return AConfig;
        }

        [Fact]
        public void yahoo_GetItemsTest() {
            string type = "news";
            string site = "yahoo";
            string target = "IT";
            DocsPaths docs = new(type, site, target);
            AbstractScraperConfig AConfig;
            AConfig = CreateAConfigAndService(docs.ConfigPath, out NewsService service);

            //expectedの作成
            string expect = File.ReadAllText(docs.GetItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<News>>(expect);

            //Act
            List<Object> Items = service.GetItems(AConfig);
            List<News> actualItems = Items.Cast<News>().ToList();
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
        public void yahoo_DocParseItemsTest() {
            string type = "News";
            string site = "yahoo";
            string target = "IT";

            //Arrange
            DocsPaths docs = new(type, site, target);
            AbstractScraperConfig AConfig;
            NewsService service;
            string config = File.ReadAllText(docs.ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (NewsService)ScraperFactory.Create(AConfig);

            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(docs.targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(docs.DocParseItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<News>>(expect);

            //Act
            // actualの作成
            List<Object> Items = service.DocParseItems(AConfig, doc);
            List<News> actualItems = Items.Cast<News>().ToList();
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
