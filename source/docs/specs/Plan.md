# 改修したいこと
- ScraperをSeleniumからPlaywrightに変更する
- Configのデータ構造を変更
- UIをいい感じにしたい→優先度低、将来的にはblazorかasp.netでwebアプリにしたい
- テストコードの充実→unitTestSammaryを追加してunitTestのカバレッジ管理、unitTestを充実+github ActionsでCI環境構築

---

## Configのデータ構造を変更

### 背景・目的

- Configを生成するシステムを別途開発予定のため、webページからConfigを自動生成しやすい構造にしたい
- 現状はフィールド名（`TEXT`, `USER_ID`, `DATE`など）がコード側に固定されており、新サイト追加のたびにコード変更が必要
- `LOGIC`の値（`Posts` / `News` / `EC`）が曖昧で、NewsとECは本質的に同じ操作なのに別扱いになっている

### 現状の問題

```csharp
// PostConfig.cs にフィールドが固定定義されている
public ConfigNodeBase? TEXT { get; set; }
public ConfigNodeBase? USER_ID { get; set; }
public ConfigNodeBase? DATE { get; set; }
// ← 新フィールドを追加するたびにコード変更が必要
```

### 新しいデータ構造

`LOGIC`を廃止し、`EXTRACT`に抽出したいものを直接定義する。

```json
{
  "SITE_NAME": "サイト名",
  "URL": "https://...",
  "NAVIGATE_PAGES": [...],

  "EXTRACT": {
    "任意のキー名": <抽出定義>
  }
}
```

`EXTRACT`の値は2パターン。`ITEM`の有無で識別する。

| パターン | 使いどころ             | 識別方法 |
|---------|----------------------|---------|
| Single  | 特定の1要素を取得       | `ITEM`なし |
| List    | 繰り返し要素を全件取得   | `ITEM`あり |

#### Single（特定要素1つ）

```json
"EXTRACT": {
  "page_title": {
    "NODE": "//h1[@class='title']",
    "TYPE": "text"
  },
  "og_image": {
    "NODE": "//meta[@property='og:image']",
    "TYPE": "attribute",
    "ATTRIBUTE": "content"
  },
  "price": {
    "NODE": "//span[@class='price']",
    "TYPE": "text",
    "REGEX": "[\\d,]+"
  }
}
```

#### List（繰り返し要素）

```json
"EXTRACT": {
  "posts": {
    "CONTEXT": "//ul[@id='thread']",
    "ITEM": ".//li[contains(@class, 'response')]",
    "FIELDS": {
      "text":    { "NODE": ".//div[@class='body']", "TYPE": "text" },
      "user_id": { "NODE": ".//div[@class='info']", "TYPE": "text", "REGEX": "([0-9A-Za-z+\\-/]+)(?= \\[)" },
      "date":    { "NODE": ".//div[@class='date']", "TYPE": "text" }
    }
  }
}
```

#### SingleとListの混在も可能

```json
{
  "SITE_NAME": "5ch",
  "URL": "https://...",
  "EXTRACT": {
    "thread_title": {
      "NODE": "//h1",
      "TYPE": "text"
    },
    "posts": {
      "CONTEXT": "//ul[@id='thread']",
      "ITEM": ".//li",
      "FIELDS": {
        "text":    { "NODE": ".//div[@class='body']", "TYPE": "text" },
        "user_id": { "NODE": ".//div[@class='info']", "TYPE": "text" }
      }
    }
  }
}
```

### C#側のデータ構造

```csharp
// 抽出定義
public class ExtractDef
{
    public string? NODE { get; set; }       // Single用 XPath
    public string? TYPE { get; set; }       // "text" | "attribute" | "html"
    public string? ATTRIBUTE { get; set; } // TYPE=attributeの場合
    public string? REGEX { get; set; }     // オプション

    public string? CONTEXT { get; set; }   // List用 コンテナXPath（省略可）
    public string? ITEM { get; set; }      // List用 各要素XPath
    public Dictionary<string, ExtractDef>? FIELDS { get; set; } // List用 フィールド定義
}

// 抽出結果
abstract class ExtractResult { }
class StringResult : ExtractResult { string Value; }
class ListResult   : ExtractResult { List<Dictionary<string, string>> Items; }

// Config
public class ScraperConfig
{
    public string SITE_NAME { get; set; }
    public string URL { get; set; }
    public List<NavigatePage>? NAVIGATE_PAGES { get; set; }
    public Dictionary<string, ExtractDef> EXTRACT { get; set; }
}
```

### 抽出処理（コードは変わらない）

```csharp
foreach (var (key, def) in config.EXTRACT)
{
    if (def.ITEM != null)
        results[key] = new ListResult(ParseList(doc, def));
    else
        results[key] = new StringResult(ParseSingle(doc, def));
}
// ← JSONに何を書こうとコードは変わらない
```

### メリット

- `LOGIC`フィールドが不要になる
- フィールド名はJSONで自由に定義→コード変更なしで新サイト対応可能
- SingleとListを同一Config内に混在できる
- UI側もキー名からDataGridカラムを動的生成できる
- Config自動生成システムが扱いやすいシンプルな構造
=======
- Configのデータ構造を変更→要件定義から。Configをつくるシステムは別で開発予定だから、webページからConfigを自動生成しやすいデータ構造にしたい
- UIをいい感じにしたい→優先度低、将来的にはblazorかasp.netでwebアプリにしたい
- テストコードの充実→unitTestSammaryを追加してunitTestのカバレッジ管理、unitTestを充実+github ActionsでCI環境構築
>>>>>>> 5fa3a1804a648899c8560f197a5d7ec4216e94d1
