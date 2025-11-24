using matome_phase1.scraper.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Models {
    public record ItemsVM(AbstractScraperConfig Config, List<object> Items);

}
