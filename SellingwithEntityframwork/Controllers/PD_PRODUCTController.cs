using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SellingwithEntityframwork.Models;

namespace SellingwithEntityframwork.Controllers
{
    public class PD_PRODUCTController : Controller
    {
        private ProductionEntities db = new ProductionEntities();

        // GET: PD_PRODUCT ดึงข้อม฿ลด้วย Entity Framwork
        public ActionResult Index()
        {
            ViewData["listTYPE"] = db.PD_TYPE.ToList();
            return View(db.PD_PRODUCT.ToList());
        }
        // GET: ส่ง ID ผ่าน parameter เข้าสู่ Details 
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_PRODUCT pD_PRODUCT = db.PD_PRODUCT.Find(id);
            if (pD_PRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pD_PRODUCT);
        }
        // GET: หน้าเพิ่มข้อมูล
        public ActionResult Create()
        {
            ViewData["listTYPE"] = db.PD_TYPE.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า create
        public ActionResult Create([Bind(Include = "PD_ID,PD_NAME,PD_VALUE,PD_PRICE,TP_ID,PD_UNIT")] PD_PRODUCT pD_PRODUCT)
        {
            pD_PRODUCT.PD_ID = "1";
            var lastid = db.PD_PRODUCT.OrderByDescending(x => x.PD_ID).FirstOrDefault();
            if (lastid != null)
            {
                string lastnumberid = lastid.PD_ID.Substring(2);
                int lastnumberidINT = int.Parse(lastnumberid) + 1;
                int checkmaxlenght = lastnumberidINT.ToString().Length;
                string PD_ID = "PD";
                for (int i = checkmaxlenght; i < 4; i++) 
                {
                    PD_ID += "0";
                }
                PD_ID += lastnumberidINT;

                pD_PRODUCT.PD_ID = PD_ID;
            }
            else
            {
                pD_PRODUCT.PD_ID ="PD0001";
            }

            if (ModelState.IsValid)
            {
                db.PD_PRODUCT.Add(pD_PRODUCT);
                db.SaveChanges();
                return RedirectToAction("Index"); //รีไดเรคกลับไปหน้า Index ของ PD_PRODUCT คอนโทรเลอร์
            }

            return View(pD_PRODUCT);
        }
        // ส่ง ID ผ่าน parameter มาที่ หน้า Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_PRODUCT pD_PRODUCT = db.PD_PRODUCT.Find(id);
            if (pD_PRODUCT == null)
            {
                return HttpNotFound();
            }
            ViewData["listTYPE"] = db.PD_TYPE.ToList();
            return View(pD_PRODUCT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า edit
        public ActionResult Edit([Bind(Include = "PD_ID,PD_NAME,PD_VALUE,PD_PRICE,TP_ID,PD_UNIT")] PD_PRODUCT pD_PRODUCT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pD_PRODUCT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); //รีไดเรคกลับไปหน้า Index ของ PD_PRODUCT คอนโทรเลอร์
            }
            return View(pD_PRODUCT);
        }
        // ส่ง ID ผ่าน parameter มาที่ หน้า Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_PRODUCT pD_PRODUCT = db.PD_PRODUCT.Find(id);
            if (pD_PRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pD_PRODUCT);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PD_PRODUCT pD_PRODUCT = db.PD_PRODUCT.Find(id);
            db.PD_PRODUCT.Remove(pD_PRODUCT);
            db.SaveChanges();
            return RedirectToAction("Index"); //รีไดเรคกลับไปหน้า Index ของ PD_PRODUCT คอนโทรเลอร์
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
