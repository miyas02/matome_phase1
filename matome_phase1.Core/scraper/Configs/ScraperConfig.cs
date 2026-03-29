namespace matome_phase1.scraper.Configs {
    public class ScraperConfig {
        public string SITE_NAME { get; set; }
        public string URL { get; set; }
        public List<NavigatePage>? NAVIGATE_PAGES { get; set; }
        public Dictionary<string, ExtractDef> EXTRACT { get; set; }
    }
}
