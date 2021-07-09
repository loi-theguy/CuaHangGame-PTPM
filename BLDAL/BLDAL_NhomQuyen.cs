using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_NhomQuyen : DataHelper<NhomQuyen>
    {
        public override int Delete(string pID)
        {
            try
            {
                NhomQuyen nhom = context.NhomQuyens.FirstOrDefault(nq => nq.MaNhom == pID);
                if (nhom == null) return NONEXISTENT;
                context.NhomQuyens.DeleteOnSubmit(nhom);
                context.SubmitChanges();
            }
            catch {
                return DEPENDED;
            }
            return SUCCESS;
        }

        public override List<NhomQuyen> GetData()
        {
            return context.NhomQuyens.Select(nq=>nq).ToList();
        }

        public override bool Insert(NhomQuyen entity)
        {
            try
            {
                if(string.IsNullOrEmpty(entity.MaNhom)) entity.MaNhom = GenerateID();
                context.NhomQuyens.InsertOnSubmit(entity);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public override bool Update(NhomQuyen entity)
        {
            try
            {
                NhomQuyen nhomQuyen = context.NhomQuyens.FirstOrDefault(nq => nq.MaNhom == entity.MaNhom);
                nhomQuyen.TenNhom = entity.MaNhom;
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public override string GenerateID()
        {
            string type = "NQ";
            int max = -1;
            foreach (NhomQuyen nhomQuyen in context.NhomQuyens.Select(nq => nq))
            {
             
                int temp = int.Parse(nhomQuyen.MaNhom.Substring(2));
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

        public List<CTNhomQuyen> GetDataCTNhomQuyen(string pMaNhom)
        {
            return context.CTNhomQuyens.Select(ct => ct).Where(ct=>ct.MaNhom==pMaNhom).ToList();
        }

        public bool DeleteCTNhomQuyen(string pMaNhom, string pMaQuyen)
        {
            try
            {
                CTNhomQuyen chiTiet = context.CTNhomQuyens.FirstOrDefault(ct => ct.MaNhom == pMaNhom && ct.MaQuyen == pMaQuyen);
                context.CTNhomQuyens.DeleteOnSubmit(chiTiet);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }
        public bool InsertCTNhomQuyen(CTNhomQuyen pChiTiet)
        {
            try
            {
                context.CTNhomQuyens.InsertOnSubmit(pChiTiet);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }
    }
}
