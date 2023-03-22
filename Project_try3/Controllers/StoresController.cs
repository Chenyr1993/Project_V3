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
    public class StoresController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: Stores
        public ActionResult Index()
        {
            var stores = db.Stores.Include(s => s.Admin).Include(s => s.Members);
            return View(stores.ToList());
        }

        // GET: Stores/Details/5
        public ActionResult Details(int? id,string GPID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores stores = db.Stores.Find(id);
            if (stores == null)
            {
                return HttpNotFound();
            }
            return View(stores);
        }

        //GP
        public ActionResult DetailsforGP(int? id,string GroupBuyId)
        {
            if (id == null || GroupBuyId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores stores = db.Stores.Find(id);
            if (stores == null)
            {
                return HttpNotFound();
            }
            stores= db.Stores.Find(id, GroupBuyId);
            return View(stores);
        }


        // GET: Stores/Create
        public ActionResult Create()
        {
            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name");
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name");
            return View();
        }

        // POST: Stores/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,AdminSN,MemberSN,Name,Phone,Address,Description,CreatedDate,Status")] Stores stores)
        {
            if (ModelState.IsValid)
            {
                stores.CreatedDate = DateTime.Now;
                db.Stores.Add(stores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name", stores.AdminSN);
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name", stores.MemberSN);
            return View(stores);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores stores = db.Stores.Find(id);
            if (stores == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name", stores.AdminSN);
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name", stores.MemberSN);
            return View(stores);
        }

        // POST: Stores/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,AdminSN,MemberSN,Name,Phone,Address,Description,CreatedDate,Status")] Stores stores, DateTime cd)
        {
            if (ModelState.IsValid)
            {
                stores.CreatedDate = cd.Date;
                db.Entry(stores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name", stores.AdminSN);
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name", stores.MemberSN);
            return View(stores);
        }

        // GET: Stores/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores stores = db.Stores.Find(id);
            if (stores == null)
            {
                return HttpNotFound();
            }
            return View(stores);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stores stores = db.Stores.Find(id);
            db.Stores.Remove(stores);
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
