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
    /// Interaction logic for ControlQuanLyNhanVien.xaml
    /// </summary>
    public partial class ControlQuanLyNhanVien : UserControl
    {
        private BLDAL_NhanVienQuanLy nvHelper;
        private BLDAL_TaiKhoan tkHelper;
        private DataHelper helper;
        public ControlQuanLyNhanVien()
        {
            InitializeComponent();
            helper = new DataHelper();
            tkHelper = new BLDAL_TaiKhoan();
            nvHelper = new BLDAL_NhanVienQuanLy();
            Loaded += ControlQuanLyNhanVien_Loaded;
        }

        private void ControlQuanLyNhanVien_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            dgNV.ItemsSource = nvHelper.GetView_NVQL();
            SelectItemAt(0);
        }
        private void SelectItemAt(int index)
        {
            if (dgNV.Items.Count == 0) return;
            dgNV.SelectedItems.Clear();
            object item = dgNV.Items[index];
            dgNV.SelectedItem = item;
            dgNV.ScrollIntoView(item);
        }

        private void dgNV_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgNV.SelectedItems.Count == 0) return;
            View_NVQL tk = (View_NVQL)dgNV.SelectedItems[0];
            txtHoTen.Text = tk.HoTen;
            txtEmail.Text = tk.Email;
            txtSoDienThoai.Text = tk.SoDienThoai;
            txtNgaySinh.SelectedDate = tk.NgaySinh;
            txtUsername.Text = tk.Username;
            txtLuongCoBan.Text = tk.LuongCoBan.ToString();
            txtPhuCapTrachNhiem.Text = tk.PhuCapTrachNhiem.ToString();
        }

        private bool HasSelected()
        {
            if (dgNV.SelectedItems.Count > 0)
            {
                return true;
            }
            MessageBox.Show("Vui lòng chọn dòng dữ liệu");
            return false;
        }

        private bool HasEmptyField()
        {
            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên");
                return true;
            }
            if (txtNgaySinh.SelectedDate == null)
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
            if (string.IsNullOrEmpty(txtLuongCoBan.Text))
            {
                MessageBox.Show("Vui lòng nhập lương cơ bản");
                return true;
            }
            if (string.IsNullOrEmpty(txtPhuCapTrachNhiem.Text))
            {
                MessageBox.Show("Vui lòng nhập phụ cấp trách nhiệm");
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
            if (!tkHelper.IsUsernameValid(txtUsername.Text) && !isEditting)
            {
                MessageBox.Show("Tên tài khoản này đã tồn tại");
                return false;
            }
            double d = 0;
            if (!double.TryParse(txtLuongCoBan.Text, out d))
            {
                MessageBox.Show("Lương cơ bản không hợp lệ");
                return false;
            }
            if (!double.TryParse(txtPhuCapTrachNhiem.Text, out d))
            {
                MessageBox.Show("Phụ cấp trách nhiệm không hợp lệ");
                return false;
            }
            return true;
        }
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            if (!AreAllFieldsValid(false)) return;
            NhanVienQuanLy nv = new NhanVienQuanLy();
            TaiKhoan tk = new TaiKhoan();
            tk.HoTen = txtHoTen.Text;
            tk.NgaySinh = txtNgaySinh.SelectedDate;
            tk.SoDienThoai = txtSoDienThoai.Text;
            tk.Email = txtEmail.Text;
            tk.Username = txtUsername.Text;
            tk.Pass = nvHelper.ComputeHash(txtUsername.Text);
            nv.TaiKhoan = tk;
            nv.LuongCoBan = double.Parse(txtLuongCoBan.Text);
            nv.PhuCapTrachNhiem = double.Parse(txtPhuCapTrachNhiem.Text);
            if (nvHelper.Insert(nv))
            {
                MessageBox.Show("Thêm nhân viên thành công");
                UpdateData();
                return;
            }
            UpdateData();
            MessageBox.Show("Thêm nhân viên thất bại");
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
            foreach (object item in dgNV.SelectedItems)
            {
                View_NVQL tk = (View_NVQL)item;
                if (nvHelper.Delete(tk.MaTK) != BLDAL_NhanVienQuanLy.SUCCESS)
                {
                    MessageBox.Show("Không thể xóa " + tk.MaTK);
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
            View_NVQL tk = (View_NVQL)dgNV.SelectedItems[0];
            NhanVienQuanLy nvInDb = nvHelper.GetNhanVienQuanLy(tk.MaTK);
            TaiKhoan tkInDb = nvInDb.TaiKhoan;
            tkInDb.HoTen = txtHoTen.Text;
            tkInDb.NgaySinh = txtNgaySinh.SelectedDate;
            tkInDb.SoDienThoai = txtSoDienThoai.Text;
            tkInDb.Email = txtEmail.Text;
            tkInDb.Username = txtUsername.Text;
            nvInDb.LuongCoBan = double.Parse(txtLuongCoBan.Text);
            nvInDb.PhuCapTrachNhiem = double.Parse(txtPhuCapTrachNhiem.Text);
            if (nvHelper.Update(nvInDb))
            {
                MessageBox.Show("Cập nhật thông tin thành công");
                UpdateData();
                return;
            }
            MessageBox.Show("Cập nhật thông tin thất bại");
        }
    }
}
