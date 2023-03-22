using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
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
        SetData sd = new SetData();
        GetData gd=new GetData();
        // GET: GroupBuyings
        public ActionResult Index()
        {
            var groupBuying = db.GroupBuying.Include(g => g.Delivery).Include(g => g.Members).Include(g => g.PayType).Include(g => g.Stores);

            return View(groupBuying.ToList());
        }
        public ActionResult Display()
        {
            int id = ((Users)Session["user"]).Members.FirstOrDefault().SN;
            var groupBuying = db.GroupBuying.Include(g => g.Delivery).Where(g => g.Members.SN == id).Include(g => g.PayType).Include(g => g.Stores).OrderByDescending(g=>g.ID);
            
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
        [LoginCheck]
        // GET: GroupBuyings/Create
        public ActionResult Create(int? storeSN)
        {
            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method");
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name");
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method");
            ViewBag.StoreSN = storeSN;
            ViewBag.StoreName = db.Stores.Where(s => s.SN == storeSN).FirstOrDefault().Name;
            

            return View();
        }

        // POST: GroupBuyings/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupBuying groupBuying, int storeSN, DateTime RequireDate, DateTime Startdate, DateTime CloseDate)
         {
            if (ModelState.IsValid)
            {
                string sql = "select dbo.getGroupBuyID() as newID";
                //var gpID = gd.TableQuery(sql).ToString();
                var rs = gd.TableQuery(sql);
                string gpID = "";

                if (rs != null)
                {
                    DataRow drow= rs.Rows[0];
                    gpID=drow["newID"].ToString();

                }

                groupBuying.ID = gpID;
                groupBuying.StoreSN = storeSN;
                groupBuying.CreatedDate = DateTime.Now;
                groupBuying.Startdate = Startdate;
                groupBuying.RequireDate = RequireDate;
                groupBuying.CloseDate = CloseDate;
                string address= groupBuying.ShipAddress == null ? "此筆訂單為外帶自取，無須填地址" : groupBuying.ShipAddress;
                string descr = (groupBuying.Description) == null ? "無" : groupBuying.Description;
                var money = groupBuying.LimitMoney == null ? 0 : groupBuying.LimitMoney;
                var num = groupBuying.LimitNumber == null ? 0 : groupBuying.LimitNumber;
                var memID = ((Users)Session["user"]).Members.FirstOrDefault().SN;
                string sql2 = "insert into GroupBuying( ID,StoreSN,Startdate,CreatedPerson,Title,Description,RequireDate,CloseDate,ShipAddress,DeliverySN,LimitMoney,LimitNumber,Continued,PaySN ) " +
                   "values(@ID,@StoreSN,@Startdate,@CreatedPerson,@Title,@Description,@RequireDate,@CloseDate,@ShipAddress,@DeliverySN,@LimitMoney,@LimitNumber,@Continued,@PaySN)";

                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("ID",groupBuying.ID),
                    new SqlParameter("StoreSN",groupBuying.StoreSN),
                    new SqlParameter("Startdate",groupBuying.Startdate),
                    new SqlParameter("CreatedPerson",memID),
                    new SqlParameter("Title",groupBuying.Title),
                    new SqlParameter("Description",descr),
                    new SqlParameter("RequireDate",groupBuying.RequireDate),
                    new SqlParameter("CloseDate",groupBuying.CloseDate),
                    new SqlParameter("ShipAddress",address),
                    new SqlParameter("DeliverySN",groupBuying.DeliverySN),
                    new SqlParameter("LimitMoney",money),
                    new SqlParameter("LimitNumber",num),
                    new SqlParameter("Continued",groupBuying.Continued),
                    new SqlParameter("PaySN",groupBuying.PaySN)
                };

                sd.executeSql(sql2, list);
                
                return RedirectToAction("Display");
            }

            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method", groupBuying.DeliverySN);
            ViewBag.CreatedPerson = new SelectList(db.Members, "SN", "Name", groupBuying.CreatedPerson);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", groupBuying.PaySN);
          
            return View(groupBuying);
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
                return RedirectToAction("Display");
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
