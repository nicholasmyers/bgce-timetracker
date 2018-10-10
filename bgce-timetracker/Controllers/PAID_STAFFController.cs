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
    public class PAID_STAFFController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: PAID_STAFF
        public ActionResult Index()
        {
            var pAID_STAFF = db.PAID_STAFF.Include(p => p.USER);
            return View(pAID_STAFF.ToList());
        }

        // GET: PAID_STAFF/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAID_STAFF pAID_STAFF = db.PAID_STAFF.Find(id);
            if (pAID_STAFF == null)
            {
                return HttpNotFound();
            }
            return View(pAID_STAFF);
        }

        // GET: PAID_STAFF/Create
        public ActionResult Create()
        {
            //cViewBag.emplID = new SelectList(db.USERs, "userID", "fname");
            return View();
        }

        // POST: PAID_STAFF/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emplID,pto_accrual_rate,max_pto_accrual,total_pto_accrued,pay_rate,pay_schedule")] PAID_STAFF pAID_STAFF)
        {
            if (ModelState.IsValid)
            {
                db.PAID_STAFF.Add(pAID_STAFF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.emplID = new SelectList(db.USERs, "userID", "fname", pAID_STAFF.emplID);
            return View(pAID_STAFF);
        }

        // GET: PAID_STAFF/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAID_STAFF pAID_STAFF = db.PAID_STAFF.Find(id);
            if (pAID_STAFF == null)
            {
                return HttpNotFound();
            }
            ViewBag.emplID = new SelectList(db.USERs, "userID", "fname", pAID_STAFF.emplID);
            return View(pAID_STAFF);
        }

        // POST: PAID_STAFF/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emplID,pto_accrual_rate,max_pto_accrual,total_pto_accrued,pay_rate,pay_schedule")] PAID_STAFF pAID_STAFF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pAID_STAFF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.emplID = new SelectList(db.USERs, "userID", "fname", pAID_STAFF.emplID);
            return View(pAID_STAFF);
        }

        // GET: PAID_STAFF/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAID_STAFF pAID_STAFF = db.PAID_STAFF.Find(id);
            if (pAID_STAFF == null)
            {
                return HttpNotFound();
            }
            return View(pAID_STAFF);
        }

        // POST: PAID_STAFF/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PAID_STAFF pAID_STAFF = db.PAID_STAFF.Find(id);
            db.PAID_STAFF.Remove(pAID_STAFF);
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
