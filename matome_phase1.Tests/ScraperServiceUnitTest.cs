using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.services;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace matome_phase1.Tests.ScraperServiceUnitTest {
    public class ScraperServcieUnitTest {
        public static IEnumerable<object[]> TestCases => new[] {
            new object[] { "5ch", "5ch_ScraperConfig.json", "5ch_DocParseItems_Expect.json", @"log/5ch_DocParseItems_Actual.json", "targetHtml.html" }
        };
        string basePath = Path.Combine(AppContext.BaseDirectory, "TestFiles");
        [Theory]
        [MemberData(nameof(TestCases))]
        public void DocParseItemsTest(string target, string configPath, string expectPath, string actualPath, string targetHtml) {
            //DocsPaths docs = new(target);
            string configJson = File.ReadAllText(Path.Combine(basePath, configPath));
            using JsonDocument doc = JsonDocument.Parse(configJson);
            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter()
                }
            };
            ScraperConfig scraperConfig = JsonSerializer.Deserialize<ScraperConfig>(configJson, options) ?? throw new InvalidOperationException("デシリアライズ失敗");
            ScraperService scraperService = new ScraperService();

            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(Path.Combine(basePath, targetHtml));
            var html = new HtmlDocument();
            html.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(Path.Combine(basePath, expectPath));
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
            string outPath = Path.Combine(AppContext.BaseDirectory, actualPath);
            Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
            File.WriteAllText(outPath, json); //書き出し

            //Assert
            Assert.Equal(expectList, Items);
        }
    }
}
