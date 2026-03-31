using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.services;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace matome_phase1.Tests.ScraperServiceUnitTest {
    public class SeleniumServcieUnitTest {
        public static IEnumerable<object[]> TestCases => new[] {
            new object[] { "5ch", @"TestFiles/5ch_ScraperConfig.json", @"TestFiles/5ch_DocParseItems_Expect.json", @"log/5ch_DocParseItems_Actual.json", @"TestFiles/targetHtml.html" }
        };
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        [Theory]
        [MemberData(nameof(TestCases))]
        public void DocParseItemsTest(string target, string configPath, string expectPath, string actualPath, string targetHtml) {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectRoot)
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information("DocParseItemsTest start: target={Target}", target);
            Log.Information("configPath={ConfigPath}", Path.Combine(projectRoot, configPath));
            Log.Information("expectPath={ExpectPath}", Path.Combine(projectRoot, expectPath));
            Log.Information("targetHtml={TargetHtml}", Path.Combine(projectRoot, targetHtml));
            Log.Information("actualPath={ActualPath}", Path.Combine(projectRoot, actualPath));

            string configJson = File.ReadAllText(Path.Combine(projectRoot, configPath));
            using JsonDocument doc = JsonDocument.Parse(configJson);
            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter()
                }
            };
            ScraperConfig scraperConfig = JsonSerializer.Deserialize<ScraperConfig>(configJson, options) ?? throw new InvalidOperationException("デシリアライズ失敗");
            PlaywrightService scraperService = new PlaywrightService();

            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(Path.Combine(projectRoot, targetHtml));
            var html = new HtmlDocument();
            html.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(Path.Combine(projectRoot, expectPath));
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
            string outPath = Path.Combine(projectRoot, actualPath);
            Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
            File.WriteAllText(outPath, json); //書き出し

            //Assert
            Assert.Equal(expectList, Items);
            Log.Information("DocParseItemsTest end: target={Target}, items={Count}", target, Items?.Count);
            Log.CloseAndFlush();
        }
    }
}
