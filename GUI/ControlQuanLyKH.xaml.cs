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
    /// Interaction logic for ControlQuanLyKH.xaml
    /// </summary>
    public partial class ControlQuanLyKH : UserControl
    {
        private BLDAL_KhachHang khHelper;
        private DataHelper helper;
        public ControlQuanLyKH()
        {
            InitializeComponent();
            helper = new DataHelper();
            khHelper = new BLDAL_KhachHang();
            Loaded += ControlQuanLyKH_Loaded;
        }

        private void ControlQuanLyKH_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            dgKH.ItemsSource = khHelper.GetData();
            SelectItemAt(0);
        }
        private void SelectItemAt(int index)
        {
            if (dgKH.Items.Count == 0) return;
            dgKH.SelectedItems.Clear();
            object item = dgKH.Items[index];
            dgKH.SelectedItem = item;
            dgKH.ScrollIntoView(item);
        }

        private void dgKH_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgKH.SelectedItems.Count == 0) return;
            TaiKhoan tk = (TaiKhoan)dgKH.Items[0];
            txtHoTen.Text = tk.HoTen;
            txtEmail.Text = tk.Email;
            txtSoDienThoai.Text = tk.SoDienThoai;
            txtNgaySinh.SelectedDate = tk.NgaySinh;
            txtUsername.Text = tk.Username;
        }

        private bool HasSelected()
        {
            if (dgKH.SelectedItems.Count > 0)
            {
                return true;
            }
            MessageBox.Show("Vui lòng chọn dòng dữ liệu");
            return false ;
        }

        private bool HasEmptyField()
        {
            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng");
                return true;
            }
            if (txtNgaySinh.SelectedDate==null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh");
                return true;
            }
            if (string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại");
                return true;
            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email");
                return true;
            }
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản");
                return true;
            }
            return false;
        }


        private bool AreAllFieldsValid(bool isEditting)
        {
            if (DateTime.Now.Year - txtNgaySinh.SelectedDate.Value.Year < 18)
            {
                MessageBox.Show("Tuổi phải từ 18 trở lên");
                return false;
            }
            if (!helper.IsPhoneNumberValid(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }
            if (!helper.IsEmailValid(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ");
                return false;
            }
            if (!khHelper.IsUsernameValid(txtUsername.Text)&&!isEditting)
            {
                MessageBox.Show("Tên tài khoản này đã tồn tại");
                return false;
            }
            return true;
        }
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            if (!AreAllFieldsValid(false)) return;
            TaiKhoan tk = new TaiKhoan();
            tk.HoTen = txtHoTen.Text;
            tk.NgaySinh = txtNgaySinh.SelectedDate;
            tk.SoDienThoai = txtSoDienThoai.Text;
            tk.Email = txtEmail.Text;
            tk.Username = txtUsername.Text;
            tk.Pass = khHelper.ComputeHash(txtUsername.Text);
            if (khHelper.Insert(tk))
            {
                MessageBox.Show("Thêm khách hàng thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Thêm khách hàng thất bại");
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
            foreach (object item in dgKH.SelectedItems)
            {
                TaiKhoan tk = (TaiKhoan)item;
                if (khHelper.Delete(tk.MaTK)!=BLDAL_KhachHang.SUCCESS)
                {
                    MessageBox.Show("Không thể xóa "+tk.MaTK);
                    UpdateData();
                    return;
                }
            }
            UpdateData();
            MessageBox.Show("Xóa thành công");
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelected()) return;
            if (HasEmptyField()) return;
            if (!AreAllFieldsValid(true)) return;
            if (!ConfirmAction("Bạn chắc chắn muốn sửa thông tin?")) return;
            TaiKhoan tk = (TaiKhoan)dgKH.SelectedItems[0];
            TaiKhoan tkInDb = khHelper.GetKhachHang(tk.MaTK);
            tkInDb.HoTen = txtHoTen.Text;
            tkInDb.NgaySinh = txtNgaySinh.SelectedDate;
            tkInDb.SoDienThoai = txtSoDienThoai.Text;
            tkInDb.Email = txtEmail.Text;
            tkInDb.Username = txtUsername.Text;
            if (khHelper.Update(tkInDb))
            {
                MessageBox.Show("Cập nhật thông tin thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Cập nhật thông tin thất bại");
        }
    }
}
