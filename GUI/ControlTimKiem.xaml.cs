using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLDAL;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ControlTimKiem.xaml
    /// </summary>
    public partial class ControlTimKiem : UserControl
    {
        public MainWindow ParentMain { get; set; }
        public ControlCart ParentCart { get; set; }
        private ControlGameDetail controlGameDetail = null;
        private List<Game> result;
        private BLDAL_Game gameHelper;
        DataHelper helper;
        public ControlTimKiem()
        {
            InitializeComponent();
            result = new List<Game>();
            gameHelper = new BLDAL_Game();
            helper = new DataHelper();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            spResultContent.Children.Clear();
            if (txtTuKhoa.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm");
                return;
            }
            result = gameHelper.Search(txtTuKhoa.Text);
            if (result == null ||result.Count==0)
            {
                MessageBox.Show("Không có tựa game phù hợp với từ khóa bạn vừa nhập, vui lòng thử lại!");
                return;
            }
            foreach (Game game in result)
            {
                WideGameCard card = new WideGameCard();
                card.txtID.Text = game.MaGame;
                card.imgGame.Source = helper.GetBitmapImage(game.HinhDaiDien);
                card.tbTitle.Text = game.TenGame;
                card.tbPrice.Text = game.DonGia.ToString();
                card.btn.Click += BtnDetails_Click;
                spResultContent.Children.Add(card);
            }
            spResultContent.UpdateLayout();
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Content;
            TextBlock txt = (TextBlock)grid.Children[1];
            DisplayDetails(txt.Text);
        }

        private void DisplayDetails(string pMaGame)
        {
            if (controlGameDetail == null) controlGameDetail = new ControlGameDetail();
            Game game = gameHelper.GetGame(pMaGame);
            controlGameDetail.ParentMain = ParentMain;
            controlGameDetail.SelectedGame = game;
            controlGameDetail.ParentCart = ParentCart;
            controlGameDetail.UpdateDetails();
            controlGameDetail.IsInitializedFromSearch = true;
            controlGameDetail.IsInitializedFromCart = false;
            controlGameDetail.IsInitializedFromPurchasedGames = false;
            ParentMain.svMainContent.Content = controlGameDetail;
            ParentMain.UpdateLayout();
        }
    }
}
