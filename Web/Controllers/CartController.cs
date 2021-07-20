using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLDAL;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private BLDAL_Game gameHelper = new BLDAL_Game();
        private BLDAL_HoaDon hdHelper = new BLDAL_HoaDon();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ItemCount()
        {
            int t = 0;
            if (Session["cart"] != null)
                t = ((List<Game>)Session["cart"]).Count;
            return PartialView(t);
        }

        public ActionResult AddItem(string id)
        {
            List<Game> list = Session["cart"] as List<Game>;
            if (list == null)
            {
                list = new List<Game>();
            }
            foreach(Game g in list)
                if(g.MaGame==id) return RedirectToAction("Index", "Home");
            Game game = gameHelper.GetGame(id);
            list.Add(game);
            Session["cart"] = list;
            return RedirectToAction("Index","Home");
        }

        public ActionResult ItemList()
        {
            List<Game> list = Session["cart"] as List<Game>;
            return View(list);
        }

        public ActionResult DeleteItem(string id)
        {
            List<Game> list = Session["cart"] as List<Game>;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].MaGame == id)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
            Session["cart"] = list;
            return RedirectToAction("ItemList");
        }
        public ActionResult Purchase()
        {
            List<Game> list = Session["cart"] as List<Game>;
            List<Game> gameDaMua;
            List<Game> finalList = new List<Game>();
            TaiKhoan tk = Session["kh"] as TaiKhoan;
            if (tk == null)
            {
                ViewBag.NullUser = true;
                return RedirectToAction("ItemList"); 
            }
            gameDaMua = gameHelper.GetDataGameDaMua(tk.MaTK);
            for (int i = 0; i < list.Count; i++)
            {
                bool check = true;
                for (int j = 0; j < gameDaMua.Count; j++)
                    if (list[i].MaGame == gameDaMua[j].MaGame)
                    {
                        check = false;
                        break;
                    }
                if (check) finalList.Add(list[i]);
            }
            if (finalList != null && finalList.Count > 0)
            {
                HoaDon hoaDon = new HoaDon();
                hoaDon.MaTK = tk.MaTK;
                hoaDon.NgayLap = DateTime.Now;
                hdHelper.Insert(hoaDon);
                foreach (Game game in finalList)
                {
                    hdHelper.InsertCTHoaDon(hoaDon.MaHD, game.MaGame);
                }
            }
            Session["cart"] = null;
            return RedirectToAction("ItemList");
        }
    }
}