using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using matome_phase1.constants;
using System.Diagnostics;


namespace matome_phase1.scraper {
    internal class ScraperOwner : IScraperOwner  {
        public void LoadConfig(string configPath) {
            string json = File.ReadAllText(configPath);
            using JsonDocument doc = JsonDocument.Parse(json);
            string logicValue = doc.RootElement.GetProperty("LOGIC").GetString();

            //jsonのlogic属性のvalueで生成するインスタンスを切り替え
            //サイトカテゴリごとに条件分岐を追加する
            if (logicValue == ScraperLogics.Posts) {
                PostsScraperConfig ScraperConfig = JsonSerializer.Deserialize<PostsScraperConfig>(json);
                List<Post> Posts = ScraperConfig.GetPosts();
                foreach (var post in Posts) {
                    Debug.WriteLine($"ID: {post.Id}, UserID: {post.UserId}, Text: {post.Text}, Date: {post.Date}");
                }
                Debug.WriteLine(Posts);
            }
        }
    }
}
