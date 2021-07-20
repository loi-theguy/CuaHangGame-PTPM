using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_HoaDon : DataHelper<HoaDon>
    {
        public override int Delete(string pID)
        {
            try
            {
                HoaDon hoaDon = context.HoaDons.FirstOrDefault(hd => hd.MaHD == pID);
                if (hoaDon == null) return NONEXISTENT;
                context.HoaDons.DeleteOnSubmit(hoaDon);
                context.SubmitChanges();
            }
            catch {
                return DEPENDED;
            }
            return SUCCESS;
        }

        public override List<HoaDon> GetData()
        {
            return context.HoaDons.Select(hd=>hd).ToList();
        }

        public override bool Insert(HoaDon entity)
        {
            try
            {
                if(string.IsNullOrEmpty(entity.MaHD)) entity.MaHD = GenerateID();
                context.HoaDons.InsertOnSubmit(entity);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public override bool Update(HoaDon entity)
        {
            try {
                HoaDon hoaDon = context.HoaDons.FirstOrDefault(hd => hd.MaHD == entity.MaHD);
                hoaDon.MaTK = entity.MaTK;
                hoaDon.NgayLap = entity.NgayLap;
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public override string GenerateID()
        {
            string type = "HD";
            int max = -1;
            foreach (HoaDon hoaDon in context.HoaDons.Select(hd => hd))
            {
                int temp = int.Parse(hoaDon.MaHD.Substring(2));
                if (temp > max) max = temp;
            }
            max += 1;
            string id = max.ToString();
            while (id.Length < 7)
            {
                id = "0" + id;
            }
            return type + id;
        }
        public List<CTHoaDon> GetDataCTHoaDon(string pMaHD)
        {
            return context.CTHoaDons.Select(ct => ct).Where(ct => ct.MaHD == pMaHD).ToList();
        }
        public bool DeleteCTHoaDon(string pMaHD, string pMaGame)
        {
            try {
                CTHoaDon chiTiet = context.CTHoaDons.FirstOrDefault(ct => ct.MaHD == pMaHD && ct.MaGame == pMaGame);
                context.CTHoaDons.DeleteOnSubmit(chiTiet);
                context.SubmitChanges();
            } catch { return false; }
            return true;
        }
        public bool InsertCTHoaDon(string pMaHD, string pMaGame)
        {
            try
            {
                CTHoaDon chiTiet = new CTHoaDon();
                chiTiet.MaHD = pMaHD;
                chiTiet.MaGame = pMaGame;
                context.CTHoaDons.InsertOnSubmit(chiTiet);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public List<HoaDon> GetData(string pMaTK)
        {
            return context.HoaDons.Select(hd => hd).Where(hd => hd.MaTK == pMaTK).ToList();
        }

        public bool IsExisted(string pMaHD, string pMaGame)
        {
            return context.CTHoaDons.FirstOrDefault(ct => ct.MaGame == pMaGame && ct.MaHD == pMaHD) != null;
        }

        public List<View_CTHD> GetView_CTHDs(string pMaHD)
        {
            return context.View_CTHDs.Select(ct => ct).Where(ct => ct.MaHD == pMaHD).ToList();
        }

        public List<View_HoaDon> GetView_HoaDons()
        {
            return context.View_HoaDons.Select(hd => hd).ToList();
        }

        private bool AreSameDates(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }
        public double GetDateRevenue(DateTime date)
        {
            List<HoaDon> hoaDons = context.HoaDons.ToList();
            List<HoaDon> hds = new List<HoaDon>();
            double sum = 0;
            foreach (HoaDon hd in hoaDons)
                if (AreSameDates(date, (DateTime)hd.NgayLap))
                    hds.Add(hd);
            BLDAL_Game gameHelper = new BLDAL_Game();
            foreach (HoaDon hd in hds)
            {
                List<CTHoaDon> cts = GetDataCTHoaDon(hd.MaHD);
                foreach (CTHoaDon ct in cts)
                    sum +=(double) gameHelper.GetGame(ct.MaGame).DonGia;
            }
            return sum;
        }

        public List<TrainData> GetTrainDatas(DateTime start, DateTime end)
        {
            List<TrainData> result = new List<TrainData>();
            for (DateTime day = start; DateTime.Compare(day, end) <= 0; day=day.AddDays(1))
            {
                DateTime d = new DateTime(day.Year, day.Month, day.Day);
                double revenue = GetDateRevenue(day);
                result.Add(new TrainData() { Date =d, Revenue=revenue});
            }
            return result;
        }
    }
}
