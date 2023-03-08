using Project_try3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_try3.Controllers
{
    public class HomeController : Controller
    {
        Project_V3Entities db=new Project_V3Entities();
        // GET: Home
        public ActionResult Index()
        {
            var stores=db.Stores.Where(p=>p.Status=="正常營業中").ToList();
            
            return View(stores);
        }
        public ActionResult CategeoryList() 
        {
            var categeory=db.Category.ToList();
            return View(categeory);
        }
    }

}