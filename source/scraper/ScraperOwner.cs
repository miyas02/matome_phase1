using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using matome_phase1.constants;


namespace matome_phase1.scraper {
    internal class ScraperOwner : IScraperOwner  {
        public AbstractScraperConfig ScraperConfigFactory(string json) {
            using JsonDocument doc = JsonDocument.Parse(json);
            string logicValue = doc.RootElement.GetProperty("LOGIC").GetString();

            //jsonのlogic属性のvalueで生成するインスタンスを切り替え
            if (logicValue == ScraperLogics.Posts) {
                AbstractScraperConfig ScraperConfig = JsonSerializer.Deserialize<PostsScraperConfig>(json);
                return ScraperConfig;
            }
            Console.WriteLine(Constants.InvalidLogicValue);
            Environment.Exit(1);
            return null;
        }
        public void LoadConfig(string configPath) {
            string json = File.ReadAllText(configPath);
           
            AbstractScraperConfig ScraperConfig = ScraperConfigFactory(json);
            Console.WriteLine(ScraperConfig);
        }
    }
}
