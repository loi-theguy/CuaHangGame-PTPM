using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_NhanVienQuanLy : DataHelper<NhanVienQuanLy>
    {
        BLDAL_TaiKhoan tkHelper = new BLDAL_TaiKhoan();
        //nhom nhan vien quan ly
        private const string NHOM = "NQ00000001";
        public override int Delete(string pID)
        {
            try
            {
                NhanVienQuanLy nhanVien = context.NhanVienQuanLies.FirstOrDefault(nv => nv.MaTK == pID);
                if (nhanVien == null) return NONEXISTENT;
                context.NhanVienQuanLies.DeleteOnSubmit(nhanVien);
                context.SubmitChanges();
                tkHelper.Delete(pID);
            }
            catch
            {
                return DEPENDED;
            }
            return SUCCESS;
        }

        public override List<NhanVienQuanLy> GetData()
        {
            return context.NhanVienQuanLies.Select(nv => nv).ToList();
        }

        public override bool Insert(NhanVienQuanLy entity)
        {
            try
            {
                string id = tkHelper.GenerateID();
                entity.TaiKhoan.MaNhom = NHOM;
                if (string.IsNullOrEmpty(entity.TaiKhoan.MaTK)) entity.TaiKhoan.MaTK = id;
                context.TaiKhoans.InsertOnSubmit(entity.TaiKhoan);
                context.SubmitChanges();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public override bool Update(NhanVienQuanLy entity)
        {
            try
            {
                NhanVienQuanLy nhanVien = context.NhanVienQuanLies.FirstOrDefault(nv => nv.MaTK == entity.MaTK);
                nhanVien.LuongCoBan = entity.LuongCoBan;
                nhanVien.PhuCapTrachNhiem = entity.PhuCapTrachNhiem;
                context.SubmitChanges();
                tkHelper.Update(entity.TaiKhoan);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public NhanVienQuanLy GetNhanVienQuanLy(string pMaTK)
        {
            return context.NhanVienQuanLies.FirstOrDefault(nv => nv.MaTK == pMaTK);
        }

        public List<View_NVQL> GetView_NVQL()
        {
            return context.View_NVQLs.Select(ql => ql).ToList();
        }

        public List<NhanVienReportInfo> GetNhanVienReportInfos()
        {
            List<NhanVienReportInfo> result = new List<NhanVienReportInfo>();
            List<NhanVienQuanLy> list = GetData();
            for(int i=0;i<list.Count;i++)
            {
                NhanVienReportInfo info = new NhanVienReportInfo();
                info.STT = (i + 1).ToString();
                info.MaTK = list[i].MaTK;
                info.HoTen = list[i].TaiKhoan.HoTen;
                info.LuongCoBan = list[i].LuongCoBan.ToString();
                info.PhuCapTrachNhiem = list[i].PhuCapTrachNhiem.ToString();
                info.LuongThang = (list[i].LuongCoBan + list[i].PhuCapTrachNhiem).ToString();
                result.Add(info);
            }
            return result;
        }
    }
}
