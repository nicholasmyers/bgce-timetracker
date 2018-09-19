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
    public class UsersController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: Users
        public ActionResult Index()
        {
            var uSERs = db.USERs.Include(u => u.LOCATION1).Include(u => u.PAID_STAFF).Include(u => u.UNIT_DIRECTOR).Include(u => u.USER2).Include(u => u.VOLUNTEER);
            return View(uSERs.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERs.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name");
            ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule");
            ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID");
            ViewBag.manager = new SelectList(db.USERs, "userID", "fname");
            ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,fname,lname,active,start_date,created_on,created_by,updated_on,updated_by,manager,location,username,email,passwrd,passwrd_last_set,passwrd_expired,passwd_salt,is_administrator,user_type,total_hours_worked")] USER uSER)
        {
            if (ModelState.IsValid)
            {
                db.USERs.Add(uSER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name", uSER.location);
            ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule", uSER.userID);
            ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID", uSER.userID);
            ViewBag.manager = new SelectList(db.USERs, "userID", "fname", uSER.manager);
            ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID", uSER.userID);
            return View(uSER);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERs.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name", uSER.location);
            ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule", uSER.userID);
            ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID", uSER.userID);
            ViewBag.manager = new SelectList(db.USERs, "userID", "fname", uSER.manager);
            ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID", uSER.userID);
            return View(uSER);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,fname,lname,active,start_date,created_on,created_by,updated_on,updated_by,manager,location,username,email,passwrd,passwrd_last_set,passwrd_expired,passwd_salt,is_administrator,user_type,total_hours_worked")] USER uSER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name", uSER.location);
            ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule", uSER.userID);
            ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID", uSER.userID);
            ViewBag.manager = new SelectList(db.USERs, "userID", "fname", uSER.manager);
            ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID", uSER.userID);
            return View(uSER);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERs.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER uSER = db.USERs.Find(id);
            db.USERs.Remove(uSER);
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
