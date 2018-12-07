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

        private bool isClockedIn() {
            int id = (int)Session["UserID"];
            return db.TIME_SHEET_ENTRY.Where(timeSheetEntry => timeSheetEntry.employee == id && timeSheetEntry.is_clocked_in == true).FirstOrDefault() != null;
        }

        private bgce_timetracker.Models.TIME_SHEET getActiveTimeSheet() {
            int id = (int)Session["UserID"];
            return db.TIME_SHEET.Where(timeSheet => timeSheet.employee == id && timeSheet.active).FirstOrDefault();
        }

        private bgce_timetracker.Models.TIME_SHEET_ENTRY getActiveTimeSheetEntry() {
            int id = (int)Session["UserID"];
            return db.TIME_SHEET_ENTRY.Where(tse => tse.entryID == id && tse.is_clocked_in == true).FirstOrDefault();
        }

        public ActionResult punch(bgce_timetracker.Models.LOGIN loginModel)
        {
            int id = (int)Session["UserID"];
            TempData["id"] = id;

            if (!isClockedIn()) //if the user is not clocked in, clock them in and display a confirmation message telling them they clocked in successfully.
            {
                if (clockUserIn())
                {
                    loginModel.punchStatusConfirmation = "Successfully punched in.";
                }
            }
            else{ //if the user is clocked in, clock them out and display a confirmation message telling them they clocked out successfully.
                if (clockUserOut()) 
                {
                    loginModel.punchStatusConfirmation = "Successfully punched out.";
                }
            }

            return RedirectToAction("punchConfirmation", "Logins", loginModel);
        }

        public bool clockUserIn()
        {
            TIME_SHEET_ENTRY timeSheetEntry = new TIME_SHEET_ENTRY();

            int id = (int) TempData["id"];
            TempData.Keep("id");

            var activeTimeSheet = getActiveTimeSheet();
            var user = db.USERs.Where(employee => employee.userID == id).FirstOrDefault();
            var timeType = user.user_type;
            timeSheetEntry.employee = activeTimeSheet.employee;
            
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
        // GET: TimeSheetEntry/Create
        public ActionResult Create()
        {
            int id = (int)Session["UserID"];
            if (isClockedIn())
            {
                //let the user set their clock out time and update the current time sheet entry they're clocked in on
                var activeTimeSheetEntry = getActiveTimeSheetEntry();
                return View(activeTimeSheetEntry);
            }
            else
            {
                //let the user set a clock in and out time for a new time sheet entry for today
            }
            return View();
        }

        // POST: TimeSheetEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TIME_SHEET_ENTRY tIME_SHEET_ENTRY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIME_SHEET_ENTRY).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
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
