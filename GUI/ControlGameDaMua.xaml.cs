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
    /// Interaction logic for ControlGameDaMua.xaml
    /// </summary>
    public partial class ControlGameDaMua : UserControl
    {
        public MainWindow ParentMain { get; set; }
        public ControlCart ParentCart { get; set; }
        private BLDAL_Game gameHelper;
        private DataHelper helper=new DataHelper();
        private ControlGameDetail controlGameDetail;
        public ControlGameDaMua()
        {
            InitializeComponent();
            gameHelper = new BLDAL_Game();
        }

        public ControlGameDaMua(MainWindow parentMain, ControlCart parentCart)
        {
            InitializeComponent();
            ParentMain = parentMain;
            ParentCart = parentCart;
            gameHelper = new BLDAL_Game();
        }

        public void UpdateDetail()
        {
            spContent.Children.Clear();
            List<Game> games = gameHelper.GetDataGameDaMua(ParentMain.User.MaTK);
            txtNotif.Visibility = Visibility.Hidden;
            if (games.Count == 0)
            {
                txtNotif.Visibility = Visibility.Visible;
                spContent.UpdateLayout();
                return;
            }
            foreach (Game game in games)
            {
                WideGameCard card = new WideGameCard();
                card.imgGame.Source = helper.GetBitmapImage(game.HinhDaiDien);
                card.tbTitle.Text = game.TenGame;
                card.tbPrice.Visibility = Visibility.Hidden;
                card.btn.Click += BtnDetail_Click;
                card.txtID.Text = game.MaGame;
                spContent.Children.Add(card);
            }
            spContent.UpdateLayout();
        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
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
            controlGameDetail.IsInitializedFromSearch = false;
            controlGameDetail.IsInitializedFromCart = false;
            controlGameDetail.IsInitializedFromPurchasedGames = true;
            ParentMain.svMainContent.Content = controlGameDetail;
            ParentMain.UpdateLayout();
        }
    }
}
