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
    /// Interaction logic for ControlQuanLyHD.xaml
    /// </summary>
    public partial class ControlQuanLyHD : UserControl
    {
        BLDAL_HoaDon hdHelper;
        public ControlQuanLyHD()
        {
            InitializeComponent();
            hdHelper = new BLDAL_HoaDon();
            Loaded += ControlQuanLyHD_Loaded;
        }

        private void ControlQuanLyHD_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }
        private void SelectItemAt(int index)
        {
            if (dgHD.Items.Count == 0) return;
            dgHD.SelectedItems.Clear();
            object item = dgHD.Items[index];
            dgHD.SelectedItem = item;
            dgHD.ScrollIntoView(item);
        }
        private void dgHD_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            dgGame.ItemsSource = null;
            txtTongTien.Text = string.Empty;
            if (dgHD.SelectedItems.Count == 0) return;
            View_HoaDon hd = (View_HoaDon) dgHD.SelectedItems[0];
            List<View_CTHD> list = hdHelper.GetView_CTHDs(hd.MaHD);
            dgGame.ItemsSource = list;
            double t = 0;
            foreach (View_CTHD ct in list) t +=(double) ct.DonGia;
            txtTongTien.Text = t.ToString() + " VND";
        }

        public void UpdateData()
        {
            dgHD.ItemsSource = hdHelper.GetView_HoaDons();
            SelectItemAt(0);
        }

        private bool ConfirmAction(string action)
        {
            MessageBoxResult result = MessageBox.Show(action,
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgHD.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 dòng dữ liệu để xóa");
                return;
            }
            if (ConfirmAction("Bạn chắc chắn muốn xóa hóa đơn?"))
            {
                foreach (object item in dgHD.SelectedItems)
                {
                    View_HoaDon hd = (View_HoaDon)item;
                    List<CTHoaDon> cthd = hdHelper.GetDataCTHoaDon(hd.MaHD);
                    foreach (CTHoaDon ct in cthd)
                    {
                        hdHelper.DeleteCTHoaDon(ct.MaHD, ct.MaGame);
                    }
                    if (hdHelper.Delete(hd.MaHD) != BLDAL_HoaDon.SUCCESS)
                    {
                        MessageBox.Show("Xóa thất bại hóa đơn "+hd.MaHD+", vui lòng thử lại");
                        UpdateData();
                        return;
                    }
                }
                UpdateData();
                MessageBox.Show("Xóa thành công");
            }
        }
    }
}
