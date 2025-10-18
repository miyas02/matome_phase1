using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Models {
    public class Post {
        public string? Id { 
            get; set; 
        }
        public DateTime? Date{ 
            get; set; 
        }
        public string? Text { 
            get; set; 
        }
        public string? UserId {
            get; set;
        }
        public string? Reply {
            get; set;
        }
        public string? ImageUrl {
            get; set;
        }
    }
}
