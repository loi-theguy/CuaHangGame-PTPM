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
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public MainWindow ParentMain { get; set; }
        private BLDAL_TaiKhoan tkHelper; 
        public ChangePasswordWindow()
        {
            InitializeComponent();
            tkHelper = new BLDAL_TaiKhoan();
        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyField())
                return;
            if (txtNewPassword.Password != txtNewPasswordConf.Password)
            {
                MessageBox.Show("Mật khẩu mới không trùng khớp!");
                return;
            }
            if (tkHelper.ComputeHash(txtOldPassword.Password) != ParentMain.User.Pass)
            {
                MessageBox.Show("Sai mật khẩu cũ!");
                return;
            }
            ParentMain.User.Pass = tkHelper.ComputeHash(txtNewPassword.Password);
            if (tkHelper.Update(ParentMain.User))
            {
                MessageBox.Show("Đổi mật khẩu thành công");
                return;
            }
            MessageBox.Show("Đổi mật khẩu thất bại, vui lòng thử lại");
        }

        private bool HasEmptyField()
        {
            if (string.IsNullOrEmpty(txtOldPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cũ");
                return true;
            }
            if (string.IsNullOrEmpty(txtNewPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới");
                return true;
            }
            if (string.IsNullOrEmpty(txtNewPasswordConf.Password))
            {
                MessageBox.Show("Vui lòng xác nhận mật khẩu mới");
                return true;
            }
            return false;
        }
    }
}
