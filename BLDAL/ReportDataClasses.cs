using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class GameReportResult
    {
        public string STT { get; set; }
        public string MaGame { get; set; }
        public string TenGame { get; set; }
        public string DonGia { get; set; }
        public string SoLuong { get; set; }
        public string ThanhTien { get; set; }
    }

    public class NhanVienReportInfo
    { 
        public string STT { get; set; }
        public string MaTK { get; set; }
        public string HoTen { get; set; }
        public string LuongCoBan { get; set; }
        public string PhuCapTrachNhiem { get; set; }
        public string LuongThang { get; set; }
    }

    public class TrainData {
        public DateTime Date { get; set; }
        public double Revenue { get; set; }
    }
}
