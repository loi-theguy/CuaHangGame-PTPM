using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    class BLDAL_NhanVienHoTro : DataHelper<NhanVienHoTro>
    {
        BLDAL_TaiKhoan tkHelper=new BLDAL_TaiKhoan();
        public override int Delete(string pID)
        {
            try
            {
                NhanVienHoTro nhanVien = context.NhanVienHoTros.FirstOrDefault(nv => nv.MaTK == pID);
                if (nhanVien == null) return NONEXISTENT;
                context.NhanVienHoTros.DeleteOnSubmit(nhanVien);
                context.SubmitChanges();
                tkHelper.Delete(pID);
            }
            catch {
                return DEPENDED;
            }
            return SUCCESS;
        }

        public override List<NhanVienHoTro> GetData()
        {
            return context.NhanVienHoTros.Select(nv=>nv).ToList();
        }

        public override bool Insert(NhanVienHoTro entity)
        {
            try
            {
                tkHelper.Insert(entity.TaiKhoan);
                context.NhanVienHoTros.InsertOnSubmit(entity);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public override bool Update(NhanVienHoTro entity)
        {
            try
            {
                NhanVienHoTro nhanVien = context.NhanVienHoTros.FirstOrDefault(nv => nv.MaTK == entity.MaTK);
                nhanVien.DonGiaGio = entity.DonGiaGio;
                context.SubmitChanges();
                tkHelper.Update(entity.TaiKhoan);
            }
            catch {
                return false;
            }
            return true;
        }

        protected override string GenerateID()
        {
            string type = "HT";
            int max = -1;
            foreach (TaiKhoan taiKhoan in context.TaiKhoans.Select(tk => tk))
            {
                if (taiKhoan.MaTK.Substring(0, 2) != "HT") continue;
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
        public bool InsertSoGioLam(string pMaTK, DateTime start, DateTime end)
        {
            try
            {
                CTGioLam gioLam = new CTGioLam();
                gioLam.MaTK = pMaTK;
                gioLam.ThoiGianBatDau = start;
                gioLam.ThoiGianKetThuc = end;
                TimeSpan duration = end - start;
                gioLam.SoGio = duration.TotalHours;
                context.CTGioLams.InsertOnSubmit(gioLam);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }
        public List<CTGioLam> GetSoGioLam(string pMaTK)
        {
            return context.CTGioLams.Select(gl => gl).Where(gl => gl.MaTK == pMaTK).ToList();
        }

        public bool DeleteSoGioLam(string pMaTK, DateTime pThoiGianBatDau)
        {
            try {
                CTGioLam gioLam = context.CTGioLams.FirstOrDefault(gl => gl.MaTK == pMaTK && pThoiGianBatDau.Equals(gl.ThoiGianBatDau));
                context.CTGioLams.DeleteOnSubmit(gioLam);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }
    }
}
