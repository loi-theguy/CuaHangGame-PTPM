using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLDAL;

namespace Web.Controllers
{

    public class HomeController : Controller
    {
        private BLDAL_Game gameHelper = new BLDAL_Game();
        private BLDAL_KhachHang khHelper = new BLDAL_KhachHang();
        public ActionResult Index()
        {
            Session["mk_error"] = null;
            List<Game> list = gameHelper.GetData();
            return View(list);
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            string username = collection["username"];
            string password = collection["password"];
            TaiKhoan tk = khHelper.GetTaiKhoan(username);
            if (!khHelper.IsUserValid(username, password) || tk.MaNhom != "NQ00000002")
            {
                object error = "Sai mật khẩu hoặc tài khoản không tồn tại";
                return View(error);
            }
            Session["kh"] = tk;
            return RedirectToAction("Index");
        }

        public ActionResult DangXuat()
        {
            Session["kh"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult GameDaMua()
        {
            if (Session["kh"] == null) return RedirectToAction("Index");
            List<Game> list = gameHelper.GetDataGameDaMua(((TaiKhoan)Session["kh"]).MaTK);
            return View(list);
        }
        public ActionResult ChiTiet(string id)
        {
            Game game = gameHelper.GetGame(id);
            return View(game);
        }

        public ActionResult Search(FormCollection collection)
        {
            if (collection["keyWord"] == null) return View();
            List<Game> games = gameHelper.Search(collection["keyWord"]);
            return View(games);
        }

        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection)
        {
            TaiKhoan tk = new TaiKhoan();
            tk.HoTen = collection["hoTen"];
            tk.NgaySinh = DateTime.Parse(collection["ngaySinh"]);
            tk.SoDienThoai = collection["soDienThoai"];
            tk.Email = collection["email"];
            tk.Username = collection["username"];
            tk.Pass = khHelper.ComputeHash(collection["password"]);
            if (collection["password"] != collection["re-password"] || !khHelper.IsUsernameValid(collection["username"]))
                return RedirectToAction("DangKy");
            khHelper.Insert(tk);
            return RedirectToAction("DangNhap");
        }
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection collection)
        {
            Session["mk_error"] = null;
            string oldp = collection["oldPass"];
            string newp = collection["newPass"];
            string renewp = collection["reNewPass"];
            TaiKhoan tk = (TaiKhoan)Session["kh"];
            if (renewp != newp || !khHelper.IsUserValid(tk.Username,oldp))
            {
                Session["mk_error"] = "Sai mật khẩu, vui lòng kiểm tra lại";
                return RedirectToAction("DoiMatKhau");
            }
            TaiKhoan tk2 = khHelper.GetKhachHang(tk.MaTK);
            tk2.Pass = khHelper.ComputeHash(newp);
            khHelper.Update(tk2);
            return RedirectToAction("Index");
        }
    }
}