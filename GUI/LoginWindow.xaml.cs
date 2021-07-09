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

namespace GUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        BLDAL_TaiKhoan tkHelper = new BLDAL_TaiKhoan();
        public LoginWindow()
        {
            InitializeComponent();
            //MainWindow main = new MainWindow();
            //main.Show();
            //DataHelper helper = new DataHelper();
            //string test = helper.ComputeHash("Hello");
            //MessageBox.Show(test);
        }

        private bool HasEmptyField()
        {
            if (txtUsername.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!");
                return true;
            }
            if (txtPassword.Password == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                return true;
            }
            return false;
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow();
            window.LoginParent = this;
            window.Show();
            Hide();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField()) return;
            if (tkHelper.IsUserValid(txtUsername.Text, txtPassword.Password))
            {
                MainWindow main = new MainWindow();
                main.User = tkHelper.GetTaiKhoan(txtUsername.Text);
                main.LoginParent = this;
                main.Show();
                Hide();
                return;
            }
            MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng, vui lòng thử lại.");
        }
    }
}
