using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    class BLDAL_Game:DataHelper<Game>
    {
        public override List<Game> GetData()
        {
            return context.Games.Select(g => g).ToList();
        }

        protected override string GenerateID()
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
                pGame.MaGame = GenerateID();
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
                game.Trailer = entity.Trailer;
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
    }
}
