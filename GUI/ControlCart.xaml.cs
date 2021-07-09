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
    /// Interaction logic for ControlCart.xaml
    /// </summary>
    public partial class ControlCart : UserControl
    {
        public List<Game> CartItems { get; set; }
        public MainWindow ParentMain { get; set; }
        private ControlGameDetail controlGameDetail = null;
        private BLDAL_Game gameHelper;
        private BLDAL_HoaDon hdHelper;
        private DataHelper helper;
        public TaiKhoan User { get; set; }
        public ControlCart()
        {
            InitializeComponent();
            helper = new DataHelper();
            hdHelper = new BLDAL_HoaDon();
            gameHelper = new BLDAL_Game();
            CartItems = new List<Game>();
            Loaded += ControlCart_Loaded;
        }

        private void ControlCart_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCart();
        }

        public void UpdateCart()
        {
            spCartContent.Children.Clear();
            if (CartItems.Count == 0)
            {
                txtNotif.Visibility = Visibility.Visible;
                btnThanhToan.Visibility = Visibility.Hidden;
                UpdateLayout();
                return;
            }
            txtNotif.Visibility = Visibility.Hidden;
            btnThanhToan.Visibility = Visibility.Visible;
            foreach (Game game in CartItems)
            {
                WideGameCard card = new WideGameCard();
                card.imgGame.Source = helper.GetBitmapImage(game.HinhDaiDien);
                card.tbTitle.Text = game.TenGame;
                card.tbPrice.Visibility = Visibility.Hidden;
                card.btnXoa.Visibility = Visibility.Visible;
                card.btnXoa.Click += BtnXoa_Click;
                card.btn.Click += BtnDetail_Click;
                card.txtID.Text = game.MaGame;
                card.txtXoaID.Text = game.MaGame;
                spCartContent.Children.Add(card);
            }
            spCartContent.UpdateLayout();
            
            
        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Content;
            TextBlock txt = (TextBlock)grid.Children[1];
            DisplayDetails(txt.Text);
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Content;
            TextBlock txt = (TextBlock)grid.Children[1];
            foreach (Game game in CartItems)
            {
                if (game.MaGame == txt.Text)
                {
                    CartItems.Remove(game);
                    UpdateCart();
                    return;
                }
            }  
        }
        private void DisplayDetails(string pMaGame)
        {
            if (controlGameDetail == null) controlGameDetail = new ControlGameDetail();
            Game game = gameHelper.GetGame(pMaGame);
            controlGameDetail.ParentMain = ParentMain;
            controlGameDetail.SelectedGame = game;
            controlGameDetail.ParentCart = this;
            controlGameDetail.UpdateDetails();
            controlGameDetail.IsInitializedFromSearch = false;
            controlGameDetail.IsInitializedFromCart = true;
            controlGameDetail.IsInitializedFromPurchasedGames = false;
            ParentMain.svMainContent.Content = controlGameDetail;
            ParentMain.UpdateLayout();
        }

        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            HoaDon hoaDon = new HoaDon();

            hoaDon.MaHD = hdHelper.GenerateID();
            hoaDon.MaTK = User.MaTK;
            hoaDon.NgayLap = DateTime.Now;
            if (!hdHelper.Insert(hoaDon))
            {
                MessageBox.Show("Tạo hóa đơn thất bại, vui lòng thử lại.");
                return;
            }
            foreach (Game game in CartItems)
            {
                if(!hdHelper.InsertCTHoaDon(hoaDon.MaHD, game.MaGame))
                {
                    MessageBox.Show("Thêm chi tiết hóa đơn thất bại, vui lòng thử lại.");
                    return;
                }
            }
            CartItems.Clear();
            UpdateCart();
            MessageBox.Show("Thanh toán thành công!");
        }

        public bool AddItem(Game game)
        {
            for (int i = 0; i < CartItems.Count; i++)
                if (CartItems[i].MaGame == game.MaGame)
                {
                    MessageBox.Show("Game này đã có trong giỏ của bạn.");
                    return false;
                }
            CartItems.Add(game);
            return true;
        }
    }
}
