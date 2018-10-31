using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bgce_timetracker.Models;

namespace bgce_timetracker.Controllers
{
    public class TimeSheetEntryController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: TimeSheetEntry
        public ActionResult Index()
        {
            var tIME_SHEET_ENTRY = db.TIME_SHEET_ENTRY.Include(t => t.TIME_SHEET1);
            return View(tIME_SHEET_ENTRY.ToList());
        }

        // GET: TimeSheetEntry/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIME_SHEET_ENTRY tIME_SHEET_ENTRY = db.TIME_SHEET_ENTRY.Find(id);
            if (tIME_SHEET_ENTRY == null)
            {
                return HttpNotFound();
            }
            return View(tIME_SHEET_ENTRY);
        }

        // GET: TimeSheetEntry/Create
        public ActionResult punch()
        {
            int id = (int)Session["UserID"];
            var activeTimeSheet = db.TIME_SHEET.Where(x => x.employee == id).ToList();



            if (!isClockedIn(activeTimeSheet))
            {
                clockUserIn(activeTimeSheet);
            }
            /*
            else{
                clockUserOut(activeTimeSheet);
            }
            */
            

            return RedirectToAction("Index", "Home");
        }

        public bool isClockedIn(List<TIME_SHEET> activeTimeSheet) {
            int id = (int)Session["UserID"];
            var tse = db.TIME_SHEET_ENTRY.Where(x => x.is_clocked_in == true && x.employee == id);
            return tse == null;
        }

        public void clockUserIn(List<TIME_SHEET> activeTimeSheet)
        {
            TIME_SHEET_ENTRY timeSheetEntry = new TIME_SHEET_ENTRY();
            foreach (var item in activeTimeSheet)
            {
                timeSheetEntry.employee = item.employee;
            }
            timeSheetEntry.clock_in_time = System.DateTime.Now;
            timeSheetEntry.date = System.DateTime.Now;
            timeSheetEntry.created_on = System.DateTime.Now;
            timeSheetEntry.is_clocked_in = true;
            db.TIME_SHEET_ENTRY.Add(timeSheetEntry);
            db.SaveChanges();
        }

        public void clockUserOut(List<TIME_SHEET> activeTimeSheet) {
            int tsid = 0;
            TIME_SHEET_ENTRY updated = new TIME_SHEET_ENTRY();

            foreach (var item in activeTimeSheet) {
                tsid = item.timesheetID;
            }

            var activeTimeSheetEntry = db.TIME_SHEET_ENTRY.Where(x => x.time_sheet == tsid).ToList();

            foreach (var item in activeTimeSheetEntry) {
                updated.clock_in_time = item.clock_in_time;
                updated.date = item.date;
                updated.created_on = item.created_on;
                updated.clock_out_time = System.DateTime.Now;
            }
            db.Entry(updated).State = EntityState.Modified;
            db.SaveChanges();
        }

        // POST: TimeSheetEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "entryID,time_sheet,date,hours_worked,comment,clock_in_time,clock_out_time,is_clocked_in,updated_on,updated_by,time_type,overtime_hours_worked,pto_earned,created_on")] TIME_SHEET_ENTRY tIME_SHEET_ENTRY)
        {
            if (ModelState.IsValid)
            {
                db.TIME_SHEET_ENTRY.Add(tIME_SHEET_ENTRY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.time_sheet = new SelectList(db.TIME_SHEET, "timesheetID", "comments", tIME_SHEET_ENTRY.time_sheet);
            return View(tIME_SHEET_ENTRY);
        }

        // GET: TimeSheetEntry/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIME_SHEET_ENTRY tIME_SHEET_ENTRY = db.TIME_SHEET_ENTRY.Find(id);
            if (tIME_SHEET_ENTRY == null)
            {
                return HttpNotFound();
            }
            ViewBag.time_sheet = new SelectList(db.TIME_SHEET, "timesheetID", "comments", tIME_SHEET_ENTRY.time_sheet);
            return View(tIME_SHEET_ENTRY);
        }

        // POST: TimeSheetEntry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "entryID,time_sheet,date,hours_worked,comment,clock_in_time,clock_out_time,is_clocked_in,updated_on,updated_by,time_type,overtime_hours_worked,pto_earned,created_on")] TIME_SHEET_ENTRY tIME_SHEET_ENTRY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIME_SHEET_ENTRY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.time_sheet = new SelectList(db.TIME_SHEET, "timesheetID", "comments", tIME_SHEET_ENTRY.time_sheet);
            return View(tIME_SHEET_ENTRY);
        }

        // GET: TimeSheetEntry/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIME_SHEET_ENTRY tIME_SHEET_ENTRY = db.TIME_SHEET_ENTRY.Find(id);
            if (tIME_SHEET_ENTRY == null)
            {
                return HttpNotFound();
            }
            return View(tIME_SHEET_ENTRY);
        }

        // POST: TimeSheetEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TIME_SHEET_ENTRY tIME_SHEET_ENTRY = db.TIME_SHEET_ENTRY.Find(id);
            db.TIME_SHEET_ENTRY.Remove(tIME_SHEET_ENTRY);
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
