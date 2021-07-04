using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    class BLDAL_TheLoai : DataHelper<TheLoai>
    {
        public override int Delete(string pID)
        {
            try
            {
                TheLoai theLoai = context.TheLoais.FirstOrDefault(tl => tl.MaTL == pID);
                if (theLoai == null) return NONEXISTENT;
                context.TheLoais.DeleteOnSubmit(theLoai);
                context.SubmitChanges();
            }
            catch {
                return DEPENDED;
            }
            return SUCCESS;
        }

        public override List<TheLoai> GetData()
        {
            return context.TheLoais.Select(tl=>tl).ToList();
        }

        public override bool Insert(TheLoai entity)
        {
            try
            {
                entity.MaTL = GenerateID();
                context.TheLoais.InsertOnSubmit(entity);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public override bool Update(TheLoai entity)
        {
            try
            {
                TheLoai theLoai = context.TheLoais.FirstOrDefault(tl => tl.MaTL == entity.MaTL);
                theLoai.TenTL = entity.TenTL;
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        protected override string GenerateID()
        {
            string type = "TL";
            int max = -1;
            foreach (TheLoai theLoai in context.TheLoais.Select(tl => tl))
            {
                int temp = int.Parse(theLoai.MaTL.Substring(2));
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
