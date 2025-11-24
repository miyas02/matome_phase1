using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using matome_phase1.constants;
using System.Diagnostics;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Interface;


namespace matome_phase1.scraper {
    internal class ScraperOwner : IScraperOwner  {
        public IScraperService ScraperService { get; set; }
        public AbstractScraperConfig AConfig { get; set; }
        public ItemsVM ItemsVM { get; set; }
        public ItemsVM LoadConfig(string config) {
            //jsonのlogic属性のvalueで生成するインスタンスを切り替え
            AConfig = ScraperFactory.Create(config);
            //AConfigのプロパティのnullチェック
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }

            ScraperService = ScraperFactory.Create(AConfig);

            //TODO AConfigの入力(読み込み)チェック
            List<Object> Items = ScraperService.GetItems(AConfig);
            ItemsVM = new ItemsVM(AConfig,Items);
            return ItemsVM;
        }
    }
}
