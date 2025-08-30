using OpenQA.Selenium.DevTools.V136.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Interface {
    internal interface IScraperOwner {
        public void LoadConfig(string configPath);
    }
}
