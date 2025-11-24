using matome_phase1.scraper;
using matome_phase1.scraper.Interface;
using matome_phase1.scraper.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Security.RightsManagement;
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

        private ObservableCollection<Object> _currentItems;
        public ObservableCollection<Object> CurrentItems {
            get => _currentItems;
            set { 
                _currentItems = value;
                OnPropertyChanged(nameof(CurrentItems));
            }
        }
        private string _siteName;
        public string SiteName {
            get => _siteName;
            set {
                _siteName = value;
                OnPropertyChanged(nameof(SiteName));
            }
        }
        private string _url;
        public string URL {
            get => _url;
            set {
                _url = value;
                OnPropertyChanged(nameof(URL));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow() {
            InitializeComponent();
            DataContext = this;
        }
        private void ScrapingBtn_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("スクレイピングを開始します。");
            using HttpClient client = new HttpClient();
            string config = client.GetStringAsync(configPathURL).Result;

            IScraperOwner scraperOwner = new ScraperOwner();
            ItemsVM ItemsVM = scraperOwner.LoadConfig(config);
            SiteName = ItemsVM.Config.SITE_NAME;
            URL = ItemsVM.Config.URL;
            switch (ItemsVM.Items.FirstOrDefault()) {
                case Post:
                    CurrentItems = new ObservableCollection<Object>(ItemsVM.Items.ConvertAll(item => (Post)item));
                    break;
                case EC:
                    CurrentItems = new ObservableCollection<Object>(ItemsVM.Items.ConvertAll(item => (EC)item));
                    break;
            }
            MessageBox.Show($"スクレイピングが完了しました。取得件数: {ItemsVM.Items.Count}件");
        }
        private void ClearBtn_Click(object sender, RoutedEventArgs e) {
            CurrentItems.Clear();
            SiteName = string.Empty;
            URL = string.Empty;
        }
        private void ItemsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            if (e.PropertyName != "Text") return;
            e.Column.Width = new DataGridLength(600);
            if (e.Column is DataGridTextColumn textColumn) {
                if (ItemsDataGrid.Resources["WrapTextBlockStyle"] is Style wrapStyle) {
                    textColumn.ElementStyle = wrapStyle;
                }
            }
        }

    }
}