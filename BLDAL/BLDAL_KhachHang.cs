using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_KhachHang : BLDAL_TaiKhoan
    {
        //nhom khach hang
        private const string NHOM = "NQ00000002";
        public override int Delete(string pMaTK)
        {
            return base.Delete(pMaTK);
        }

        public override List<TaiKhoan> GetData()
        {
            return context.TaiKhoans.Select(tk=>tk).Where(tk=>tk.MaNhom==NHOM).ToList();
        }

        public override bool Insert(TaiKhoan pTaiKhoan)
        {
            pTaiKhoan.MaNhom = NHOM;
            if(string.IsNullOrEmpty(pTaiKhoan.MaTK)) pTaiKhoan.MaTK = GenerateID();
            return base.Insert(pTaiKhoan);
        }

        public override bool Update(TaiKhoan pTaiKhoan)
        {
            return base.Update(pTaiKhoan);
        }

        public override string GenerateID()
        {
            return base.GenerateID();
        }

        public TaiKhoan GetKhachHang(string pMaTK)
        {
            return context.TaiKhoans.FirstOrDefault(tk => tk.MaTK == pMaTK);
        }
    }
}
