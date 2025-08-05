using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    internal class ScraperServiceFactory {
        public static IScraperService Create(AbstractScraperConfig Config) {

            return Config switch {
                PostsScraperConfig  => new PostsScraperService(),
            };
        }
    }
}
