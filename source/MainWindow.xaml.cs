using matome_phase1.scraper;
using matome_phase1.scraper.Interface;
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
    public partial class MainWindow : Window {
        //config.json取得
        string configPathURL = "https://raw.githubusercontent.com/miyas02/matome_phase1/refs/heads/master/source/docs/SampleConfig.json";

        public MainWindow() {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("スクレイピングを開始します。");
            using HttpClient client = new HttpClient();
            string config = client.GetStringAsync(configPathURL).Result;

            IScraperOwner scraperOwner = new ScraperOwner();
            List<Object> Items = scraperOwner.LoadConfig(config);
            MessageBox.Show($"スクレイピングが完了しました。取得件数: {Items.Count}件");
        }
    }
}