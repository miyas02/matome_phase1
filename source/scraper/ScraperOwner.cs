﻿using System;
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
        public required IScraperService ScraperService { get; set; }
        public required AbstractScraperConfig AConfig { get; set; }
        public void LoadConfig(string config) {
            using JsonDocument doc = JsonDocument.Parse(config);
            string logicValue = doc.RootElement.GetProperty("LOGIC").GetString() ?? "default";

            //jsonのlogic属性のvalueで生成するインスタンスを切り替え
            //サイトカテゴリごとに条件分岐を追加する
            AConfig = ScraperFactory.Create(config);
            //AConfigのプロパティのnullチェック
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            string te = AConfig.LOGIC;

            ScraperService = ScraperFactory.Create(AConfig);

            //TODO AConfigの入力(読み込み)チェック

            List<Object> Items = ScraperService.GetItems(AConfig);
            foreach (var item in Items) {
                if (item is Post) {
                    var p = (Post)item;
                    Debug.WriteLine($"ID: {p.Id}, UserID: {p.UserId}, Text: {p.Text}, Date: {p.Date}");
                }
            }
            Debug.WriteLine(Items);
        }
    }
}
