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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LoginWindow LoginParent { get; set; }
        public TaiKhoan User { get; set; }
        //private Dictionary<string, string> menuItemsTitle = new Dictionary<string, string>();
        private BLDAL_Quyen quyenHelper;
        private Dictionary<string, RadioButton> menuItems = new Dictionary<string, RadioButton>();
        private ControlHome controlHome = null;
        private ControlCart controlCart = null;
        private ControlTimKiem controlTimKiem = null;
        private ControlGameDaMua controlGameDaMua = null;
        private ControlQuanLyNSX controlQuanLyNSX = null;
        private ControlQuanLyKH controlQuanLyKH = null;
        private ControlQuanLyNhanVien controlQuanLyNhanVien = null;
        private ControlQuanLyHD controlQuanLyHD = null;
        private ControlQuanLyTL controlQuanLyTL = null;
        private ControlQuanLyGame controlQuanLyGame = null;
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
            Loaded += MainWindow_Loaded;
            quyenHelper = new BLDAL_Quyen();
            controlHome = new ControlHome();
            controlCart = new ControlCart();
            controlTimKiem = new ControlTimKiem();
            controlGameDaMua = new ControlGameDaMua();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtGreeting.Text = "Xin chào " + User.HoTen;
            InitSideMenuItems();
            LoadSideMenu();
        }

        private void InitSideMenuItems()
        {
            //menuItemsTitle.Add("QU00000001", "Quản lý nhân viên");
            //menuItemsTitle.Add("QU00000002", "Quản lý game");
            //menuItemsTitle.Add("QU00000003", "Quản lý nhà sản xuất");
            //menuItemsTitle.Add("QU00000004", "Quản lý khách hàng");
            //menuItemsTitle.Add("QU00000005", "Quản lý hóa đơn");
            //menuItemsTitle.Add("QU00000006", "Thống kê - Report");
            //menuItemsTitle.Add("QU00000007", "Trang chủ");
            //menuItemsTitle.Add("QU00000009", "Quản lý giỏ hàng");
            //menuItemsTitle.Add("QU00000010", "Tìm kiếm");
            //foreach (KeyValuePair<string, string> pair in menuItemsTitle)
            //{
            //    menuItems.Add(pair.Key, new RadioButton());
            //    menuItems[pair.Key].Content = pair.Value;
            //    Style style = FindResource("MenuButtonTheme") as Style;
            //    menuItems[pair.Key].Style = style;
            //}
            List<Quyen> quyens = quyenHelper.GetData();
            foreach (Quyen quyen in quyens)
            {
                menuItems.Add(quyen.MaQuyen, new RadioButton());
                menuItems[quyen.MaQuyen].Content = quyen.TenQuyen;
                Style style = FindResource("MenuButtonTheme") as Style;
                menuItems[quyen.MaQuyen].Style = style;
            }
            RadioButton btnQuanLyNhanVien = menuItems["QU00000001"];
            btnQuanLyNhanVien.Checked += BtnQuanLyNhanVien_Checked;
            RadioButton btnQuanLyGame = menuItems["QU00000002"];
            btnQuanLyGame.Checked += BtnQuanLyGame_Checked;
            RadioButton btnQuanLyNSX= menuItems["QU00000003"];
            btnQuanLyNSX.Checked += BtnQuanLyNSX_Checked;
            RadioButton btnQuanLyKH= menuItems["QU00000004"];
            btnQuanLyKH.Checked += BtnQuanLyKH_Checked;
            RadioButton btnQuanLyHD= menuItems["QU00000005"];
            btnQuanLyHD.Checked += BtnQuanLyHD_Checked;
            RadioButton btnThongKeReport= menuItems["QU00000006"];
            btnThongKeReport.Checked += BtnThongKeReport_Checked;
            RadioButton btnTrangChu= menuItems["QU00000007"];
            btnTrangChu.Checked += BtnTrangChu_Checked;
            RadioButton btnQuanLyGH= menuItems["QU00000009"];
            btnQuanLyGH.Checked += BtnQuanLyGH_Checked;
            RadioButton btnTimKiem= menuItems["QU00000010"];
            btnTimKiem.Checked += BtnTimKiem_Checked;
            RadioButton btnQuanLyTL = menuItems["QU00000011"];
            btnQuanLyTL.Checked += BtnQuanLyTL_Checked;

        }
        #region menu items' checked events =>adding custom control callers here
        private void BtnQuanLyTL_Checked(object sender, RoutedEventArgs e)
        {
            if (controlQuanLyTL == null) controlQuanLyTL = new ControlQuanLyTL();
            svMainContent.Content = controlQuanLyTL;
            svMainContent.UpdateLayout();
        }
        
        private void BtnTimKiem_Checked(object sender, RoutedEventArgs e)
        {
            if (controlTimKiem == null) controlTimKiem = new ControlTimKiem();
            controlTimKiem.ParentMain = this;
            controlTimKiem.ParentCart = controlCart;
            svMainContent.Content = controlTimKiem;
            svMainContent.UpdateLayout();
        }

        private void BtnQuanLyGH_Checked(object sender, RoutedEventArgs e)
        {
            if (controlCart == null) controlCart = new ControlCart();
            controlCart.ParentMain = this;
            controlCart.User = User;
            svMainContent.Content = controlCart;
            svMainContent.UpdateLayout();
        }

        private void BtnTrangChu_Checked(object sender, RoutedEventArgs e)
        {
            if (controlHome == null) controlHome = new ControlHome();
            controlHome.ControlGameDetail.IsInitializedFromSearch = false;
            controlHome.ControlGameDetail.IsInitializedFromCart = false;
            controlHome.ControlGameDetail.IsInitializedFromPurchasedGames = false;
            controlHome.ParentMain = this;
            controlHome.ParentCart = controlCart;
            svMainContent.Content = controlHome;
            svMainContent.UpdateLayout();
        }

        private void BtnThongKeReport_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnQuanLyHD_Checked(object sender, RoutedEventArgs e)
        {
            if (controlQuanLyHD == null) controlQuanLyHD = new ControlQuanLyHD();
            svMainContent.Content = controlQuanLyHD;
            svMainContent.UpdateLayout();
        }

        private void BtnQuanLyKH_Checked(object sender, RoutedEventArgs e)
        {
            if (controlQuanLyKH == null) controlQuanLyKH = new ControlQuanLyKH();
            svMainContent.Content = controlQuanLyKH;
            svMainContent.UpdateLayout();
        }

        private void BtnQuanLyNSX_Checked(object sender, RoutedEventArgs e)
        {
            if (controlQuanLyNSX == null) controlQuanLyNSX = new ControlQuanLyNSX();
            svMainContent.Content = controlQuanLyNSX;
            svMainContent.UpdateLayout();
        }

        private void BtnQuanLyGame_Checked(object sender, RoutedEventArgs e)
        {
            if (controlQuanLyGame == null) controlQuanLyGame = new ControlQuanLyGame();
            controlQuanLyGame.UpdateData();
            svMainContent.Content = controlQuanLyGame;
            svMainContent.UpdateLayout();
        }

        private void BtnQuanLyNhanVien_Checked(object sender, RoutedEventArgs e)
        {
            if (controlQuanLyNhanVien == null) controlQuanLyNhanVien = new ControlQuanLyNhanVien();
            svMainContent.Content = controlQuanLyNhanVien;
            svMainContent.UpdateLayout();
        }
        private void BtnGameDaMua_Checked(object sender, RoutedEventArgs e)
        {
            controlGameDaMua.ParentCart = controlCart;
            controlGameDaMua.ParentMain = this;
            controlGameDaMua.UpdateDetail();
            svMainContent.Content = controlGameDaMua;
            svMainContent.UpdateLayout();
        }
        #endregion
        private void LoadSideMenu()
        {
            bool first = true;
            BLDAL_NhomQuyen nhomHelper = new BLDAL_NhomQuyen();
            List<CTNhomQuyen> listQuyen = nhomHelper.GetDataCTNhomQuyen(User.MaNhom);
            foreach (CTNhomQuyen quyen in listQuyen)
            {
                if (quyen.MaQuyen == "QU00000008") continue;
                RadioButton button = menuItems[quyen.MaQuyen];
                button.IsChecked = first;
                first = false;
                SidePanel.Children.Add(button);
            }
            if (User.MaNhom == "NQ00000002")
            {
                RadioButton button = new RadioButton();
                Style style = FindResource("MenuButtonTheme") as Style;
                button.Style = style;
                button.Content = "Game đã mua";
                button.Click += BtnGameDaMua_Checked;
                SidePanel.Children.Add(button);
            }
            SidePanel.UpdateLayout();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            LoginParent.Show();
        }
        public void ReturnToHome()
        {
            BtnTrangChu_Checked(null, null);
        }
        public void ReturnToSearch()
        {
            BtnTimKiem_Checked(null, null);
        }

        public void ReturnToPurchasedGames()
        {
            BtnGameDaMua_Checked(null, null);
        }
        public void ReturnToCart()
        {
            BtnQuanLyGH_Checked(null, null);
        }

        private void btnDoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow window = new ChangePasswordWindow();
            window.ParentMain = this;
            window.ShowDialog();
        }
    }
}
