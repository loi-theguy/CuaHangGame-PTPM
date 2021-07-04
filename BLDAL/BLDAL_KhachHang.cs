using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    class BLDAL_KhachHang : BLDAL_TaiKhoan
    {
        public override int Delete(string pMaTK)
        {
            return base.Delete(pMaTK);
        }

        public override List<TaiKhoan> GetData()
        {
            return base.GetData();
        }

        public override bool Insert(TaiKhoan pTaiKhoan)
        {
            pTaiKhoan.MaTK = GenerateID();
            return base.Insert(pTaiKhoan);
        }

        public override bool Update(TaiKhoan pTaiKhoan)
        {
            return base.Update(pTaiKhoan);
        }

        protected override string GenerateID()
        {
            return base.GenerateID();
        }
    }
}
