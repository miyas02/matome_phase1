using HtmlAgilityPack;
using matome_phase1.scraper.Configs;

namespace matome_phase1.scraper.Interface {
    public interface IScraperService {
        public List<Dictionary<string, string>> GetItems(ScraperConfig scraperConfig);
        public List<Dictionary<string, string>> DocParseItems(ScraperConfig scraperConfig, HtmlDocument doc);
    }
}
