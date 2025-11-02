using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    public class ScraperFactory {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Config"></param>
        /// <returns>IScraperService</returns>
        public static IScraperService Create(AbstractScraperConfig Config) {

            return Config switch {
                PostConfig  => new PostsScraperService(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns>AbstractScraperConfig</returns>
        public static AbstractScraperConfig Create(string json) {
            using JsonDocument doc = JsonDocument.Parse(json);
            string logic = doc.RootElement.GetProperty("LOGIC").GetString();

            var options = new JsonSerializerOptions {
                Converters = { new JsonStringEnumConverter()
                }
            };
            return logic switch {
                "Posts" => JsonSerializer.Deserialize<PostConfig>(json, options)
            };
        }
    }
}
