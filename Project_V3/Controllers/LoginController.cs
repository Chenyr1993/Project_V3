using Project_V3.Models;
using Project_V3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_V3.Controllers
{
    public class LoginController : Controller
    {
        Project_V3Entities db=new Project_V3Entities();
        // GET: Login
        public ActionResult Login(VMLoginModel vM)
        {
            
            return View();
        }
    }
}