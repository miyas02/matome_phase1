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
using matome_phase1.scraper.Interface;


namespace matome_phase1.scraper {
    internal class ScraperOwner : IScraperOwner  {
        public IScraperService ScraperService { get; set; }
        public AbstractScraperConfig AConfig { get; set; }
        public void LoadConfig(string config) {
            using JsonDocument doc = JsonDocument.Parse(config);
            string logicValue = doc.RootElement.GetProperty("LOGIC").GetString();

            //jsonのlogic属性のvalueで生成するインスタンスを切り替え
            //サイトカテゴリごとに条件分岐を追加する
            AConfig = ScraperFactory.Create(config);
            ScraperService = ScraperFactory.Create(AConfig);

            List<Object> Items = ScraperService.GetItems(AConfig);
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
