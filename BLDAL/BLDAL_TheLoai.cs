using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_TheLoai : DataHelper<TheLoai>
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
                if(string.IsNullOrEmpty(entity.MaTL)) entity.MaTL = GenerateID();
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

        public override string GenerateID()
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

        public List<Game_TheLoai> GetDataGame_TheLoais(string pMaGame)
        {
            return context.Game_TheLoais.Select(tl => tl).Where(tl => tl.MaGame == pMaGame).ToList();
        }

        public bool InsertGame_TheLoai(string pMaGame, string pMaTL)
        {
            try
            {
                Game_TheLoai gtl = new Game_TheLoai();
                gtl.MaGame = pMaGame;
                gtl.MaTL = pMaTL;
                context.Game_TheLoais.InsertOnSubmit(gtl);
                context.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteGame_TheLoai(string pMaGame, string pMaTL)
        {
            try
            {
                Game_TheLoai gtl = context.Game_TheLoais.FirstOrDefault(tl => tl.MaGame == pMaGame && tl.MaTL == pMaTL);
                context.Game_TheLoais.DeleteOnSubmit(gtl);
                return true;
            }
            catch {
                return false;
            }
        }
        public TheLoai GetTheLoai(string pMaTL)
        {
            return context.TheLoais.FirstOrDefault(tl => tl.MaTL == pMaTL);
        }
    }
}
