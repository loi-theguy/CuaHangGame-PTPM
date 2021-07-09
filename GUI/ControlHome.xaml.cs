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
    /// Interaction logic for ControlHome.xaml
    /// </summary>
    public partial class ControlHome : UserControl
    {
        public ControlCart ParentCart { get; set; }
        public MainWindow ParentMain { get; set; }

        BLDAL_Game gameHelper = new BLDAL_Game();

        public ControlGameDetail ControlGameDetail { get; set; }
        public ControlHome()
        {
            InitializeComponent();
            List<Game> list = gameHelper.GetData();
            ControlGameDetail = new ControlGameDetail();
            DataHelper dataBindingHelper = new DataHelper();
            foreach (Game game in list)
            {
                GameCard gameCard = new GameCard();
                gameCard.imgGame.Source = dataBindingHelper.GetBitmapImage(game.HinhDaiDien);
                gameCard.tbTitle.Text = game.TenGame;
                gameCard.tbPrice.Text = game.DonGia.ToString();
                gameCard.txtID.Text = game.MaGame;
                gameCard.btn.Click += BtnDetailGameCard_Click;
                spNewGameContent.Children.Add(gameCard);
            }
            spNewGameContent.UpdateLayout();
            foreach (Game game in list)
            {
                GameCard gameCard = new GameCard();
                gameCard.imgGame.Source = dataBindingHelper.GetBitmapImage(game.HinhDaiDien);
                gameCard.tbTitle.Text = game.TenGame;
                gameCard.tbPrice.Text = game.DonGia.ToString();
                gameCard.txtID.Text = game.MaGame;
                gameCard.btn.Click += BtnDetailGameCard_Click;
                spHotGameContent.Children.Add(gameCard);
            }
            spHotGameContent.UpdateLayout();
            foreach (Game game in list)
            {
                WideGameCard gameCard = new WideGameCard();
                gameCard.imgGame.Source = dataBindingHelper.GetBitmapImage(game.HinhDaiDien);
                gameCard.tbTitle.Text = game.TenGame;
                gameCard.tbPrice.Text = game.DonGia.ToString();
                gameCard.txtID.Text = game.MaGame;
                gameCard.btn.Click += BtnDetailWideGameCard_Click;
                spAllContent.Children.Add(gameCard);
            }
            spAllContent.UpdateLayout();
        }

        private void BtnDetailWideGameCard_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Content;
            TextBlock txt = (TextBlock)grid.Children[1];
            DisplayDetails(txt.Text);
        }

        private void BtnDetailGameCard_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Content;
            TextBlock txt = (TextBlock)grid.Children[3];
            DisplayDetails(txt.Text);
        }

        private void DisplayDetails(string pMaGame)
        {
            if (ControlGameDetail == null) ControlGameDetail = new ControlGameDetail();
            Game game = gameHelper.GetGame(pMaGame);
            ControlGameDetail.ParentMain = ParentMain;
            ControlGameDetail.SelectedGame = game;
            ControlGameDetail.ParentCart = ParentCart;
            ControlGameDetail.UpdateDetails();
            ParentMain.svMainContent.Content = ControlGameDetail;
            ParentMain.UpdateLayout();
        }
    }
}
