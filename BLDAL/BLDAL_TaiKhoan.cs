using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_TaiKhoan:DataHelper<TaiKhoan>
    {
        public override List<TaiKhoan> GetData()
        {
            return context.TaiKhoans.Select(tk => tk).ToList();
        }

        protected override string GenerateID()
        {
            string type = "KH";
            int max = -1;
            foreach (TaiKhoan taiKhoan in context.TaiKhoans.Select(tk => tk))
            {
                if (taiKhoan.MaTK.Substring(0, 2) != "KH") continue;
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

        public override bool Insert(TaiKhoan pTaiKhoan)
        {
            try
            {
                context.TaiKhoans.InsertOnSubmit(pTaiKhoan);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }
        public override bool Update(TaiKhoan pTaiKhoan)
        {
            NhanVienHoTro nv = new NhanVienHoTro();
            try
            {
                TaiKhoan temp = context.TaiKhoans.FirstOrDefault(tk => tk.MaTK == pTaiKhoan.MaTK);
                temp.HoTen = pTaiKhoan.HoTen;
                temp.MaNhom = pTaiKhoan.MaNhom;
                temp.NgaySinh = pTaiKhoan.NgaySinh;
                temp.Pass = pTaiKhoan.Pass;
                temp.SoDienThoai = pTaiKhoan.SoDienThoai;
                temp.Username = pTaiKhoan.Username;
                temp.Email = pTaiKhoan.Email;
                temp.MaNhom = pTaiKhoan.MaNhom;
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }
        public override int Delete(string pMaTK)
        {
            try
            {
                TaiKhoan temp = context.TaiKhoans.FirstOrDefault(tk => tk.MaTK == pMaTK);
                if (temp == null) return NONEXISTENT;
                context.TaiKhoans.DeleteOnSubmit(temp);
                context.SubmitChanges();
            }
            catch {
                return DEPENDED;
            }
            return SUCCESS;
        }
    }
}
