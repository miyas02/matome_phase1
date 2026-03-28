using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.services;
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
        internal DocsPaths(string target) {
            targetHtml = Path.Combine(basePath, "targetHtml.html");
            ConfigPath = Path.Combine(basePath, $"{target}_ScraperConfig.json");
            DocParseItems_expectPath = Path.Combine(basePath, $"{target}_DocParseItems_Expect.json");
            DocParseItems_actualPath = Path.Combine(basePath, $"{target}_DocParseItems_actual.json");
            GetItems_expectPath = Path.Combine(basePath, $"{target}_GetItems_Expect.json");
            GetItems_actualPath = Path.Combine(basePath, $"{target}_GetItems_actual.json");
            log = Path.Combine(basePath, "logs", $"{target}_ScraperServiceTest_log.txt");
        }
    }
    public class ScraperServcieTest {
        //private ScraperConfig CreateAConfigAndService(string configPath, out ScraperService service) {
        //    ScraperConfig scraperConfig;
        //    string config = File.ReadAllText(configPath);
        //    return scraperConfig;
        //}

        [Theory]
        [InlineData("5ch")]
        public void GetItemsTest(string target) {
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
        
        [Theory]
        [InlineData("5ch")]
        public void DocParseItemsTest(string target) {
            DocsPaths docs = new(target);
            string configJson = File.ReadAllText(Path.Combine(docs.basePath, docs.ConfigPath));
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
            string outPath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "log", docs.DocParseItems_actualPath);
            Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
            File.WriteAllText(outPath, json); //書き出し

            //Assert
            Assert.Equal(expectList, Items);
        }
    }
}
