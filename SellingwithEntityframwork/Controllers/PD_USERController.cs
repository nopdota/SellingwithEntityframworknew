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
    public class PD_USERController : Controller
    {
        private ProductionEntities db = new ProductionEntities();

        // GET: PD_USER
        public ActionResult Index()
        {
            return View(db.PD_USER.ToList());
        }


        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_USER pD_USER = db.PD_USER.Find(id);
            if (pD_USER == null)
            {
                return HttpNotFound();
            }
            return View(pD_USER);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USERNAME,PASSWORD")] PD_USER pD_USER)
        {
            bool checkuser = false;
            if (pD_USER.USERNAME != null) 
            { 
                int checkusercount =  db.PD_USER.Where(x => x.USERNAME == pD_USER.USERNAME).ToList().Count;
                if (checkusercount == 0)
                {
                    checkuser = false;
                }
                else 
                {
                    checkuser = true;
                }
            }
            if (checkuser == false)
            {
                if (ModelState.IsValid)
                {
                    db.PD_USER.Add(pD_USER);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else 
            {
                ViewBag.alertmsg = "username ถูกใช้ไปแล้ว";
            }
            return View(pD_USER);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_USER pD_USER = db.PD_USER.Find(id);
            if (pD_USER == null)
            {
                return HttpNotFound();
            }
            return View(pD_USER);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USERNAME,PASSWORD")] PD_USER pD_USER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pD_USER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pD_USER);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_USER pD_USER = db.PD_USER.Find(id);
            if (pD_USER == null)
            {
                return HttpNotFound();
            }
            return View(pD_USER);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PD_USER pD_USER = db.PD_USER.Find(id);
            db.PD_USER.Remove(pD_USER);
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
