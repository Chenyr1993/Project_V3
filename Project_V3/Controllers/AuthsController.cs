using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_V3.Models;

namespace Project_V3.Controllers
{
    public class AuthsController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: Auths
        public ActionResult Index()
        {
            return View(db.Auth.ToList());
        }

        // GET: Auths/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auth auth = db.Auth.Find(id);
            if (auth == null)
            {
                return HttpNotFound();
            }
            return View(auth);
        }

        // GET: Auths/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auths/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,AuthName")] Auth auth)
        {
            if (ModelState.IsValid)
            {
                db.Auth.Add(auth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auth);
        }

        // GET: Auths/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auth auth = db.Auth.Find(id);
            if (auth == null)
            {
                return HttpNotFound();
            }
            return View(auth);
        }

        // POST: Auths/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,AuthName")] Auth auth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auth);
        }

        // GET: Auths/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auth auth = db.Auth.Find(id);
            if (auth == null)
            {
                return HttpNotFound();
            }
            return View(auth);
        }

        // POST: Auths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Auth auth = db.Auth.Find(id);
            db.Auth.Remove(auth);
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
