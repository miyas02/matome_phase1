using matome_phase1.scraper.Configs;

namespace matome_phase1.scraper.Models {
    public record ItemsVM(ScraperConfig Config, List<Dictionary<string, string>> Items);
}
