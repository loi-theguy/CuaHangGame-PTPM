using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ControlQuanLyNSX.xaml
    /// </summary>
    public partial class ControlQuanLyNSX : UserControl
    {
        private BLDAL_NhaSanXuat nsxHelper;
        private DataHelper helper;
        public ControlQuanLyNSX()
        {
            InitializeComponent();
            nsxHelper = new BLDAL_NhaSanXuat();
            helper = new DataHelper();
            //dgNSX.ItemsSource = data;
            Loaded += ControlQuanLyNSX_Loaded;
        }

        private void ControlQuanLyNSX_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            dgNSX.ItemsSource= nsxHelper.GetData();
            SelectItemAt(0);
        }

        private void dgNSX_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgNSX.SelectedItems.Count == 0) return;
            NhaSanXuat nsx = (NhaSanXuat)dgNSX.SelectedItems[0];
            txtTenNSX.Text = nsx.TenNSX;
            txtSoDienThoai.Text = nsx.SoDienThoai;
            txtEmail.Text = nsx.Email;
            txtDiaChi.Text = nsx.DiaChi;
        }
        private void SelectItemAt(int index)
        {
            if (dgNSX.Items.Count == 0) return;
            dgNSX.SelectedItems.Clear();
            object item = dgNSX.Items[index];
            dgNSX.SelectedItem = item;
            dgNSX.ScrollIntoView(item);
        }

        private bool HasSelected()
        {
            return dgNSX.SelectedItems.Count > 0;
        }
        private bool HasEmptyField()
        {
            if (string.IsNullOrEmpty(txtTenNSX.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà sản xuất");
                return true;
            }
            
            return false;
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            if (!helper.IsPhoneNumberValid(txtSoDienThoai.Text) && !string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return;
            }
            if (!helper.IsEmailValid(txtEmail.Text) && !string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ");
                return;
            }
            NhaSanXuat nsx = new NhaSanXuat();
            nsx.TenNSX = txtTenNSX.Text;
            nsx.SoDienThoai = txtSoDienThoai.Text;
            nsx.Email = txtEmail.Text;
            nsx.DiaChi = txtDiaChi.Text;
            if (nsxHelper.Insert(nsx))
            {
                MessageBox.Show("Thêm thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Thêm thất bại, vui lòng thử lại");  
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelected())
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 dòng dữ liệu trước khi xóa");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn xóa dữ liệu?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result==MessageBoxResult.Yes)
            {
                foreach (object item in dgNSX.SelectedItems)
                {
                    NhaSanXuat nsx = (NhaSanXuat)item;
                    if (nsxHelper.Delete(nsx.MaNSX) != BLDAL_TaiKhoan.SUCCESS)
                    {
                        MessageBox.Show("Không thể xóa " + nsx.MaNSX);
                        UpdateData();
                        return;
                    }
                }
                UpdateData();
                MessageBox.Show("Xóa thành công");
                return;
            }
        }

        private bool FullCheck()
        {
            if (!HasSelected())
            {
                MessageBox.Show("Vui lòng chọn 1 dòng dữ liệu");
                return false;
            }
            if (HasEmptyField()) return false;
            if (!helper.IsPhoneNumberValid(txtSoDienThoai.Text) && !string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }
            if (!helper.IsEmailValid(txtEmail.Text) && !string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ");
                return false;
            }
            return true;
        }
        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (!FullCheck()) return;
            MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn cập nhật dữ liệu?",
                "Xác nhận cập nhật", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            NhaSanXuat nsx = (NhaSanXuat)dgNSX.SelectedItems[0];
            NhaSanXuat nsxInDb = nsxHelper.GetNhaSanXuat(nsx.MaNSX);
            nsxInDb.TenNSX = txtTenNSX.Text;
            nsxInDb.SoDienThoai = txtSoDienThoai.Text;
            nsxInDb.Email = txtEmail.Text;
            nsxInDb.DiaChi = txtDiaChi.Text;
            if (nsxHelper.Update(nsxInDb))
            {
                MessageBox.Show("Cập nhật thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Cập nhật thất bại");
        }
    }
}
