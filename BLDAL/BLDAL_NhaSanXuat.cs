using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    class BLDAL_NhaSanXuat : DataHelper<NhaSanXuat>
    {
        public override int Delete(string pID)
        {
            try
            {
                NhaSanXuat nhaSanXuat = context.NhaSanXuats.FirstOrDefault(nsx => nsx.MaNSX == pID);
                if (nhaSanXuat == null) return NONEXISTENT;
                context.NhaSanXuats.DeleteOnSubmit(nhaSanXuat);
                context.SubmitChanges();
            }
            catch {
                return DEPENDED;
            }
            return SUCCESS;
        }

        public override List<NhaSanXuat> GetData()
        {
            return context.NhaSanXuats.Select(nsx=>nsx).ToList();
        }

        public override bool Insert(NhaSanXuat entity)
        {
            try
            {
                entity.MaNSX = GenerateID();
                context.NhaSanXuats.InsertOnSubmit(entity);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public override bool Update(NhaSanXuat entity)
        {
            try
            {
                NhaSanXuat nhaSanXuat = context.NhaSanXuats.FirstOrDefault(nsx => nsx.MaNSX == entity.MaNSX);
                nhaSanXuat.TenNSX = entity.TenNSX;
                nhaSanXuat.SoDienThoai = entity.SoDienThoai;
                nhaSanXuat.Email = entity.Email;
                nhaSanXuat.DiaChi = entity.DiaChi;
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        protected override string GenerateID()
        {
            string type = "NSX";
            int max = -1;
            foreach (NhaSanXuat nhaSanXuat in context.NhaSanXuats.Select(nsx => nsx))
            {
                int temp = int.Parse(nhaSanXuat.MaNSX.Substring(3));
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
    }
}
