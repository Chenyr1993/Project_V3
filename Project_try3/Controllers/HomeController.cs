using Project_try3.Models;
using Project_try3.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project_try3.Controllers
{
    public class HomeController : Controller
    {
        Project_V3Entities db = new Project_V3Entities();
        // GET: Home
        public ActionResult Index()
        {
            var stores = db.Stores.Where(p => p.Status == "正常營業中").ToList();

            return View(stores);

        }
        public ActionResult CategeoryList()
        {
            var categeory = db.Category.ToList();
            return View(categeory);
        }


        public ActionResult _Menu(int? id)
        {
            var products = db.Products.Where(p => p.StoreSN == id && p.Discontinued == false).Include(p => p.Category).ToList();

            return PartialView(products);
        }

        public ActionResult MenuforGP(int? storeSN,string GPID)
        {
            ViewBag.GPID = GPID;
            var products = db.Products.Where(p => p.StoreSN == storeSN ).Include(p => p.Category).ToList();
            
            return View(products);
        }

        public ActionResult MyCart(int? storeSN)
        {
            var cart = db.Products.Where(p => p.StoreSN == storeSN).ToList();
            //if (GpID != null) 
            //{
            //    cart = db.Products.Where(c => c.StoreSN == id).Include(c => c.Orderdetails.FirstOrDefault().Orders.GroupBuyingID == GpID).ToList();
            //}
            return View(cart);
        }
        public ActionResult MyCartforGP(int storeSN,string GPID)
        {
            //var i = db.Products.FirstOrDefault().Orderdetails.Where(o => o.Orders.GroupBuying.ID == id).ToList();
            //var cart = db.GroupBuying.Where(g=>g.ID==id).Include(g=>g.StoreSN==storeSN).Include(g=>g.product)
            var products=db.Products.Where(p=>p.StoreSN==storeSN && p.Orderdetails.FirstOrDefault().Orders.GroupBuyingID==GPID);
            return View(products);
        }
    }
}