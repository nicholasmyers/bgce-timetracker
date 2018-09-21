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
    public class LoginsController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: Logins
        public ActionResult Index()
        {
            var lOGINs = db.LOGINs.Include(l => l.USER);
            return View(lOGINs.ToList());
        }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGIN lOGIN = db.LOGINs.Find(id);
            if (lOGIN == null)
            {
                return HttpNotFound();
            }
            return View(lOGIN);
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.USERs, "userID", "fname");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,username,password,password_salt,password_last_set,is_locked_out,is_password_expired")] LOGIN lOGIN)
        {
            if (ModelState.IsValid)
            {
                db.LOGINs.Add(lOGIN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userID = new SelectList(db.USERs, "userID", "fname", lOGIN.userID);
            return View(lOGIN);
        }

        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGIN lOGIN = db.LOGINs.Find(id);
            if (lOGIN == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.USERs, "userID", "fname", lOGIN.userID);
            return View(lOGIN);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,password,password_salt,password_last_set,is_locked_out,is_password_expired")] LOGIN lOGIN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOGIN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.USERs, "userID", "fname", lOGIN.userID);
            return View(lOGIN);
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGIN lOGIN = db.LOGINs.Find(id);
            if (lOGIN == null)
            {
                return HttpNotFound();
            }
            return View(lOGIN);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LOGIN lOGIN = db.LOGINs.Find(id);
            db.LOGINs.Remove(lOGIN);
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
