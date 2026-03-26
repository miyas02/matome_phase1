using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.services;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V136.Overlay;
using System.Printing;
using System.Security.Policy;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace matome_phase1.Tests.ScraperServiceTest {
    internal class DocsPaths {
        internal string targetHtml { get ; init; }
        internal string ConfigPath { get; init; }
        internal string DocParseItems_expectPath { get; init; }
        internal string DocParseItems_actualPath { get; init; }
        internal string GetItems_expectPath { get ; init; }
        internal string GetItems_actualPath { get; init; }
        internal string log { get; init; }
        internal string basePath = Path.Combine(AppContext.BaseDirectory, "TestFiles");
        internal DocsPaths(string site) {
            targetHtml = @$"TestFiles\targetHtml.html";
            ConfigPath = @$"TestFiles\Config.json";
            DocParseItems_expectPath = @$"TestFiles\DocParseItems_Expect.json";
            DocParseItems_actualPath = @$"TestFiles\log\{site}_DocParseItems_actual.json";
            GetItems_expectPath = @$"TestFiles\GetItems_Expect.json";
            GetItems_actualPath = @$"TestFiles\log\{site}_GetItems_actual.json";
            log = @$"TestFiles\logs\{site}_ScraperServiceTest_log.txt";
        }
    }
    public class ScraperServcieTest {
        //private ScraperConfig CreateAConfigAndService(string configPath, out ScraperService service) {
        //    ScraperConfig scraperConfig;
        //    string config = File.ReadAllText(configPath);
        //    return scraperConfig;
        //}

        [Fact]
        public void ch5_GetItemsTest() {
            string target = "5ch";
            DocsPaths docs = new(target);
            string configJson = File.ReadAllText(Path.Combine(docs.basePath, $"{target}_ScraperConfig.json"));
            using JsonDocument doc = JsonDocument.Parse(configJson);
            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter()
                }
            };
            ScraperConfig scraperConfig = JsonSerializer.Deserialize<ScraperConfig>(configJson, options) ?? throw new InvalidOperationException("デシリアライズ失敗");
            ScraperService scraperService = new ScraperService();

            //expectedの作成
            //string expect = File.ReadAllText(docs.GetItems_expectPath);
            //var expectList = JsonSerializer.Deserialize<List<Post>>(expect);

            //Act
            List<Dictionary<string,string>> Items = scraperService.GetItems(scraperConfig);
            var op = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string json = JsonSerializer.Serialize(Items, op); //Listをjsonにシリアライズ
            string outPath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "log", $"{target}_GetItems_actual.json");
            Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
            File.WriteAllText(outPath, json); //書き出し

            //Assert
            Assert.NotEqual(0, Items.Count);

        }
        
        [Fact]
        public void ch5_DocParseItemsTest() {
            string target = "5ch";
            DocsPaths docs = new(target);
            string configJson = File.ReadAllText(Path.Combine(docs.basePath, $"{target}_ScraperConfig.json"));
            using JsonDocument doc = JsonDocument.Parse(configJson);
            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter()
                }
            };
            ScraperConfig scraperConfig = JsonSerializer.Deserialize<ScraperConfig>(configJson, options) ?? throw new InvalidOperationException("デシリアライズ失敗");
            ScraperService scraperService = new ScraperService();

            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(docs.targetHtml);
            var html = new HtmlDocument();
            html.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(docs.DocParseItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(expect);

            //Act
            // actualの作成
            var Items = scraperService.DocParseItems(scraperConfig, html);
            var op = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string json = JsonSerializer.Serialize(Items, op); //Listをjsonにシリアライズ
            string outPath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "log", $"{target}_DocParseItems_actual.json");
            Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
            File.WriteAllText(outPath, json); //書き出し

            //Assert
            Assert.Equal(expectList, Items);
        }
    }
}
