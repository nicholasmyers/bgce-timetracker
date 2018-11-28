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
            if (Request.IsAuthenticated)
            {
                var tIME_SHEET_ENTRY = db.TIME_SHEET_ENTRY.Include(t => t.TIME_SHEET1);
                return View(tIME_SHEET_ENTRY.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: TimeSheetEntry/Details/5
        public ActionResult Details(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: TimeSheetEntry/Create
        public ActionResult punch(bgce_timetracker.Models.LOGIN loginModel)
        {
            int id = (int)Session["UserID"];
            TempData["id"] = id;
            bool isClockedIn = db.TIME_SHEET_ENTRY.Where(timeSheet => timeSheet.employee == id && timeSheet.is_clocked_in == true).FirstOrDefault() != null;

            if (!isClockedIn) //if the user is not clocked in, clock them in and display a confirmation message telling them they clocked in successfully.
            {
                if (clockUserIn())
                {
                    loginModel.punchStatusConfirmation = "Successfully clocked in.";
                }
            }
            else{ //if the user is clocked in, clock them out and display a confirmation message telling them they clocked out successfully.
                if (clockUserOut()) 
                {
                    loginModel.punchStatusConfirmation = "Successfully clocked out.";
                }
            }

            return RedirectToAction("punchConfirmation", "Logins", loginModel);
        }

        public bool clockUserIn()
        {
            TIME_SHEET_ENTRY timeSheetEntry = new TIME_SHEET_ENTRY();

            int id = (int) TempData["id"];
            TempData.Keep("id");

            var activeTimeSheet = db.TIME_SHEET.Where(timeSheet => timeSheet.employee == id).FirstOrDefault();
            var user = db.USERs.Where(employee => employee.userID == id).FirstOrDefault();
            var timeType = "paid";
            timeSheetEntry.employee = activeTimeSheet.employee;

            //check if the user is clocking in as food service, if so set their time type to food service.
            var isFoodService = TempData["isFoodService"];
            if (isFoodService.Equals("True")) {
                timeType = "food";
            }
            
            timeSheetEntry.clock_in_time = System.DateTime.Now;
            timeSheetEntry.date = System.DateTime.Now;
            timeSheetEntry.created_on = System.DateTime.Now;
            timeSheetEntry.is_clocked_in = true;
            timeSheetEntry.time_type = timeType;
            db.TIME_SHEET_ENTRY.Add(timeSheetEntry);
            db.SaveChanges();
            return true;
        }

        public bool clockUserOut() {
            int tsid;

            int id = (int)TempData["id"];
            TempData.Keep("id");

            var activeTimeSheet = db.TIME_SHEET.Where(timeSheet => timeSheet.employee == id && timeSheet.active).FirstOrDefault();
            tsid = activeTimeSheet.timesheetID;

            var activeTimeSheetEntry = db.TIME_SHEET_ENTRY.Where(timeSheetEntry => timeSheetEntry.time_sheet == tsid && timeSheetEntry.is_clocked_in).FirstOrDefault();

            activeTimeSheetEntry.is_clocked_in = false;
            activeTimeSheetEntry.clock_out_time = DateTime.Now;
            db.Entry(activeTimeSheetEntry).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        // POST: TimeSheetEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "entryID,time_sheet,date,hours_worked,comment,clock_in_time,clock_out_time,is_clocked_in,updated_on,updated_by,time_type,overtime_hours_worked,pto_earned,created_on")] TIME_SHEET_ENTRY tIME_SHEET_ENTRY)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: TimeSheetEntry/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: TimeSheetEntry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "entryID,time_sheet,date,hours_worked,comment,clock_in_time,clock_out_time,is_clocked_in,updated_on,updated_by,time_type,overtime_hours_worked,pto_earned,created_on")] TIME_SHEET_ENTRY tIME_SHEET_ENTRY)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: TimeSheetEntry/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: TimeSheetEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.IsAuthenticated)
            {
                TIME_SHEET_ENTRY tIME_SHEET_ENTRY = db.TIME_SHEET_ENTRY.Find(id);
                db.TIME_SHEET_ENTRY.Remove(tIME_SHEET_ENTRY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
