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
    /// Interaction logic for ControlQuanLyGame.xaml
    /// </summary>
    public partial class ControlQuanLyGame : UserControl
    {
        private BLDAL_Game gameHelper;
        private BLDAL_TheLoai tlHelper;
        private BLDAL_NhaSanXuat nsxHelper;
        public ControlQuanLyGame()
        {
            InitializeComponent();
            gameHelper = new BLDAL_Game();
            tlHelper = new BLDAL_TheLoai();
            nsxHelper = new BLDAL_NhaSanXuat();
            Loaded += ControlQuanLyGame_Loaded;
        }

        private void ControlQuanLyGame_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            dgGame.ItemsSource = gameHelper.GetView_Games();
            cbNSX.ItemsSource = nsxHelper.GetData();
            SelectItemAt(0);
            cbTLBS.SelectedItem = null;
            cbNSX.SelectedItem = null;
        }

        private void SelectItemAt(int index)
        {
            if (dgGame.Items.Count == 0) return;
            dgGame.SelectedItems.Clear();
            object item = dgGame.Items[index];
            dgGame.SelectedItem = item;
            dgGame.ScrollIntoView(item);
        }
        private bool HasSelected()
        {
            if (dgGame.SelectedItems.Count > 0)
            {
                return true;
            }
            MessageBox.Show("Vui lòng chọn dòng dữ liệu");
            return false;
        }
        private bool HasEmptyField()
        {
            if (string.IsNullOrEmpty(txtTenGame.Text))
            {
                MessageBox.Show("Vui lòng nhập tên game");
                return true;
            }
            if (string.IsNullOrEmpty(txtMoTa.Text))
            {
                MessageBox.Show("Vui lòng nhập mô tả");
                return true;
            }
            if (string.IsNullOrEmpty(txtHinhDaiDien.Text))
            {
                MessageBox.Show("Vui lòng nhập tên hình");
                return true;
            }
            if (cbNSX.SelectedItem==null)
            {
                MessageBox.Show("Vui lòng nhà sản xuất");
                return true;
            }
            if (string.IsNullOrEmpty(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn giá");
                return true;
            }
            return false;
        }
        private void dgGame_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            dgTLCG.ItemsSource = null;
            if (dgGame.SelectedItems.Count == 0) return;
            View_Game game = (View_Game)dgGame.SelectedItems[0];
            dgTLCG.ItemsSource = gameHelper.GetTheLoais(game.MaGame);
            cbTLBS.ItemsSource = gameHelper.GetTheLoaiBoSungs(game.MaGame);
            txtTenGame.Text = game.TenGame;
            txtMoTa.Text = game.MoTa;
            txtHinhDaiDien.Text = game.HinhDaiDien;
            txtDonGia.Text = game.DonGia.ToString();
            foreach (object obj in cbNSX.Items)
                if (((NhaSanXuat)obj).MaNSX == game.MaNSX)
                {
                    cbNSX.SelectedItem = obj;
                    break;
                }
        }

        private bool IsDonGiaValid()
        {
            double t = -1;
            if (!double.TryParse(txtDonGia.Text, out t) || t < 0)
            {
                MessageBox.Show("Vui lòng nhập đơn giá hợp lệ");
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            if (!IsDonGiaValid()) return;
            Game game = new Game();
            game.TenGame = txtTenGame.Text;
            game.MoTa = txtMoTa.Text;
            game.HinhDaiDien = txtHinhDaiDien.Text;
            game.MaNSX = ((NhaSanXuat)cbNSX.SelectedItem).MaNSX;
            game.DonGia = double.Parse(txtDonGia.Text);
            if (gameHelper.Insert(game))
            {
                MessageBox.Show("Thêm game thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Thêm game thất bại");
            UpdateData();
        }
        private bool ConfirmAction(string action)
        {
            MessageBoxResult result = MessageBox.Show(action,
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelected()) return;
            if (!ConfirmAction("Bạn chắc chắn muốn xóa dữ liệu?")) return;
            foreach (object item in dgGame.SelectedItems)
            {
                View_Game game = (View_Game)item;
                if (gameHelper.Delete(game.MaGame) != BLDAL_Game.SUCCESS)
                {
                    MessageBox.Show("Xoá thất bại game " + game.TenGame);
                    UpdateData();
                    return;
                }
            }
            MessageBox.Show("Xóa thành công");
            UpdateData();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            if (!HasSelected()) return;
            if (!ConfirmAction("Bạn chắc chắn muốn cập nhật dữ liệu?")) return;
            View_Game game = (View_Game)dgGame.SelectedItems[0];
            Game gameInDb = gameHelper.GetGame(game.MaGame);
            gameInDb.TenGame = txtTenGame.Text;
            gameInDb.MoTa = txtMoTa.Text;
            gameInDb.HinhDaiDien = txtHinhDaiDien.Text;
            gameInDb.DonGia = double.Parse(txtDonGia.Text);
            gameInDb.MaNSX = ((NhaSanXuat)cbNSX.SelectedItem).MaNSX;
            if (gameHelper.Update(gameInDb))
            {
                MessageBox.Show("Cập nhật dữ liệu thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Cập nhật dữ liệu thất bại");
            UpdateData();
        }

        private void btnXoaTL_Click(object sender, RoutedEventArgs e)
        {
            if (dgTLCG.SelectedItems.Count == 0) {
                MessageBox.Show("Vui lòng chọn thể loại cần xóa");
                return;
            }
            if (!ConfirmAction("Bạn chắc chắc muốn xóa thể loại của game?")) return;
            View_Game game = (View_Game) dgGame.SelectedItems[0];
            foreach (object item in dgTLCG.SelectedItems)
            {
                TheLoai tl = (TheLoai)item;
                if (!gameHelper.DeleteGameTheLoai(game.MaGame, tl.MaTL))
                {
                    UpdateData();
                    MessageBox.Show("Xóa thất bại thể loại " + tl.TenTL);
                    return;
                }
            }
            UpdateData();
            MessageBox.Show("Xóa thành công");
        }

        private void btnThemTL_Click(object sender, RoutedEventArgs e)
        {
            if (cbTLBS.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn thể loại cần thêm trước");
                return;
            }
            View_Game game = (View_Game)dgGame.SelectedItems[0];
            if (gameHelper.InsertGameTheLoai(new Game_TheLoai()
                { MaGame = game.MaGame, MaTL = ((TheLoai)cbTLBS.SelectedItem).MaTL }))
            {
                UpdateData();
                MessageBox.Show("Thêm thể loại thành công");
                return;
            }
            UpdateData();
            MessageBox.Show("Thêm thể loại thất bại");

        }
    }
}
