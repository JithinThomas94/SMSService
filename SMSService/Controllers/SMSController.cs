using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMSService.Models;


namespace SMSService.Controllers
{
    public class SMSController : Controller
    {
        private SMSConStr db = new SMSConStr();

        // GET: SMS
        public ActionResult Index()
        {
            return View(db.SmsTables.ToList());
        }

        // GET: SMS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmsTable smsTable = db.SmsTables.Find(id);
            if (smsTable == null)
            {
                return HttpNotFound();
            }
            return View(smsTable);
        }

        // GET: SMS/Create
        public ActionResult Create()
        {
            SMSConStr db = new SMSConStr();
            var getGroupList = db.GroupTables.ToList();
            SelectList list = new SelectList(getGroupList, "Name", "Name");
            ViewBag.Groups = list;
            return View();
        }

        // POST: SMS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Groups,Description,Name,Number,Email")] SmsTable smsTable)
        {
            if (ModelState.IsValid)
            {
                SmsTable newobj = new SmsTable();
                newobj.Name = smsTable.Name;
                newobj.Email = smsTable.Email;
                newobj.Description = smsTable.Description;
                newobj.Groups = smsTable.Groups;
                newobj.Number = smsTable.Number;
                db.SmsTables.Add(newobj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(smsTable);
        }

        // GET: SMS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmsTable smsTable = db.SmsTables.Find(id);
            if (smsTable == null)
            {
                return HttpNotFound();
            }
            return View(smsTable);
        }

        // POST: SMS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Groups,Description,Name,Number,Email")] SmsTable smsTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smsTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(smsTable);
        }

        // GET: SMS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmsTable smsTable = db.SmsTables.Find(id);
            if (smsTable == null)
            {
                return HttpNotFound();
            }
            return View(smsTable);
        }

        // POST: SMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SmsTable smsTable = db.SmsTables.Find(id);
            db.SmsTables.Remove(smsTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult AlreadyExist(string EmailId, string Groups)
        {
            SmsTable db = new SmsTable();

            using (var context = new SMSConStr())
            {
                db = context.SmsTables.Where(a => a.Email.ToLowerInvariant().Equals(EmailId.ToLower()) && a.Groups.ToLowerInvariant().Equals(Groups.ToLower())).FirstOrDefault();
               
            }
            bool status;
            if (db != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }

            return Json(status, JsonRequestBehavior.AllowGet);

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
