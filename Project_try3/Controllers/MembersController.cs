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
    public class MembersController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();
        SetData sd= new SetData();
        // GET: Members
        public ActionResult Index()
        {
            int id = ((Users)Session["user"]).Members.FirstOrDefault().SN;
            var members = db.Members.Find(id);
            return View(members);
        }
        public ActionResult Manager()
        {
            //給管理員的畫面
            var members = db.Members.Include(m => m.Users).ToList();

            return View(members);
        }
        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.UserSN = new SelectList(db.Users, "SN", "Account");
            return View();
        }

        // POST: Members/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Members members)
        {
            if (ModelState.IsValid)
            {
                string sql = "insert into Users ( Account,Password,AuthSN,Enabled ) " +
                     "values(@Account,@Password,@AuthSN,@Enabled)";
                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("Account",members.Users.Account),
                    new SqlParameter("Password", members.Users.Password),
                    new SqlParameter("AuthSN", 2),
                    new SqlParameter("Enabled", 1)
                };
                sd.executeSql(sql, list);
                members.UserSN = db.Users.Where(a => a.Account == members.Users.Account).FirstOrDefault().SN;
                string sql2 = "insert into Members ( UserSN,Name,Phone,Email,Address ) " +
                   "values(@UserSN,@Name,@Phone,@Email,@Address)";
                List<SqlParameter> list2 = new List<SqlParameter>
                {
                    new SqlParameter("UserSN",members.UserSN),
                    new SqlParameter("Name", members.Name),
                    new SqlParameter("Phone", members.Phone),
                    new SqlParameter("Email", members.Email),
                    new SqlParameter("Address", members.Address),

                };
                sd.executeSql(sql2, list2);

                return RedirectToAction("Index");
            }
            return View(members);
        }
  
       
        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserSN = new SelectList(db.Users, "SN", "Account", members.UserSN);
            return View(members);
        }

        // POST: Members/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,UserSN,Name,Phone,Email,Address")] Members members)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(members).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserSN = new SelectList(db.Users, "SN", "Account", members.UserSN);
            return View(members);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members members = db.Members.Find(id);
            db.Members.Remove(members);
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
