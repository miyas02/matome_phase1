using matome_phase1.constants;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Configs.Post;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace matome_phase1.scraper {
    internal class ScraperOwner  {
        public IScraperService ScraperService { get; set; }
        public ScraperConfig scraperConfig { get; set; }
        public ItemsVM ItemsVM { get; set; }
        public ItemsVM LoadConfig(string configJson) {
            using JsonDocument doc = JsonDocument.Parse(configJson);
            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter()
                }
            };
            scraperConfig = JsonSerializer.Deserialize<ScraperConfig>(configJson, options) ?? throw new InvalidOperationException("デシリアライズ失敗");

            //TODO AConfigの入力(読み込み)チェック
            //List<Object> Items = ScraperService.GetItems(scraperConfig);
            ItemsVM = new ItemsVM(AConfig,Items);
            return ItemsVM;
        }
    }
}
