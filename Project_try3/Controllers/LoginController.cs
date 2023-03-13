using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_try3.Models;
using Project_try3.ViewModels;


namespace Project_try3.Controllers
{
    [LoginCheck]
    public class LoginController : Controller
    {
      Project_V3Entities db = new Project_V3Entities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(VMLoginModel vm)
        {
            string password = hashPw.getHashPwd(vm.Password);
            var result = db.Users.Where(u => u.Account == vm.Account && u.Password == vm.Password).FirstOrDefault();

            if (result == null)
            {

                ViewBag.ErrMessage = "帳號或密碼輸入錯誤，請再重試一次！";
                return View(vm);
            }
            if (!result.Enabled)
            {
                ViewBag.ErrMessage = "帳號異常，請聯絡管理員";
                return View(vm);
            }
            if (result.AuthSN == 1)
            {
                //顯示管理員畫面;
                Session["user"] = result;
                return RedirectToAction("Index");
            }
            //顯示會員畫面(可以做在details)
            Session["user"] = result;
            return RedirectToAction("Index", "Members");
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login");
        }
    }
}