namespace matome_phase1.scraper.Configs {
    public class ExtractDef {
        // Single用
        public string? NODE { get; set; }
        public string? TYPE { get; set; }
        public string? ATTRIBUTE { get; set; }
        public string? REGEX { get; set; }

        // List用
        public string? CONTEXT { get; set; }
        public string? ITEM { get; set; }
        public Dictionary<string, ExtractDef>? FIELDS { get; set; }
    }
}
