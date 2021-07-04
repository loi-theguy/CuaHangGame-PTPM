using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    class BLDAL_NhanVienQuanLy : DataHelper<NhanVienQuanLy>
    {
        BLDAL_TaiKhoan tkHelper = new BLDAL_TaiKhoan();
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
            return context.NhanVienQuanLies.Select(nv=>nv).ToList();
        }

        public override bool Insert(NhanVienQuanLy entity)
        {
            try
            {
                tkHelper.Insert(entity.TaiKhoan);
                context.NhanVienQuanLies.InsertOnSubmit(entity);
                context.SubmitChanges();
            }
            catch
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

        protected override string GenerateID()
        {
            string type = "QL";
            int max = -1;
            foreach (TaiKhoan taiKhoan in context.TaiKhoans.Select(tk => tk))
            {
                if (taiKhoan.MaTK.Substring(0, 2) != "QL") continue;
                int temp = int.Parse(taiKhoan.MaTK.Substring(2));
                if (temp > max) max = temp;
            }
            max += 1;
            string id = max.ToString();
            while (id.Length < 8)
            {
                id = "0" + id;
            }
            return type + id;
        }
    }
}
