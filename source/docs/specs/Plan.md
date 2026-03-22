# 改修したいこと
- ScraperをSeleniumからPlaywrightに変更する
- Configのデータ構造を変更→要件定義から。Configをつくるシステムは別で開発予定だから、webページからConfigを自動生成しやすいデータ構造にしたい
- UIをいい感じにしたい→優先度低、将来的にはblazorかasp.netでwebアプリにしたい
- テストコードの充実→unitTestSammaryを追加してunitTestのカバレッジ管理、unitTestを充実+github ActionsでCI環境構築

## Configのデータ構造を変更→要件定義から。Configをつくるシステムは別で開発予定だから、webページからConfigを自動生成しやすいデータ構造にしたい
現状（コード定義が必要）
                                                                                                                                     // PostConfig.cs に固定フィールドが存在
  public ConfigNodeBase? TEXT { get; set; }
  public ConfigNodeBase? USER_ID { get; set; }
  public ConfigNodeBase? DATE { get; set; }
  // ← 新フィールドを追加するたびにコード変更が必要

  新構造（完全動的）

  // Config
  public Dictionary<string, ExtractDef> EXTRACT { get; set; }

  // 抽出処理
  foreach (var (key, def) in config.EXTRACT)
  {
      if (def.ITEM != null)
          results[key] = new ListResult(ParseList(doc, def));
      else
          results[key] = new StringResult(ParseSingle(doc, def));
  }
  // ← JSONに何を書こうとコードは変わらない

  JSONに "my_custom_field" と書けばそのままキーになります。コードは一切知りません。