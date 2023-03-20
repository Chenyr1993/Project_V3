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
    public class ManagesController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: Manages
        public ActionResult Index()
        {
            var manage = db.Manage.Include(m => m.Admin).Include(m => m.Members).Include(m => m.ProblemType);
            return View(manage.ToList());
        }

        // GET: Manages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manage manage = db.Manage.Find(id);
            if (manage == null)
            {
                return HttpNotFound();
            }
            return View(manage);
        }

        // GET: Manages/Create
        public ActionResult Create()
        {
            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name");
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name");
            ViewBag.TypeSN = new SelectList(db.ProblemType, "SN", "Type");
            return View();
        }

        // POST: Manages/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,TypeSN,Details,CreatedDate,DealDate,AdminSN,MemberSN")] Manage manage)
        {
            if (ModelState.IsValid)
            {
                db.Manage.Add(manage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name", manage.AdminSN);
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name", manage.MemberSN);
            ViewBag.TypeSN = new SelectList(db.ProblemType, "SN", "Type", manage.TypeSN);
            return View(manage);
        }

        // GET: Manages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manage manage = db.Manage.Find(id);
            if (manage == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name", manage.AdminSN);
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name", manage.MemberSN);
            ViewBag.TypeSN = new SelectList(db.ProblemType, "SN", "Type", manage.TypeSN);
            return View(manage);
        }

        // POST: Manages/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,TypeSN,Details,CreatedDate,DealDate,AdminSN,MemberSN")] Manage manage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminSN = new SelectList(db.Admin, "SN", "Name", manage.AdminSN);
            ViewBag.MemberSN = new SelectList(db.Members, "SN", "Name", manage.MemberSN);
            ViewBag.TypeSN = new SelectList(db.ProblemType, "SN", "Type", manage.TypeSN);
            return View(manage);
        }

        // GET: Manages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manage manage = db.Manage.Find(id);
            if (manage == null)
            {
                return HttpNotFound();
            }
            return View(manage);
        }

        // POST: Manages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manage manage = db.Manage.Find(id);
            db.Manage.Remove(manage);
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
