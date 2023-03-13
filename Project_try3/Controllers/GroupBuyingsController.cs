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
    public class GroupBuyingsController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();
        GetData gd= new GetData();
        SetData sd= new SetData();
        // GET: GroupBuyings
        public ActionResult Index()
        {
            var groupBuying = db.GroupBuying.Include(g => g.Delivery).Include(g => g.Members).Include(g => g.PayType).Include(g => g.Stores);
            return View(groupBuying.ToList());
        }

        // GET: GroupBuyings/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupBuying groupBuying = db.GroupBuying.Find(id);
            if (groupBuying == null)
            {
                return HttpNotFound();
            }
            return View(groupBuying);
        }
        public ActionResult Create()
        {
            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method");
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name");
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method");
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name");
            return View();
        }

        // POST: GroupBuyings/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupBuying groupBuying,string id)
        {
            if (ModelState.IsValid)
            {
     
                //db.GroupBuying.Add(groupBuying);
                //db.SaveChanges();
                string sql = "Insert into GroupBuing(ID,StoreSN,CreatedDate,Startdate,CreatedPerson,Title,Description,RequireDate,CloseDate,ShipAddress,DeliverySN,Continued,PaySN)" +
                    "values(dbo.getGroupBuyID(),@StoreSN,@CreatedDate,@Startdate,@CreatedPerson,@Title,@Description,@RequireDate,@CloseDate,@ShipAddress,@DeliverySN,@Continued,@PaySN)";
                groupBuying.CreatedDate = DateTime.Now;
                groupBuying.CreatedPerson=db.Members.Find(id).SN;

                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("StoreSN",groupBuying.StoreSN),
                    new SqlParameter("Created",groupBuying.CreatedDate),
                    new SqlParameter("Startdate",groupBuying.Startdate),
                    new SqlParameter("CreatedPerson",groupBuying.CreatedPerson),
                    new SqlParameter("Title",groupBuying.Title),
                    new SqlParameter("Description",groupBuying.Description),
                    new SqlParameter("RequireDate",groupBuying.RequireDate),
                    new SqlParameter("CloseDate",groupBuying.CloseDate),
                    new SqlParameter("ShipAddress",groupBuying.ShipAddress),
                    new SqlParameter("DeliverySN",groupBuying.DeliverySN),
                    new SqlParameter("Continued",groupBuying.Continued),
                    new SqlParameter("PaySN",groupBuying.PaySN)
                };
                sd.executeSql(sql,list);

                return RedirectToAction("Index");
            }

            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method", groupBuying.DeliverySN);
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name", groupBuying.CreatedPerson);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", groupBuying.PaySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", groupBuying.StoreSN);
            return View(groupBuying);
        }
        // POST: GroupBuyings/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingalCreate([Bind(Include = "ID,StoreSN,CreatedDate,Startdate,CreatedPerson,Title,Description,RequireDate,CloseDate,ShipAddress,DeliverySN,LimitMoney,LimitNumber,Continued,PaySN")] GroupBuying groupBuying)
        {
            if (ModelState.IsValid)
            {
                db.GroupBuying.Add(groupBuying);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method", groupBuying.DeliverySN);
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name", groupBuying.CreatedPerson);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", groupBuying.PaySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", groupBuying.StoreSN);
            return View(groupBuying);
        }
        // GET: GroupBuyings/Create
        public ActionResult SingalCreate()
        {
            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method");
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name");
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method");
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name");
            return View();
        }


        // GET: GroupBuyings/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupBuying groupBuying = db.GroupBuying.Find(id);
            if (groupBuying == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method", groupBuying.DeliverySN);
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name", groupBuying.CreatedPerson);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", groupBuying.PaySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", groupBuying.StoreSN);
            return View(groupBuying);
        }

        // POST: GroupBuyings/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StoreSN,CreatedDate,Startdate,CreatedPerson,Title,Description,RequireDate,CloseDate,ShipAddress,DeliverySN,LimitMoney,LimitNumber,Continued,PaySN")] GroupBuying groupBuying)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupBuying).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method", groupBuying.DeliverySN);
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name", groupBuying.CreatedPerson);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", groupBuying.PaySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", groupBuying.StoreSN);
            return View(groupBuying);
        }

        // GET: GroupBuyings/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupBuying groupBuying = db.GroupBuying.Find(id);
            if (groupBuying == null)
            {
                return HttpNotFound();
            }
            return View(groupBuying);
        }

        // POST: GroupBuyings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GroupBuying groupBuying = db.GroupBuying.Find(id);
            db.GroupBuying.Remove(groupBuying);
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
