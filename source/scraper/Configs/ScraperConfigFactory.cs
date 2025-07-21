using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    internal class ScraperConfigFactory {
        public static AbstractScraperConfig Create(string json) {
            using JsonDocument doc = JsonDocument.Parse(json);
            string logic = doc.RootElement.GetProperty("LOGIC").GetString();

            return logic switch {
                "Posts" => JsonSerializer.Deserialize<PostsScraperConfig>(json)
            };
        }
    }
}
