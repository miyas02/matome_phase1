using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public abstract class AbstractScraperConfig {
        [JsonPropertyName("SITE_NAME")]
        public string? SITE_NAME { get; set; }

        [JsonPropertyName("URL")]
        public string? URL { get; set; }

        [JsonPropertyName("NAVIGATE_PAGES")]
        public List<NAVIGATEPAGE>? NAVIGATE_PAGES { get; set; }

        [JsonPropertyName("LOGIC")]
        public string? LOGIC { get; set; }
    }
}
