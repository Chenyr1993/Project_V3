﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_V3.Models;

namespace Project_V3.Controllers
{
    public class UsersController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Auth);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult _Create()
        {
            ViewBag.AuthSN = new SelectList(db.Auth, "SN", "AuthName");
            return PartialView();
        }

        // POST: Users/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Users users)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(new Users()
                    {
                        SN = users.SN,
                        Account = users.Account,
                        Password = users.Password,
                        AuthSN = users.AuthSN,
                        CreatedDate = DateTime.Now,
                        Enabled = users.Enabled
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    var getFullMessage = string.Join("; ", entityError);
                    var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                }

                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthSN = new SelectList(db.Auth, "SN", "AuthName", users.AuthSN);
            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult _Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthSN = new SelectList(db.Auth, "SN", "AuthName", users.AuthSN);
            return PartialView(users);
        }

        // POST: Users/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,Account,Password,CreatedDate,AuthSN,Enabled")] Users users,DateTime cd)
        {
            if (ModelState.IsValid)
            {
                users.CreatedDate = cd.Date;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthSN = new SelectList(db.Auth, "SN", "AuthName", users.AuthSN);
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
