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
using WordExcelExport;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ControlThongKeReport.xaml
    /// </summary>
    public partial class ControlThongKeReport : UserControl
    {
        private BLDAL_Game gameHelper;
        private BLDAL_NhanVienQuanLy nvHelper;
        private BackPropagationModel model;
        public TaiKhoan User { get; set; }
        public ControlThongKeReport()
        {
            InitializeComponent();
            gameHelper = new BLDAL_Game();
            nvHelper = new BLDAL_NhanVienQuanLy();
            Loaded += ControlThongKeReport_Loaded;
        }

        private void ControlThongKeReport_Loaded(object sender, RoutedEventArgs e)
        {
            if (User.MaNhom != "NQ00000000")
            {
                btnKhenThuong.Visibility = Visibility.Hidden;
                btnThongKeNhanVien.Visibility = Visibility.Hidden;
            }
                
            cbNhanVien.ItemsSource = nvHelper.GetView_NVQL();
        }

        private bool HasEmptyOrInvalidFieldForKhenThuong()
        {
            if (cbNhanVien.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn");
                return true;
            }
            if (string.IsNullOrEmpty(txtLyDo.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do khen thưởng");
                return true;
            }
            if (string.IsNullOrEmpty(txtSoTien.Text))
            {
                MessageBox.Show("Vui lòng nhập số tiền thưởng");
                return true;
            }
            long t = 0;
            if (!long.TryParse(txtSoTien.Text, out t)|| t <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền là số nguyên dương");
                return true;
            }

            return false;
        }
        private bool HasEmptyFieldForReport()
        {
            if (txtNgayBatDau.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu thống kê");
                return true;
            }
            if (txtNgayKetThuc.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày kết thúc thống kê");
                return true;
            }
            return false;
        }
        private void btnThongKeGame_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyFieldForReport()) return;
            ExcelExport export = new ExcelExport();
            List<GameReportResult> data = gameHelper.GetGameReportResults((DateTime)txtNgayBatDau.SelectedDate,(DateTime) txtNgayKetThuc.SelectedDate);
            string fileName = "";
            export.ThongKeGame(data, ref fileName, true, User.HoTen);
        }

        private void btnThongKeNhanVien_Click(object sender, RoutedEventArgs e)
        {
            ExcelExport export = new ExcelExport();
            List<NhanVienReportInfo> data = nvHelper.GetNhanVienReportInfos();
            string fileName = "";
            export.ThongKeNhanVien(data, ref fileName, true, User.HoTen);
        }

        private void btnKhenThuong_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyOrInvalidFieldForKhenThuong()) return;
            WordExport export = new WordExport();
            string ngay = DateTime.Now.Day.ToString();
            string thang = DateTime.Now.Month.ToString();
            string nam = DateTime.Now.Year.ToString();
            string tenNV = ((View_NVQL)cbNhanVien.SelectedItem).HoTen;
            export.KhenThuong("01", ngay, thang, nam, txtLyDo.Text, tenNV, txtSoTien.Text);
        }

        private void txtNgayBDTrain_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (model == null) model = new BackPropagationModel();
            model.Start =(DateTime) txtNgayBDTrain.SelectedDate;
            model.End = new DateTime(2021, 7, 12);
            string result=model.Predict().ToString();
            if (!model.Success)
            {
                MessageBox.Show("Vui lòng chọn ngày hợp lệ");
                return;
            }
            dgTrainData.ItemsSource = model.TrainDatas;
            txtDoanhThu.Text = result;
        }
    }
}
