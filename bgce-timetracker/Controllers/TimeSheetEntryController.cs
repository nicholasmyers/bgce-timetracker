﻿using System;
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
        public ActionResult ClockIn(bgce_timetracker.Models.LOGIN userModel)
        {
            TIME_SHEET activeTimeSheet = db.TIME_SHEET.Find(userModel.userID); //query time sheet entry table instead

            if (isClockedIn(activeTimeSheet)) {
                clockUserOut(activeTimeSheet);
            }
            else {
                clockUserIn(activeTimeSheet);
            }

            return null;
        }

        public bool isClockedIn(TIME_SHEET activeTimeSheet) {
            return activeTimeSheet.active;
        }

        public void clockUserIn(TIME_SHEET activeTimeSheet) {
            TIME_SHEET_ENTRY timeSheetEntry = new TIME_SHEET_ENTRY();
            timeSheetEntry.employee = activeTimeSheet.employee;
            TimeSpan time = TimeSpan.Parse("HH:mm:ss tt");
            timeSheetEntry.clock_in_time = time;
            db.TIME_SHEET_ENTRY.Add(timeSheetEntry);
        }

        public void clockUserOut(TIME_SHEET activeTimeSheet) {

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
