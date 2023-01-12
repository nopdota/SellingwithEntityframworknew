using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SellingwithEntityframwork.Models;

namespace SellingwithEntityframwork.Controllers
{
    public class PD_ORDERController : Controller
    {
        private ProductionEntities db = new ProductionEntities();

        // GET: PD_ORDER
        public ActionResult MainOrder()
        {
            if (db.PD_ORDER.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).ToList().Count != 0)
            {
                ViewData["listORder"] = db.PD_ORDER.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).ToList();
            }
            else
            {
                ViewData["listORder"] = null;
            }
            ViewData["listTYPE"] = db.PD_TYPE.ToList();
            ViewData["listPRODUCT"] = db.PD_PRODUCT.ToList();
            return View();
        }
        public class GetOrderDataClass
        {

            public int Number { get; set; }
            public string ProdcutID { get; set; }
            public string ProdcutName { get; set; }
            public string Quantity { get; set; }
            public string Price { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
        }
        public JsonResult GetOrderData(string oderid)
        {

            List<GetOrderDataClass> Data = new List<GetOrderDataClass>();
            int i = 1;
            var dbproduct = db.PD_PRODUCT.ToList();
            var dbtype = db.PD_TYPE.ToList();
            foreach (var item in db.PD_ORDER.AsEnumerable().Where(x => x.OD_ID == oderid).ToList())
            {
                GetOrderDataClass dt = new GetOrderDataClass();
                dt.Number = i;
                dt.ProdcutID = item.PD_ID;
                dt.ProdcutName = dbproduct.Where(x => x.PD_ID == item.PD_ID).FirstOrDefault().PD_NAME;
                dt.Quantity = item.OD_QUANTITY.ToString();
                dt.Price = item.PRICE.ToString();
                dt.Type = dbproduct.Where(x => x.PD_ID == item.PD_ID).FirstOrDefault().TP_ID + " " + dbtype.Where(x => x.TP_ID == dbproduct.Where(q => q.PD_ID == item.PD_ID).FirstOrDefault().TP_ID).FirstOrDefault().TP_NAME;
                dt.Unit = dbproduct.Where(x => x.PD_ID == item.PD_ID).FirstOrDefault().PD_UNIT;
                i++;
                Data.Add(dt);

            }

            string strdatajsonOrder = JsonConvert.SerializeObject(Data);

            return Json(new { strjsondata = strdatajsonOrder, orderid = oderid });
        }
        public class ClassGetGroupOrderData
        {
            public int Number { get; set; }
            public string OrderID { get; set; }
            public string Date { get; set; }
            public string pricesum { get; set; }
        }
        public JsonResult GetGroupOrderData()
        {
            List<ClassGetGroupOrderData> data = new List<ClassGetGroupOrderData>();
            int i = 1;
            var PD_orderDB = db.PD_ORDER.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).ToList();
            var GropPD_ORDER = db.PD_ORDER.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).GroupBy(x => x.OD_ID);
            foreach (var item in GropPD_ORDER)
            {
                ClassGetGroupOrderData dt = new ClassGetGroupOrderData();
                dt.Number = i;
                dt.OrderID = item.Key;
                dt.Date = PD_orderDB.Where(x => x.OD_ID == item.Key).FirstOrDefault().OD_DATE.Value.ToShortDateString();
                dt.pricesum = PD_orderDB.Where(x => x.OD_ID == item.Key).Sum(x => x.PRICE).ToString();
                i++;
                data.Add(dt);
            }
            string strdatajsonOrder = JsonConvert.SerializeObject(data);
            return Json(new { strjsondata = strdatajsonOrder});
        }
        public ActionResult Index()
        {
            ViewData["listTYPE"] = db.PD_TYPE.ToList();
            ViewData["listPRODUCT"] = db.PD_PRODUCT.ToList();
            return View(db.PD_ORDER.ToList());
        }

        public ActionResult Details(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PD_ORDER pD_ORDER = db.PD_ORDER.Where(x => x.OD_ID == id1 && x.PD_ID == id2).FirstOrDefault();
            if (pD_ORDER == null)
            {
                return HttpNotFound();
            }
            return View(pD_ORDER);
        }

        public class Iteminorder 
        { 
            public string PD_ID { get; set; }
            public string TP_ID { get; set; }
            public string QUNTITY { get; set; }
            public string PRICE { get; set; }
        }

        public JsonResult InputProductToOrder(string JsonlistProduct) 
        {
            List<PD_PRODUCT> listProduct = JsonConvert.DeserializeObject<List<PD_PRODUCT>>(JsonlistProduct);
            var Iteminsame = listProduct.GroupBy(x => new { x.PD_ID }).ToList();
            List<Iteminorder> iteminorder = new List<Iteminorder>();
            var dbproduct = db.PD_PRODUCT.ToList();
            string OD_ID = "";
            bool checkfuntion = false;
            foreach (var item in Iteminsame) 
            {
                if(dbproduct.Where(x => x.PD_ID == item.Key.PD_ID).FirstOrDefault() != null) 
                {
                    Iteminorder dt = new Iteminorder();
                    dt.PD_ID = item.Key.PD_ID;
                    dt.TP_ID = dbproduct.Where(x => x.PD_ID == item.Key.PD_ID).FirstOrDefault().TP_ID;
                    dt.QUNTITY = listProduct.Where(x => x.PD_ID == item.Key.PD_ID).ToList().Count.ToString();
                    decimal price = (dbproduct.Where(x => x.PD_ID == item.Key.PD_ID).FirstOrDefault().PD_PRICE??0);
                    string strprice = (price.ToString()  ?? "0") ;
                    dt.PRICE = (decimal.Parse(strprice) * decimal.Parse(dt.QUNTITY)).ToString();
                    iteminorder.Add(dt);
                }

            }
            
            if (db.PD_ORDER.ToList().Count!=0)
            {
                var lastid = db.PD_ORDER.OrderByDescending(x => x.OD_ID).FirstOrDefault();
                string lastnumberid = lastid.OD_ID.Substring(6);
                int lastnumberidINT = int.Parse(lastnumberid) + 1;
                int checkmaxlenght = lastnumberidINT.ToString().Length;
                OD_ID = "OD" + DateTime.Now.Year.ToString();
                for (int i = checkmaxlenght; i < 4; i++)
                {
                    OD_ID += "0";
                }
                OD_ID += lastnumberidINT;
         
                foreach (var item in iteminorder) 
                {
                    PD_ORDER pD_ORDER = new PD_ORDER();
                    pD_ORDER.OD_ID = OD_ID;
                    pD_ORDER.PD_ID = item.PD_ID;
                    pD_ORDER.OD_QUANTITY = decimal.Parse(item.QUNTITY);
                    pD_ORDER.PRICE = decimal.Parse(item.PRICE);
                    pD_ORDER.OD_DATE = DateTime.Now;
                    CreateToOrder(pD_ORDER);
                    PD_PRODUCT pD_PRODUCT = db.PD_PRODUCT.Find(item.PD_ID);
                    pD_PRODUCT.PD_VALUE = pD_PRODUCT.PD_VALUE - pD_ORDER.OD_QUANTITY;
                    FuntionEdit(pD_PRODUCT);
                    checkfuntion = true;
                }
               
            }
            else
            {
                OD_ID = "OD" + DateTime.Now.Year.ToString() +"0001";

                foreach (var item in iteminorder)
                {
                    PD_ORDER pD_ORDER = new PD_ORDER();
                    pD_ORDER.OD_ID = OD_ID;
                    pD_ORDER.PD_ID = item.PD_ID;
                    pD_ORDER.OD_QUANTITY = decimal.Parse(item.QUNTITY);
                    pD_ORDER.PRICE = decimal.Parse(item.PRICE);
                    pD_ORDER.OD_DATE = DateTime.Now;
                    CreateToOrder(pD_ORDER);
                    PD_PRODUCT pD_PRODUCT = db.PD_PRODUCT.Find(item.PD_ID);
                    pD_PRODUCT.PD_VALUE = pD_PRODUCT.PD_VALUE - pD_ORDER.OD_QUANTITY;
                    FuntionEdit(pD_PRODUCT);
                    checkfuntion = true;
                }
               
            }
            return Json(new { success = checkfuntion, orderid = OD_ID });
        }
        public string FuntionEdit([Bind(Include = "PD_ID,PD_NAME,PD_VALUE,PD_PRICE,TP_ID,PD_UNIT")] PD_PRODUCT pD_PRODUCT)
        {
            string ok = "";
            if (ModelState.IsValid)
            {
                db.Entry(pD_PRODUCT).State = EntityState.Modified;
                db.SaveChanges();
                ok = "Update Store ok";
            }

            return ok;
        }

        public string CreateToOrder([Bind(Include = "OD_ID,PD_ID,OD_DATE,OD_QUANTITY,PRICE")] PD_ORDER pD_ORDER)
        {
            string ok = "";
            if (ModelState.IsValid)
            {
                db.PD_ORDER.Add(pD_ORDER);
                db.SaveChanges();
                ok = "Insert Order ok";
            }

            return ok;
        }
        public ActionResult Edit(string id1,string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_ORDER pD_ORDER = db.PD_ORDER.Where(x=>x.OD_ID==id1&&x.PD_ID==id2).FirstOrDefault();
            if (pD_ORDER == null)
            {
                return HttpNotFound();
            }
            return View(pD_ORDER);
        }

        // POST: PD_ORDER/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OD_ID,PD_ID,OD_DATE,OD_QUANTITY,PRICE")] PD_ORDER pD_ORDER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pD_ORDER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pD_ORDER);
        }

        // GET: PD_ORDER/Delete/5
        public ActionResult Delete(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PD_ORDER pD_ORDER = db.PD_ORDER.Where(x => x.OD_ID == id1 && x.PD_ID == id2).FirstOrDefault();
            if (pD_ORDER == null)
            {
                return HttpNotFound();
            }
            return View(pD_ORDER);
        }

        // POST: PD_ORDER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PD_ORDER pD_ORDER = db.PD_ORDER.Find(id);
            db.PD_ORDER.Remove(pD_ORDER);
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
