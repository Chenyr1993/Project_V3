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
    public class ProblemTypesController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: ProblemTypes
        public ActionResult Index()
        {
            return View(db.ProblemType.ToList());
        }

        // GET: ProblemTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProblemType problemType = db.ProblemType.Find(id);
            if (problemType == null)
            {
                return HttpNotFound();
            }
            return View(problemType);
        }

        // GET: ProblemTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProblemTypes/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,Type")] ProblemType problemType)
        {
            if (ModelState.IsValid)
            {
                db.ProblemType.Add(problemType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(problemType);
        }

        // GET: ProblemTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProblemType problemType = db.ProblemType.Find(id);
            if (problemType == null)
            {
                return HttpNotFound();
            }
            return View(problemType);
        }

        // POST: ProblemTypes/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,Type")] ProblemType problemType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(problemType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(problemType);
        }

        // GET: ProblemTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProblemType problemType = db.ProblemType.Find(id);
            if (problemType == null)
            {
                return HttpNotFound();
            }
            return View(problemType);
        }

        // POST: ProblemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProblemType problemType = db.ProblemType.Find(id);
            db.ProblemType.Remove(problemType);
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
