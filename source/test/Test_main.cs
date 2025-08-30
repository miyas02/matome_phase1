using matome_phase1.scraper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace matome_phase1.test {
    /// <summary>
    /// UIのテスト用クラス
    /// </summary>
    internal class Test_main {
        //config.json取得
        string configPathURL = "https://raw.githubusercontent.com/miyas02/matome_phase1/refs/heads/master/source/docs/SampleConfig.json";

    public Test_main() {
            using HttpClient client = new HttpClient();
            string config = client.GetStringAsync(configPathURL).Result;

            IScraperOwner scraperOwner = new ScraperOwner();
            scraperOwner.LoadConfig(config);
        }

        //scraperOwnerにconfig.jsonを渡す
    }
}
