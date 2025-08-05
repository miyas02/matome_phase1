using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using matome_phase1.constants;
using System.Diagnostics;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.Configs;


namespace matome_phase1.scraper {
    internal class ScraperOwner : IScraperOwner  {
        private static readonly Dictionary<string, Func<string, object>> _handlers = new() {
            { ScraperLogics.Posts, json => {
                var config = JsonSerializer.Deserialize<PostsScraperConfig>(json);
                return config.GetItems();
                }
            }
            // 他ロジックも追加
        };

        public void LoadConfig(string configPath) {
            string json = File.ReadAllText(configPath);
            using JsonDocument doc = JsonDocument.Parse(json);
            string logicValue = doc.RootElement.GetProperty("LOGIC").GetString();

            //jsonのlogic属性のvalueで生成するインスタンスを切り替え
            //サイトカテゴリごとに条件分岐を追加する
            AbstractScraperConfig scraperConfig = ScraperConfigFactory.Create(json);
            IScraperService scraperService = ScraperServiceFactory.Create(scraperConfig);
            //Page遷移処理を行う
            List<Object> Items = scraperService.GetItems(scraperConfig);
            foreach (var item in Items) {
                if (item is Post) {
                    var p = (Post)item;
                    Debug.WriteLine($"ID: {p.Id}, UserID: {p.UserId}, Text: {p.Text}, Date: {p.Date}");
                }
            }
            Debug.WriteLine(Items);
        }
    }
}
