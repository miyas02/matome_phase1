using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    public abstract class AbstractPost {

        public string Id;
        public DateTime Date;
        public string Text;
        public string UserId;
        public string Reply;
        public string ImageUrl;

    }
}
