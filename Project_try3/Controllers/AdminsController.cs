using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_try3.Models;

namespace Project_try3.Controllers
{
    public class AdminsController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();
        SetData sd = new SetData();
        // GET: Admins
        public ActionResult Index()
        {
            var admin = db.Admin.Include(a => a.Users);
            return View(admin.ToList());
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult _Create()
        {

            ViewBag.UserSN = new SelectList(db.Users, "SN", "Account");
            return PartialView();
        }

        // POST: Admins/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            if (ModelState.IsValid)
            {
                //Users user = admin.Users;

                string sql = "insert into Users ( Account,Password,AuthSN,Enabled ) " +
                    "values(@Account,@Password,@AuthSN,@Enabled)";
                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("Account",admin.Users.Account),
                    new SqlParameter("Password", admin.Users.Password),
                    
                    new SqlParameter("AuthSN", 1),
                    new SqlParameter("Enabled", 1)
                    
                };
                sd.executeSql(sql, list);
                admin.UserSN = db.Users.Where(a => a.Account == admin.Users.Account).FirstOrDefault().SN;
                string sql2 = "insert into Admin ( UserSN,Name,Title,Phone,Email ) " +
                   "values(@UserSN,@Name,@Title,@Phone,@Email)";
                List<SqlParameter> list2 = new List<SqlParameter>
                {
                    new SqlParameter("UserSN",admin.UserSN),
                    new SqlParameter("Name", admin.Name),
                    new SqlParameter("Title", admin.Title),
                    new SqlParameter("Phone", admin.Phone),
                    new SqlParameter("Email", admin.Email)
                                   
                };
                sd.executeSql(sql2, list2);
                //分存兩張表時，只要做了一個ado.net,另一個也要做ado
                //db.Admin.Add(admin);
                //db.SaveChanges();
                //ViewBag.UserSN = admin.UserSN;

                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult _Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserSN = new SelectList(db.Users, "SN", "Account", admin.UserSN);
            return PartialView(admin);
        }

        // POST: Admins/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,UserSN,Name,Title,Phone,Extension,Email")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserSN = new SelectList(db.Users, "SN", "Account", admin.UserSN);
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            Admin admin = db.Admin.Find(id);
            db.Admin.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: Admins/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Admin admin = db.Admin.Find(id);
        //    db.Admin.Remove(admin);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
