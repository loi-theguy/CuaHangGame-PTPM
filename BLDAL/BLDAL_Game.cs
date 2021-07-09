using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_Game:DataHelper<Game>
    {
        public override List<Game> GetData()
        {
            return context.Games.Select(g => g).ToList();
        }

        public override string GenerateID()
        {
            string type= "GA";
            int max = -1;
            foreach (Game game in context.Games.Select(g => g))
            {
                if (game.MaGame.Substring(0, 2) != "GA") continue;
                int temp = int.Parse(game.MaGame.Substring(2));
                if (temp > max) max = temp;
            }
            max += 1;
            string id = max.ToString();
            while (id.Length < 8)
            {
                id = "0" + id;
            }
            return type+id;
        }

        public override bool Insert(Game pGame)
        {
            try
            {
                if(string.IsNullOrEmpty(pGame.MaGame)) pGame.MaGame = GenerateID();
                context.Games.InsertOnSubmit(pGame);
                context.SubmitChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override int Delete(string pMaGame)
        {
            try
            {
                List<Game_TheLoai> gtls = GetDataGameTheLoai(pMaGame);
                foreach (Game_TheLoai gtl in gtls)
                {
                    DeleteGameTheLoai(gtl.MaGame, gtl.MaTL);
                }    
                Game game = context.Games.FirstOrDefault(g => g.MaGame == pMaGame);
                if (game == null) return NONEXISTENT;
                context.Games.DeleteOnSubmit(game);
                context.SubmitChanges();
            }
            catch {
                return DEPENDED;
            }
            return SUCCESS;

        }

        public override bool Update(Game entity)
        {
            try
            {
                Game game = context.Games.FirstOrDefault(g => g.MaGame == entity.MaGame);
                game.TenGame = entity.TenGame;
                game.MoTa = entity.MoTa;
                game.HinhDaiDien = entity.HinhDaiDien;
                game.MaNSX = entity.MaNSX;
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public List<Game_TheLoai> GetDataGameTheLoai(string pMaGame)
        {
            return context.Game_TheLoais.Select(gtl => gtl).Where(gtl => gtl.MaGame == pMaGame).ToList();
        }

        public bool DeleteGameTheLoai(string pMaGame, string pMaTL)
        {
            try
            {
                Game_TheLoai gtl = context.Game_TheLoais.FirstOrDefault(g => g.MaGame == pMaGame && g.MaTL == pMaTL);
                context.Game_TheLoais.DeleteOnSubmit(gtl);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }
        public bool InsertGameTheLoai(Game_TheLoai pGTL)
        {
            try
            {
                context.Game_TheLoais.InsertOnSubmit(pGTL);
                context.SubmitChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public Game GetGame(string pMaGame)
        {
            return context.Games.FirstOrDefault(g => g.MaGame == pMaGame);
        }
        public List<Game> GetDataGameDaMua(string pMaTK)
        {
            BLDAL_HoaDon hdHelper = new BLDAL_HoaDon();
            List<HoaDon> hoaDons = hdHelper.GetData(pMaTK);
            List<Game> games = new List<Game>();
            foreach (HoaDon hd in hoaDons)
            {
                List<CTHoaDon> chiTiets=hdHelper.GetDataCTHoaDon(hd.MaHD);
                foreach (CTHoaDon ct in chiTiets)
                {
                    games.Add(GetGame(ct.MaGame));
                }
            }
            return games; 
        }
        public List<Game> Search(string pKeyWord)
        {
            return context
                .Games
                .Select(g => g)
                .Where(g => g.TenGame.Contains(pKeyWord) || g.MoTa.Contains(pKeyWord))
                .ToList();
        }

        public List<View_Game> GetView_Games()
        {
            return context.View_Games.ToList();
        }

        public List<TheLoai> GetTheLoais(string pMaGame)
        {
            BLDAL_TheLoai helper = new BLDAL_TheLoai();
            List<TheLoai> result = new List<TheLoai>();
            List<Game_TheLoai> list = context.Game_TheLoais.Where(gtl => gtl.MaGame == pMaGame).ToList();
            foreach (Game_TheLoai gtl in list)
                result.Add(helper.GetTheLoai(gtl.MaTL));
            return result;
        }

        private bool IsContained(TheLoai tl, List<TheLoai> list)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].MaTL == tl.MaTL) return true;
            return false;
        }
        public List<TheLoai> GetTheLoaiBoSungs(string pMaGame)
        {
            List<TheLoai> list = GetTheLoais(pMaGame);
            List<TheLoai> result = new List<TheLoai>();
            foreach (TheLoai tl in context.TheLoais.ToList())
                if (!IsContained(tl,list)) result.Add(tl);
            return result;
        }
    }
}
