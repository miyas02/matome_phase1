using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Models {
    internal class EC {
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? img { get; set; }

        public override bool Equals(object? obj) {
            if (obj is EC other) {
                return Name == other.Name && Price == other.Price && img == other.img;
            }
            return false;
        }
    }
}
