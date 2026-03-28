using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace matome_phase1.scraper {
    public class ScraperOwner {
        public IScraperService ScraperService { get; set; }
        public ScraperConfig scraperConfig { get; set; }
        public ItemsVM ItemsVM { get; set; }
        public ItemsVM LoadConfig(string configJson) {
            using JsonDocument doc = JsonDocument.Parse(configJson);
            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter() }
            };
            scraperConfig = JsonSerializer.Deserialize<ScraperConfig>(configJson, options) ?? throw new InvalidOperationException("デシリアライズ失敗");
            List<Dictionary<string, string>> Items = ScraperService.GetItems(scraperConfig);
            ItemsVM = new ItemsVM(scraperConfig, Items);
            return ItemsVM;
        }
    }
}
