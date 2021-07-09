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
    /// Interaction logic for ControlQuanLyTL.xaml
    /// </summary>
    public partial class ControlQuanLyTL : UserControl
    {
        private BLDAL_TheLoai tlHelper;
        public ControlQuanLyTL()
        {
            InitializeComponent();
            tlHelper = new BLDAL_TheLoai();
            Loaded += ControlQuanLyTL_Loaded;
        }

        private void ControlQuanLyTL_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void dgTL_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            txtTenTL.Text = string.Empty;
            if (HasSelected())
            {
                TheLoai tl = (TheLoai)dgTL.SelectedItems[0];
                txtTenTL.Text = tl.TenTL;
            }
        }
        private bool HasSelected()
        {
            return dgTL.SelectedItems.Count != 0;
        }
        private bool HasEmptyField()
        {
            if (string.IsNullOrEmpty(txtTenTL.Text))
            {
                MessageBox.Show("Vui lòng không bỏ trống tên thể loại");
                return true;
            }
            return false;
        }

        public void UpdateData()
        {
            dgTL.ItemsSource = tlHelper.GetData();
            SelectItemAt(0);
        }
        private void SelectItemAt(int index)
        {
            if (dgTL.Items.Count == 0) return;
            dgTL.SelectedItems.Clear();
            object item = dgTL.Items[index];
            dgTL.SelectedItem = item;
            dgTL.ScrollIntoView(item);
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            TheLoai tl = new TheLoai();
            tl.TenTL = txtTenTL.Text;
            if (tlHelper.Insert(tl))
            {
                MessageBox.Show("Thêm thể loại thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Thêm thể loại thất bại");
            return;
        }
        private bool ConfirmAction(string action)
        {
            MessageBoxResult result = MessageBox.Show(action,
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelected())
            {
                MessageBox.Show("Vui lòng chọn dòng để xóa");
                return;
            }
            if (!ConfirmAction("Bạn có chắc muốn xóa dữ liệu?")) return;
            foreach (object item in dgTL.SelectedItems)
            {
                TheLoai tl = (TheLoai)item;
                if (tlHelper.Delete(tl.MaTL) != BLDAL_TheLoai.SUCCESS)
                {
                    MessageBox.Show("Xóa thất bại thể loại " + tl.TenTL);
                    UpdateData();
                    return;
                }
            }
            UpdateData();
            MessageBox.Show("Xóa thành công");
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            if (!HasSelected())
            {
                MessageBox.Show("Vui lòng chọn dòng để cập nhật");
                return;
            }
            if (!ConfirmAction("Bạn chắc chắc muốn cập nhật dữ liệu?")) return;
            TheLoai tl = (TheLoai)dgTL.SelectedItems[0];
            TheLoai tlInDb = tlHelper.GetTheLoai(tl.MaTL);
            tlInDb.TenTL = txtTenTL.Text;
            if (tlHelper.Update(tlInDb))
            {
                MessageBox.Show("Cập nhật thể loại thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Cập nhật thể loại thất bại");
            UpdateData();
        }
    }
}
