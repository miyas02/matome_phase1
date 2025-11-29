```mermaid
---
title: Scraperクラス図
config:
  theme: forest
---
classDiagram

%% Interfact,Abstract,Component定義
class UI
<<Component>> UI
class IScraperOwner {
    <<Interface>>
    + IScraperService 
    + LoadConfig(configPaht)void
}
class IScraperService
<<Interface>>IScraperService
class AbstractScraperConfig
<<Abstract>>AbstractScraperConfig

%% クラス定義
class ScraperOwner{
    + IScraperService
    + LoadCofig(configFIlePath)void
}
class ScraperService {
  - GetDriver(string url)
}

class PostsScraperConfig

class PostsScraperService {
  + GetItems(AbstractScraperConfig) List<Object>
  # DocParseItems(AConfig, HtmlDocument doc) List<Object>
  - DocParsePosts(AConfig, HtmlDocument doc) List<Object>
  - GetInnerText(HtmlNode postNode, string node)
}

%% 関連
UI --> IScraperOwner: インスタンス化
IScraperOwner <|-- ScraperOwner
IScraperOwner --* IScraperService
IScraperService <|-- ScraperService
ScraperService <|-- PostsScraperService
IScraperOwner --* AbstractScraperConfig
AbstractScraperConfig <|-- PostsScraperConfig
```