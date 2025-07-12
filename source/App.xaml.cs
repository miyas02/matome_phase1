﻿using System.Configuration;
using System.Data;
using System.Windows;
using matome_phase1.scraper;

namespace matome_phase1 {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            Iscraper scraper = new spla3Scraper();
            scraper.GetPosts();
        }
    }

}
