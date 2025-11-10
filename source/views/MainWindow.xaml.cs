using matome_phase1.scraper;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace matome_phase1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged{
        //config.json取得
        string configPathURL = "https://raw.githubusercontent.com/miyas02/matome_phase1/refs/heads/master/source/docs/target_config/Config.json";

        //postsの実態(バッキングフィールド)
        private List<Post> _posts = new List<Post>();
        public List<Post> Posts {
            get { return _posts; }//バッキングフィールドの値を返す
            set {
                _posts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Posts)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow() {
            InitializeComponent();
            DataContext = this;
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("スクレイピングを開始します。");
            using HttpClient client = new HttpClient();
            string config = client.GetStringAsync(configPathURL).Result;

            IScraperOwner scraperOwner = new ScraperOwner();
            List<Object> Items = scraperOwner.LoadConfig(config);
            Posts = Items.ConvertAll(item => (Post)item);
            MessageBox.Show($"スクレイピングが完了しました。取得件数: {Items.Count}件");
        }
    }
}