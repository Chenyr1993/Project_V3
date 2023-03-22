using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Project_try3.Models;

namespace Project_try3.Controllers
{
    public class OrdersController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();
        SetData sd = new SetData();
        GetData gd = new GetData();
        // GET: Orders
        [LoginCheck]
        public ActionResult Index()
        {
            
            var orders = db.Orders.Include(o => o.GroupBuying).Include(o => o.Members).Include(o => o.PayType);
            return View(orders.ToList());
        }
        public ActionResult IndexforGP()
        {
            
            int id = ((Users)Session["user"]).Members.FirstOrDefault().SN;
            var orders = db.Orders.Include(o => o.GroupBuying).Where(o=>o.CustomerSN==id).Include(o => o.PayType);
           
            return View(orders.ToList());
        }
        public ActionResult _DetailsforGP(string GPID)
        {
            var orders = db.Orders.Where(o => o.GroupBuyingID==GPID).Include(o => o.Members).Include(o => o.Members).Include(o => o.PayType).Include(o=>o.Orderdetails);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }
        [LoginCheck]
        //給單筆訂購
        // GET: Orders/Create
        public ActionResult Create(int? storeSN)
        {
            //ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title");
            //ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name");
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method");
            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method");
            ViewBag.StoreSN = storeSN;
            ViewBag.StoreName = db.Stores.Where(s => s.SN == storeSN).FirstOrDefault().Name;

            ViewBag.CreatedDate = DateTime.Now;


            return View();
        }

        // POST: Orders/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orders orders, int storeSN,  string cartData)
        {//新增storeProc
         //存GPBuy
            if (ModelState.IsValid)
            {
                string sql = "select dbo.getGroupBuyID() as newID";
                var rs = gd.TableQuery(sql);
                string gpID = "";
                if (rs != null)
                {
                    DataRow drow = rs.Rows[0];
                    gpID = drow["newID"].ToString();

                }
                orders.GroupBuying.ID = gpID;
                orders.GroupBuying.StoreSN = storeSN;
                orders.GroupBuying.CreatedDate = DateTime.Now;
                orders.GroupBuying.Startdate = DateTime.Now;
                orders.GroupBuying.CloseDate = orders.GroupBuying.Startdate;
                var mem = ((Users)Session["user"]).Members.FirstOrDefault();
                var memID = mem.SN;
                orders.GroupBuying.Title = mem.Name + "的訂單";
                orders.GroupBuying.Description = "無";
                orders.GroupBuying.LimitMoney = 0;
                orders.GroupBuying.LimitNumber = 0;
                orders.GroupBuying.Continued = false;
                string address = orders.GroupBuying.ShipAddress == null ? "此筆訂單為外帶自取，無須填地址" : orders.GroupBuying.ShipAddress;
                orders.GroupBuying.PaySN = orders.PaySN;

                string sql2 = "insert into GroupBuying( ID,StoreSN,Startdate,CreatedPerson,Title,Description,RequireDate,CloseDate,ShipAddress,DeliverySN,LimitMoney,LimitNumber,Continued,PaySN ) " +
                 "values(@ID,@StoreSN,@Startdate,@CreatedPerson,@Title,@Description,@RequireDate,@CloseDate,@ShipAddress,@DeliverySN,@LimitMoney,@LimitNumber,@Continued,@PaySN)";
                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("ID",orders.GroupBuying.ID),
                    new SqlParameter("StoreSN",orders.GroupBuying.StoreSN),
                    new SqlParameter("StartDate",orders.GroupBuying.Startdate),
                    new SqlParameter("CreatedPerson",memID),
                    new SqlParameter("Title",orders.GroupBuying.Title),
                    new SqlParameter("Description",orders.GroupBuying.Description),
                    new SqlParameter("RequireDate",orders.GroupBuying.RequireDate),
                    new SqlParameter("CloseDate",orders.GroupBuying.CloseDate),
                    new SqlParameter("ShipAddress",address),
                    new SqlParameter("DeliverySN",orders.GroupBuying.DeliverySN),
                    new SqlParameter("LimitMoney",orders.GroupBuying.LimitMoney),
                    new SqlParameter("LimitNumber",orders.GroupBuying.LimitNumber),
                    new SqlParameter("Continued",orders.GroupBuying.Continued),
                    new SqlParameter("PaySN",orders.GroupBuying.PaySN)
                };
                sd.executeSql(sql2, list);

                //處理訂單
                SetData sd2 = new SetData();
                List<SqlParameter> list2 = new List<SqlParameter>
                {
                
                new SqlParameter("PaySN",orders.PaySN),
                new SqlParameter("Payed",orders.Payed),
                new SqlParameter("CustomerSN",memID),
                new SqlParameter("GroupBuyingID",gpID),
                new SqlParameter("cart", cartData)
                };
                sd2.executeSqlBySP("addOrders", list2);
                TempData["cartFlag"] = "OK ";
                return RedirectToAction("IndexforGP");
            }

            //ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title", orders.GroupBuyingID);
            //ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name", orders.CustomerSN);
            ViewBag.DeliverySN = new SelectList(db.Delivery, "SN", "Method", orders.GroupBuying.DeliverySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", orders.GroupBuying.StoreSN = storeSN);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", orders.PaySN);
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult CreateforGP(int StoreSN)
        {
            ViewBag.StoreSN = StoreSN;
            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title");
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name");
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method");
            return View();
        }
        // POST: Orders/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateforGP(Orders orders, string cartData)
        {//新增storeProc
            //處理新增欄位
            if (ModelState.IsValid)
            {
                var memID = ((Users)Session["user"]).Members.FirstOrDefault().SN;


                List<SqlParameter> list3 = new List<SqlParameter>
                {
                        new SqlParameter("PaySN",orders.PaySN),
                        new SqlParameter("Payed",orders.Payed),
                        new SqlParameter("CustomerSN",memID),
                        new SqlParameter("GroupBuyingID",orders.GroupBuyingID),
                        new SqlParameter("cart", cartData)
                        };

                sd.executeSqlBySP("addOrders", list3);
                TempData["cartFlag"] = "OK ";


                return RedirectToAction("IndexforGP", new { id = orders.ID });
            }

            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title", orders.GroupBuyingID);
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name", orders.CustomerSN);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", orders.PaySN);
            return View(orders);
        }
        // GET: Orders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title", orders.GroupBuyingID);
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name", orders.CustomerSN);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", orders.PaySN);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreatedDate,PaySN,Payed,CustomerSN,GroupBuyingID")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            ViewBag.GroupBuyingID = new SelectList(db.GroupBuying, "ID", "Title", orders.GroupBuyingID);
            ViewBag.CustomerSN = new SelectList(db.Members, "SN", "Name", orders.CustomerSN);
            ViewBag.PaySN = new SelectList(db.PayType, "SN", "Method", orders.PaySN);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }
    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {            
            db.Orderdetails.RemoveRange(db.Orderdetails.Where(o => o.OrderID == id));
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
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
