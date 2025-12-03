using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Models {
    internal class News {
        public string? Name { get; init; }
        public string? link { get; init; }
        public string? img { get; init; }

        public News(string name, string link, string img) {
            Name = name;
            this.link = link;
            this.img = img;
        }

        public override bool Equals(object? obj) {
            if (obj is EC other) {
                return Name == other.Name && link == other.Price && img == other.img;
            }
            return false;
        }
    }
}
