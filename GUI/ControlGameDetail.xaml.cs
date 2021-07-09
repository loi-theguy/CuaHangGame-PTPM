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
    /// Interaction logic for controlGameDetail.xaml
    /// </summary>
    public partial class ControlGameDetail : UserControl
    {
        public Game SelectedGame { get; set; }
        public ControlCart ParentCart { get; set; }
        public MainWindow ParentMain { get; set; }
        public bool IsInitializedFromSearch { get; set; }
        public bool IsInitializedFromCart { get; set; }
        public bool IsInitializedFromPurchasedGames { get; set; }

        private DataHelper helper;
        private BLDAL_NhaSanXuat nsxHelper;
        private BLDAL_HoaDon hdHelper;
        private BLDAL_TheLoai tlHelper;
        public ControlGameDetail()
        {
            InitializeComponent();
            helper = new DataHelper();
            hdHelper = new BLDAL_HoaDon();
            tlHelper = new BLDAL_TheLoai();
            nsxHelper = new BLDAL_NhaSanXuat();
            Loaded += ControlGameDetail_Loaded;
        }

        private void ControlGameDetail_Loaded(object sender, RoutedEventArgs e)
        {
            
            UpdateDetails();
        }
        public void UpdateDetails()
        {
            List<HoaDon> hoaDons = hdHelper.GetData(ParentMain.User.MaTK);
            btnThemVaoGio.Visibility = Visibility.Visible;
            foreach (HoaDon hd in hoaDons)
            {
                if (hdHelper.IsExisted(hd.MaHD, SelectedGame.MaGame))
                {
                    btnThemVaoGio.Visibility = Visibility.Hidden;
                    break;
                }
            }
            imgGame.Source = helper.GetBitmapImage(SelectedGame.HinhDaiDien);
            txtTenGame.Text ="Tên game: "+ SelectedGame.TenGame;
            txtMoTa.Text ="Mô tả: "+SelectedGame.MoTa;
            txtNhaSanXuat.Text = "Nhà sản xuất: " + nsxHelper.GetNhaSanXuat(SelectedGame.MaNSX).TenNSX;
            string theLoai = "Thể loại: ";
            bool first = true;
            foreach (Game_TheLoai gtl in tlHelper.GetDataGame_TheLoais(SelectedGame.MaGame))
            {
                if (!first)
                {
                    theLoai += ", ";
                }
                theLoai += tlHelper.GetTheLoai(gtl.MaTL).TenTL;
                first = false;
            }
            txtTheLoai.Text = theLoai;
            txtDonGia.Text = "Giá: " + SelectedGame.DonGia.ToString();
            UpdateLayout();
        }

        private void btnThemVaoGio_Click(object sender, RoutedEventArgs e)
        {
            if (ParentCart.AddItem(SelectedGame))
            {
                ParentCart.UpdateCart();
                MessageBox.Show("Thêm vào giỏ thành công!");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (IsInitializedFromCart)
            {
                ParentMain.ReturnToCart();
                return;
            }
            if (IsInitializedFromSearch)
            {
                ParentMain.ReturnToSearch();
                return;
            }
            if (IsInitializedFromPurchasedGames)
            {
                ParentMain.ReturnToPurchasedGames();
                return;
            }
            ParentMain.ReturnToHome();
        }
    }
}
