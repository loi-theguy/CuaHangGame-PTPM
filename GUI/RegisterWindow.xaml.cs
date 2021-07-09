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
using System.Windows.Shapes;
using BLDAL;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GUI
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public LoginWindow LoginParent { get; set; }
        BLDAL_KhachHang khHelper = new BLDAL_KhachHang();
        public RegisterWindow()
        {
            InitializeComponent();
            txtError.Visibility = Visibility.Hidden;
            Closed += RegisterWindow_Closed;
        }

        private void RegisterWindow_Closed(object sender, EventArgs e)
        {
            LoginParent.Show();
        }

        private bool CheckPassword()
        {
            return txtMK.Password == txtXacNhanMK.Password && txtMK.Password != string.Empty;        
        }

        private bool IsPhoneNumberValid()
        {
            long t;
            if (!long.TryParse(txtSoDienThoai.Text, out t))
                return false;
            if (txtSoDienThoai.Text.Length != 10)
                return false;
            return true;
        }

        private bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private bool HasEmptyField()
        {
            if (txtHoTen.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return true;
            }
            if (txtNgaySinh.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!");
                return true;
            }
            if (txtSoDienThoai.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập lại số điện thoại!");
                return true;
            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!");
                return true;
            }
            if (string.IsNullOrEmpty(txtMK.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                return true;
            }
            if (string.IsNullOrEmpty(txtXacNhanMK.Password))
            {
                MessageBox.Show("Vui lòng xác nhận lại mật khẩu!");
                return true;
            }
            return false;
        }

        private void txtXacNhanMK_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (CheckPassword())
            {
                txtError.Visibility = Visibility.Hidden;
                return;
            }
            if (txtMK.Password == txtXacNhanMK.Password && txtMK.Password == string.Empty)
                txtError.Content = "Mật khẩu không hợp lệ!";
            else txtError.Content = "Mật khẩu không khớp!";
            txtError.Visibility = Visibility.Visible;
        }

        private void btnConfirmRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!khHelper.IsUsernameValid(txtUsername.Text) || HasEmptyField() || !CheckPassword())
                return;
            if (!IsPhoneNumberValid())
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return;
            }
            if (!IsEmailValid(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!");
                return;
            }
            if (DateTime.Now.Year - txtNgaySinh.SelectedDate.Value.Year < 18)
            {
                MessageBox.Show("Tuổi phải từ 18 trở lên để đăng ký");
                return;
            }
            TaiKhoan tk = new TaiKhoan();
            tk.HoTen = txtHoTen.Text;
            tk.NgaySinh = txtNgaySinh.SelectedDate;
            tk.SoDienThoai = txtSoDienThoai.Text;
            tk.Email=txtEmail.Text;
            tk.Username = txtUsername.Text;
            tk.Pass = khHelper.ComputeHash(txtMK.Password);
            if (khHelper.Insert(tk))
            {
                MessageBox.Show("Đăng ký thành công!");
                return;
            }
            MessageBox.Show("Đăng ký không thành công, vui lòng thử lại!");
        }
    }
}
