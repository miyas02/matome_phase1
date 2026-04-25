# Exception Spec

## 目的

`ScraperConfig` の入力チェックで発生するエラーを、スクレイピング実行時のエラーと分離して扱う。
設定不正を明確に表現し、呼び出し側が原因を特定しやすい例外設計にする。

## 方針

- `ScraperConfig` の検証失敗は専用の例外で表現する
- HTML解析や画面遷移など、実行時の失敗は既存の `ConfigException` で扱う
- 検証例外は `ErrorCode` と `FieldPath` を持たせ、どの項目がなぜ不正かを一意に示せるようにする
- 例外クラスを細かく増やしすぎず、まずは基底例外 1 つ + enum で管理する

## 例外構造

### 基底例外

`ScraperConfigValidationException`

用途:
- `ScraperConfigValidator` が投げる設定検証用の例外

保持する情報:
- `ErrorCode`
- `FieldPath`
- `Message`
- `InnerException`

### エラーコード

`ScraperConfigValidationErrorCode`

候補:
- `MissingField`
- `InvalidType`
- `InvalidFormat`
- `InvalidValue`
- `InvalidCombination`
- `MutuallyExclusive`
- `EmptyCollection`
- `DuplicateKey`

## FieldPath の設計

`FieldPath` は設定 JSON 上の対象位置を示す文字列とする。

例:
- `SITE_NAME`
- `URL`
- `EXTRACT`
- `EXTRACT.posts`
- `EXTRACT.posts.CONTEXT`
- `EXTRACT.posts.FIELDS.text.TYPE`
- `EXTRACT.posts.FIELDS.image_url.ATTRIBUTE`

この値は、ログ出力、UI 表示、テストの期待値確認に利用する。

## ScraperConfig spec.xlsx との対応

### ルート項目

- `SITE_NAME`
  - 任意
  - 非文字列なら `InvalidType`

- `URL`
  - 必須
  - 未指定なら `MissingField`
  - URL 形式不正なら `InvalidFormat`
  - `http/https` 以外なら `InvalidValue`

- `EXTRACT`
  - 必須
  - 未指定または空なら `MissingField` または `EmptyCollection`
  - map 構造でなければ `InvalidType`

### ExtractDef

- `ExtractDef` は次のどちらか一方のみを許可する
  - list 定義: `CONTEXT + ITEM + FIELDS`
  - single 定義: `NODE + TYPE`

- どちらも満たさない場合
  - `MissingField` または `MutuallyExclusive`

- 両方を同時に満たす場合
  - `InvalidCombination`

### list 定義

- `CONTEXT`
  - 必須セット項目
  - 非文字列なら `InvalidType`
  - XPath 不正なら `InvalidFormat`

- `ITEM`
  - 必須セット項目
  - 非文字列なら `InvalidType`
  - XPath 不正なら `InvalidFormat`

- `FIELDS`
  - 必須セット項目
  - 未指定または空なら `MissingField` または `EmptyCollection`
  - map 構造でなければ `InvalidType`

### single 定義

- `NODE`
  - 必須
  - 非文字列なら `InvalidType`
  - XPath 不正なら `InvalidFormat`

- `TYPE`
  - 必須
  - `text` または `attribute` 以外なら `InvalidValue`

- `ATTRIBUTE`
  - `TYPE == "attribute"` のとき必須
  - 未指定なら `MissingField`
  - `TYPE == "text"` で指定されていた場合の扱いは保留
  - 初期実装では無視するか、将来的に warning 相当の扱いを検討する

## 使用方針

### どこで使うか

- `ScraperOwner.LoadConfig()`
  - JSON デシリアライズ後
  - `IScraperService` 呼び出し前

この位置で検証することで、不正な設定を実行系へ渡さない。

### どこで投げるか

- `ScraperConfigValidator`
  - 各ルールを検証し、違反時に `ScraperConfigValidationException` を throw する

### どこで catch するか

- UI 層または呼び出し元のアプリケーション層
  - `ScraperConfigValidationException` を捕捉して、設定不正として表示する
- 実行時エラーの `ConfigException` とは別ルートで扱う

## メッセージ形式

推奨形式:

```text
[ErrorCode] FieldPath: Message
```

例:

```text
[MissingField] URL: URL is required.
[InvalidFormat] EXTRACT.posts.CONTEXT: CONTEXT is not valid XPath.
[InvalidValue] EXTRACT.posts.FIELDS.image_url.TYPE: TYPE must be 'text' or 'attribute'.
```

## 実装イメージ

```csharp
public enum ScraperConfigValidationErrorCode {
    MissingField,
    InvalidType,
    InvalidFormat,
    InvalidValue,
    InvalidCombination,
    MutuallyExclusive,
    EmptyCollection,
    DuplicateKey
}

public class ScraperConfigValidationException : Exception {
    public ScraperConfigValidationErrorCode ErrorCode { get; }
    public string FieldPath { get; }

    public ScraperConfigValidationException(
        ScraperConfigValidationErrorCode errorCode,
        string fieldPath,
        string message,
        Exception? innerException = null)
        : base(message, innerException) {
        ErrorCode = errorCode;
        FieldPath = fieldPath;
    }
}
```

## 今回の採用判断

- 例外クラスはルールごとに増やさない
- `ScraperConfigValidationException` を 1 つ作る
- 詳細分類は `ScraperConfigValidationErrorCode` で行う
- 既存の `ConfigException` はスクレイピング実行時の失敗用として残す

この方針により、実装量を抑えつつ、ログ・UI・テストで必要な識別性を確保する。
