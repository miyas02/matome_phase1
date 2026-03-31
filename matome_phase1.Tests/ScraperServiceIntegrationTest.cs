using matome_phase1.scraper.Configs;
using matome_phase1.scraper.services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace matome_phase1.Tests.ScraperServiceIntegrationTest {
    public class ScraperServiceIntegrationTest {
        public static IEnumerable<object[]> TestCases => new[] {
            new object[] { "5ch", "5ch_ScraperConfig.json" }
        };
        string basePath = Path.Combine(AppContext.BaseDirectory, "TestFiles");
        [Theory]
        [MemberData(nameof(TestCases))]
        public void GetItemsTest(string target, string configPath) {
            string configJson = File.ReadAllText(Path.Combine(basePath, configPath));
            using JsonDocument doc = JsonDocument.Parse(configJson);
            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter() }
            };
            ScraperConfig scraperConfig = JsonSerializer.Deserialize<ScraperConfig>(configJson, options) ?? throw new InvalidOperationException("デシリアライズ失敗");
            PlaywrightService scraperService = new PlaywrightService();

            //Act
            List<Dictionary<string, string>> Items = scraperService.GetItems(scraperConfig).Result;
            var op = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string json = JsonSerializer.Serialize(Items, op);
            string outPath = Path.Combine(AppContext.BaseDirectory, "log", $"{target}_GetItems_Actual.json");
            Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
            File.WriteAllText(outPath, json);

            //Assert
            Assert.NotEqual(0, Items.Count);
        }
    }
}
