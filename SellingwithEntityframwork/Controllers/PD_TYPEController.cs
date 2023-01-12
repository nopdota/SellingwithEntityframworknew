﻿using System;
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
    public class PD_TYPEController : Controller
    {
        private ProductionEntities db = new ProductionEntities();

        // GET: PD_TYPE ดึงข้อม฿ลด้วย Entity Framwork
        public ActionResult Index()
        {
            return View(db.PD_TYPE.ToList());
        }

        // GET: ส่ง ID ผ่าน parameter เข้าสู่ Details 
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_TYPE pD_TYPE = db.PD_TYPE.Find(id);
            if (pD_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(pD_TYPE);
        }

        // GET: หน้าเพิ่มข้อมูล
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า create
        public ActionResult Create([Bind(Include = "TP_ID,TP_NAME")] PD_TYPE pD_TYPE)
        {
            pD_TYPE.TP_ID = "1";
            var lastid = db.PD_PRODUCT.OrderByDescending(x => x.PD_ID).FirstOrDefault();
            if (lastid != null)
            {
                string lastnumberid = lastid.PD_ID.Substring(2);
                int lastnumberidINT = int.Parse(lastnumberid) + 1;
                int checkmaxlenght = lastnumberidINT.ToString().Length;
                string TP_ID = "TP";
                for (int i = checkmaxlenght; i < 3; i++)
                {
                    TP_ID += "0";
                }
                TP_ID += lastnumberidINT;

                pD_TYPE.TP_ID = TP_ID;
            }
            else
            {
                pD_TYPE.TP_ID = "TP001";
            }
            if (ModelState.IsValid)
            {
                db.PD_TYPE.Add(pD_TYPE);
                db.SaveChanges();
                return RedirectToAction("Index");//รีไดเรคกลับไปหน้า Index ของ PD_TYPE คอนโทรเลอร์
            }

            return View(pD_TYPE);
        }

        // ส่ง ID ผ่าน parameter มาที่ หน้า Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_TYPE pD_TYPE = db.PD_TYPE.Find(id);
            if (pD_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(pD_TYPE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า Edit
        public ActionResult Edit([Bind(Include = "TP_ID,TP_NAME")] PD_TYPE pD_TYPE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pD_TYPE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pD_TYPE);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_TYPE pD_TYPE = db.PD_TYPE.Find(id);
            if (pD_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(pD_TYPE);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PD_TYPE pD_TYPE = db.PD_TYPE.Find(id);
            db.PD_TYPE.Remove(pD_TYPE);
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
