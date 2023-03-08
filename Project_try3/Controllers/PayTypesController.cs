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
    public class PayTypesController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: PayTypes
        public ActionResult Index()
        {
            return View(db.PayType.ToList());
        }

        // GET: PayTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayType payType = db.PayType.Find(id);
            if (payType == null)
            {
                return HttpNotFound();
            }
            return View(payType);
        }

        // GET: PayTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayTypes/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,Method")] PayType payType)
        {
            if (ModelState.IsValid)
            {
                db.PayType.Add(payType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payType);
        }

        // GET: PayTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayType payType = db.PayType.Find(id);
            if (payType == null)
            {
                return HttpNotFound();
            }
            return View(payType);
        }

        // POST: PayTypes/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,Method")] PayType payType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payType);
        }

        // GET: PayTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayType payType = db.PayType.Find(id);
            if (payType == null)
            {
                return HttpNotFound();
            }
            return View(payType);
        }

        // POST: PayTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PayType payType = db.PayType.Find(id);
            db.PayType.Remove(payType);
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
