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
    public class OrdersController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.GroupBuying).Include(o => o.Members).Include(o => o.PayType);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title");
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name");
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method");
            return View();
        }

        // POST: Orders/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreatedDate,PaySN,Payed,CustomerSN,GroupBuyingID")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title", orders.GroupBuyingID);
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name", orders.CustomerSN);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", orders.PaySN);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title", orders.GroupBuyingID);
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name", orders.CustomerSN);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", orders.PaySN);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreatedDate,PaySN,Payed,CustomerSN,GroupBuyingID")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title", orders.GroupBuyingID);
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name", orders.CustomerSN);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", orders.PaySN);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
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
