using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_try3.Models;

namespace Project_try3.Controllers
{
    public class OrderdetailsController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: Orderdetails
        public ActionResult Index()
        {
            var orderdetails = db.Orderdetails.Include(o => o.Orders).Include(o => o.Products);
            return View(orderdetails.ToList());
        }

        // GET: Orderdetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderdetails orderdetails = db.Orderdetails.Find(id);
            if (orderdetails == null)
            {
                return HttpNotFound();
            }
            return View(orderdetails);
        }

        // GET: Orderdetails/Create
        public ActionResult Create()
        {
            //var orderdetails = db.Orderdetails.Where(m => m.Orders.CustomerSN == ((Users)Session["user"]).Members.FirstOrDefault().SN).Include(o => o.Orders).Include(o => o.Products);
            //return View(orderdetails.ToList());
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "GroupBuyingID");
            ViewBag.ProductSN = new SelectList(db.Products, "SN", "ProductName");
            return View();
        }

        // POST: Orderdetails/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orderdetails orderdetails)
        {
            if (ModelState.IsValid)
            {

                db.Orderdetails.Add(orderdetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "ID", "GroupBuyingID", orderdetails.OrderID);
            ViewBag.ProductSN = new SelectList(db.Products, "SN", "ProductName", orderdetails.ProductSN);
            return View(orderdetails);
        }

        // GET: Orderdetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderdetails orderdetails = db.Orderdetails.Find(id);
            if (orderdetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "GroupBuyingID", orderdetails.OrderID);
            ViewBag.ProductSN = new SelectList(db.Products, "SN", "ProductName", orderdetails.ProductSN);
            return View(orderdetails);
        }

        // POST: Orderdetails/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,OrderID,ProductSN,Quanity,UnitPrice,Discount,Subtotal")] Orderdetails orderdetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderdetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "GroupBuyingID", orderdetails.OrderID);
            ViewBag.ProductSN = new SelectList(db.Products, "SN", "ProductName", orderdetails.ProductSN);
            return View(orderdetails);
        }

        // GET: Orderdetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderdetails orderdetails = db.Orderdetails.Find(id);
            if (orderdetails == null)
            {
                return HttpNotFound();
            }
            return View(orderdetails);
        }

        // POST: Orderdetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orderdetails orderdetails = db.Orderdetails.Find(id);
            db.Orderdetails.Remove(orderdetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
