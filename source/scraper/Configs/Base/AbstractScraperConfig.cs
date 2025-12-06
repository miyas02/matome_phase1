using matome_phase1.scraper.Configs;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs.Base {
    public abstract class AbstractScraperConfig {
        [JsonPropertyName("SITE_NAME")]
        public string? SITE_NAME { get; set; }

        [JsonPropertyName("URL")]
        public string? URL { get; set; }

        [JsonPropertyName("NAVIGATE_PAGES")]
        public List<NavigatePage>? NAVIGATE_PAGES { get; set; }

        [JsonPropertyName("LOGIC")]
        public string? LOGIC { get; set; }
    }
    public class ConfigNodeBase {
        [JsonPropertyName("TYPE")]
        public string? TYPE { get; set; }
        [JsonPropertyName("NODE")]
        public string? NODE { get; set; }
        [JsonPropertyName("ATTRIBUTE")]
        public string? ATTRIBUTE { get; set; }
        [JsonPropertyName("REGEX")]
        public string? REGEX { get; set; }
    }
    public class TARGETLINK : ConfigNodeBase {
        [JsonPropertyName("TEXT")]
        public string? TEXT { get; set; }
    }
    public class NavigatePage {
        [JsonPropertyName("TYPE")]
        public NavigatePageTypes TYPE { get; set; }

        [JsonPropertyName("TARGET_LINK")]
        public TARGETLINK? TARGET_LINK { get; set; }

        [JsonPropertyName("PAGINATION")]
        public Pagination? PAGINATION { get; set; }
    }
    public class Pagination {
        [JsonPropertyName("NODE")]
        public string? NODE { get; set; }

        [JsonPropertyName("ACTION")]
        public string? ACTION { get; set; }

        [JsonPropertyName("MAX_LOOP")]
        public int? MAX_LOOP { get; set; }
    }
}
