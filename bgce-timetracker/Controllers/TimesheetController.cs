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
    public class TimesheetController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: Timesheet
        public ActionResult Index()
        {
            var tIME_SHEET = db.TIME_SHEET.Include(t => t.PAY_PERIOD1).Include(t => t.USER).Include(t => t.USER1);
            return View(tIME_SHEET.ToList());
        }

        // GET: Timesheet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIME_SHEET tIME_SHEET = db.TIME_SHEET.Find(id);
            if (tIME_SHEET == null)
            {
                return HttpNotFound();
            }
            return View(tIME_SHEET);
        }

        // GET: Timesheet/Create
        public ActionResult Create()
        {
            ViewBag.pay_period = new SelectList(db.PAY_PERIOD, "ppID", "created_by");
            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname");
            ViewBag.employee = new SelectList(db.USERs, "userID", "fname");
            return View();
        }

        // POST: Timesheet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "timesheetID,employee,submitted_on,approved_on,approved_by,submitted,approved,comments,active,is_missing_punches,total_entries,total_hours_worked,total_overtime_worked,pay_period,total_pto_used,total_unpaid_time,created_on")] TIME_SHEET tIME_SHEET)
        {
            if (ModelState.IsValid)
            {
                db.TIME_SHEET.Add(tIME_SHEET);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pay_period = new SelectList(db.PAY_PERIOD, "ppID", "created_by", tIME_SHEET.pay_period);
            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname", tIME_SHEET.approved_by);
            ViewBag.employee = new SelectList(db.USERs, "userID", "fname", tIME_SHEET.employee);
            return View(tIME_SHEET);
        }

        // GET: Timesheet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIME_SHEET tIME_SHEET = db.TIME_SHEET.Find(id);
            if (tIME_SHEET == null)
            {
                return HttpNotFound();
            }
            ViewBag.pay_period = new SelectList(db.PAY_PERIOD, "ppID", "created_by", tIME_SHEET.pay_period);
            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname", tIME_SHEET.approved_by);
            ViewBag.employee = new SelectList(db.USERs, "userID", "fname", tIME_SHEET.employee);
            return View(tIME_SHEET);
        }

        // POST: Timesheet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "timesheetID,employee,submitted_on,approved_on,approved_by,submitted,approved,comments,active,is_missing_punches,total_entries,total_hours_worked,total_overtime_worked,pay_period,total_pto_used,total_unpaid_time,created_on")] TIME_SHEET tIME_SHEET)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIME_SHEET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pay_period = new SelectList(db.PAY_PERIOD, "ppID", "created_by", tIME_SHEET.pay_period);
            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname", tIME_SHEET.approved_by);
            ViewBag.employee = new SelectList(db.USERs, "userID", "fname", tIME_SHEET.employee);
            return View(tIME_SHEET);
        }

        // GET: Timesheet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIME_SHEET tIME_SHEET = db.TIME_SHEET.Find(id);
            if (tIME_SHEET == null)
            {
                return HttpNotFound();
            }
            return View(tIME_SHEET);
        }

        // POST: Timesheet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TIME_SHEET tIME_SHEET = db.TIME_SHEET.Find(id);
            db.TIME_SHEET.Remove(tIME_SHEET);
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
