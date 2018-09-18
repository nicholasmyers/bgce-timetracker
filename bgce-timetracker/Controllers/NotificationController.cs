using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bgce_timetracker.Models;

namespace bgce_timetracker.Controllers
{
    public class NotificationController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: Notification
        public ActionResult Index()
        {
            var nOTIFICATIONs = db.NOTIFICATIONs.Include(n => n.USER).Include(n => n.USER1);
            return View(nOTIFICATIONs.ToList());
        }

        // GET: Notification/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            return View(nOTIFICATION);
        }

        // GET: Notification/Create
        public ActionResult Create()
        {
            ViewBag.user_recipient = new SelectList(db.USERs, "userID", "fname");
            ViewBag.user_sender = new SelectList(db.USERs, "userID", "fname");
            return View();
        }

        // POST: Notification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "notifID,recipients,type,trigger,content,created_on,user_recipient,user_sender")] NOTIFICATION nOTIFICATION)
        {
            if (ModelState.IsValid)
            {
                db.NOTIFICATIONs.Add(nOTIFICATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_recipient = new SelectList(db.USERs, "userID", "fname", nOTIFICATION.user_recipient);
            ViewBag.user_sender = new SelectList(db.USERs, "userID", "fname", nOTIFICATION.user_sender);
            return View(nOTIFICATION);
        }

        // GET: Notification/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_recipient = new SelectList(db.USERs, "userID", "fname", nOTIFICATION.user_recipient);
            ViewBag.user_sender = new SelectList(db.USERs, "userID", "fname", nOTIFICATION.user_sender);
            return View(nOTIFICATION);
        }

        // POST: Notification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "notifID,recipients,type,trigger,content,created_on,user_recipient,user_sender")] NOTIFICATION nOTIFICATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nOTIFICATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_recipient = new SelectList(db.USERs, "userID", "fname", nOTIFICATION.user_recipient);
            ViewBag.user_sender = new SelectList(db.USERs, "userID", "fname", nOTIFICATION.user_sender);
            return View(nOTIFICATION);
        }

        // GET: Notification/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            return View(nOTIFICATION);
        }

        // POST: Notification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            db.NOTIFICATIONs.Remove(nOTIFICATION);
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
