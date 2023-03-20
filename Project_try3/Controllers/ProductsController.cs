using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;
using Project_try3.Models;
using static System.Net.WebRequestMethods;

namespace Project_try3.Controllers
{
    public class ProductsController : Controller
    {
        private Project_V3Entities db = new Project_V3Entities();
        SetData sd = new SetData();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Stores);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategorySN = new SelectList(db.Category, "SN", "CategoryName");
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name");

            return View();
        }

        // POST: Products/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //byte[] photo = null;
                string Sql = "INSERT INTO Products ( ProductName, Stock, Unit, UnitPrice, Discontinued, Photo,PhotoType, CreatedDate, CategorySN, StoreSN) " +
                   "VALUES (@ProductName, @Stock, @Unit, @UnitPrice, @Discontinued, CONVERT(varbinary(max),@Photo),@PhotoType, @CreatedDate, @CategorySN, @StoreSN)";
                if (file != null)
                {//如果新增無圖片資料無法通過


                    products.Photo = new byte[file.ContentLength];
                    //設定偏移量
                    file.InputStream.Read(products.Photo, 0, file.ContentLength);
                    products.PhotoType = file.ContentType;
                }
                products.CreatedDate = DateTime.Now;

                List<SqlParameter> list = new List<SqlParameter>
                    {
                    new SqlParameter("ProductName", products.ProductName),
                    new SqlParameter("Stock", products.Stock),
                    new SqlParameter("Unit", products.Unit),
                    new SqlParameter("UnitPrice", products.UnitPrice),
                    new SqlParameter("Photo", products.Photo),

                    new SqlParameter("PhotoType",products.PhotoType),
                    new SqlParameter("Discontinued", products.Discontinued),
                    new SqlParameter("CreatedDate", products.CreatedDate),
                    new SqlParameter("CategorySN", products.CategorySN),
                    new SqlParameter("StoreSN", products.StoreSN),
                    new SqlParameter("SN", products.SN)
                    };


                sd.executeSql(Sql, list);
                return RedirectToAction("Index");
            }

            ViewBag.CategorySN = new SelectList(db.Category, "SN", "CategoryName", products.CategorySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", products.StoreSN);
            return View(products);
        }

        //負向表列只有這個不需要logincheck
        [LoginCheck(flag = false)]
        //存取照片方法
        public FileContentResult GetImage(int? id)
        {
            var photo = db.Products.Find(id);
            if (photo != null)
            {
                return File(photo.Photo, photo.PhotoType);
            }
            return null;
        }




        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategorySN = new SelectList(db.Category, "SN", "CategoryName", products.CategorySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", products.StoreSN);


            return View(products);
        }

        // POST: Products/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products, HttpPostedFileBase file, DateTime dt)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    products.PhotoType = null;
                    products.Photo = null;
                }
                else
                {
                    products.Photo = new byte[file.ContentLength];
                    //設定偏移量
                    file.InputStream.Read(products.Photo, 0, file.ContentLength);
                    products.PhotoType = file.ContentType;
                }
                products.CreatedDate = dt;
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategorySN = new SelectList(db.Category, "SN", "CategoryName", products.CategorySN);
            ViewBag.StoreSN = new SelectList(db.Stores, "SN", "Name", products.StoreSN);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {

            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Products products = db.Products.Find(id);
        //    db.Products.Remove(products);
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
