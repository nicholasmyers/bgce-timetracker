using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using bgce_timetracker.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace bgce_timetracker.Controllers
{
    public class TimesheetController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: Timesheet
        public ActionResult Index(int? userID)
        {
            if (Request.IsAuthenticated)
            {
                TimeSheetSearch TSLModel = new TimeSheetSearch();
               
                var tIME_SHEET = db.TIME_SHEET.Include(t => t.PAY_PERIOD1).Include(t => t.USER).Include(t => t.USER1);
                TSLModel.TSList = tIME_SHEET.ToList();

                //return a filtered list of time sheets if a userID is received, otherwise just return the full list
                if (userID != null)
                {
                    TSLModel.TSList = tIME_SHEET.Where(i => i.employee == userID).ToList();
                    return View(TSLModel);
                }
                else
                {
                    return View(TSLModel);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Timesheet/MyTimesheet
        public ActionResult MyTimesheet()
        {
            if (Request.IsAuthenticated)
            {
                var tIME_SHEET = db.TIME_SHEET.Include(t => t.PAY_PERIOD1).Include(t => t.USER).Include(t => t.USER1);
                return View(tIME_SHEET.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Timesheet/Open
        public ActionResult Open()
        {
            if (Request.IsAuthenticated)
            {
                var tIME_SHEET = db.TIME_SHEET.Include(t => t.PAY_PERIOD1).Include(t => t.USER).Include(t => t.USER1).Where(t => t.active == true);
                return View(tIME_SHEET.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Timesheet/Details/5
        public ActionResult Details(int? id)
        {
            TempData["id"] = id;
            TempData.Keep("id");
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public ActionResult Details()
        {

            System.Diagnostics.Debug.WriteLine("9999999999999999999999999999999999999999999");
            string sWebRootFolder = HttpRuntime.AppDomainAppPath;
            string sFileName = @"Test.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Test");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Date");
                row.CreateCell(1).SetCellValue("Clock In Time");
                row.CreateCell(2).SetCellValue("Clock Out Time");
                row.CreateCell(3).SetCellValue("Hours Worked");
                row.CreateCell(4).SetCellValue("Time Type");
                row.CreateCell(5).SetCellValue("Overtime Hours");
                row.CreateCell(6).SetCellValue("PTO Earned");
                row.CreateCell(7).SetCellValue("");
                row.CreateCell(8).SetCellValue("");

                int j = 1;
                int timeID = (int)TempData.Peek("id");
                System.Diagnostics.Debug.WriteLine(timeID);
                var time = (db.TIME_SHEET_ENTRY.Where(i => i.time_sheet == timeID)).ToList();
                foreach (var entry in time)
                {
                    row = excelSheet.CreateRow(j);
                    if (entry.date.HasValue)
                    {
                        row.CreateCell(0).SetCellValue(Convert.ToString(entry.date));
                        System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.date));
                    }
                    if (entry.clock_in_time.HasValue)
                    {
                        row.CreateCell(1).SetCellValue(Convert.ToString(entry.clock_in_time));
                        System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_in_time));
                    }
                    if (entry.clock_out_time.HasValue)
                    {
                        row.CreateCell(2).SetCellValue(Convert.ToString(entry.clock_out_time));
                        System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_out_time));
                    }
                    row.CreateCell(4).SetCellValue(entry.time_type);
                    if (entry.hours_worked.HasValue)
                    {
                        row.CreateCell(3).SetCellValue(Convert.ToString(entry.hours_worked));
                        System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.hours_worked));
                    }
                    if (entry.pto_earned.HasValue)
                    {
                        row.CreateCell(6).SetCellValue(Convert.ToString(entry.pto_earned));
                        System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.pto_earned));
                    }
                    if (entry.overtime_hours_worked.HasValue)
                    {
                        row.CreateCell(5).SetCellValue(Convert.ToString(entry.overtime_hours_worked));
                        System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.overtime_hours_worked));
                    }
                    j++;
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        
    }

        // GET: Timesheet/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.pay_period = new SelectList(db.PAY_PERIOD, "ppID", "created_by");
                ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname");
                ViewBag.employee = new SelectList(db.USERs, "userID", "fname");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Timesheet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "timesheetID,employee,submitted_on,approved_on,approved_by,submitted,approved,comments,active,is_missing_punches,total_entries,total_hours_worked,total_overtime_worked,pay_period,total_pto_used,total_unpaid_time,created_on,total_pto_earned")] TIME_SHEET tIME_SHEET)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Timesheet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Timesheet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "timesheetID,employee,submitted_on,approved_on,approved_by,submitted,approved,comments,active,is_missing_punches,total_entries,total_hours_worked,total_overtime_worked,pay_period,total_pto_used,total_unpaid_time,created_on,total_pto_earned")] TIME_SHEET tIME_SHEET)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Timesheet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Timesheet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.IsAuthenticated)
            {
                TIME_SHEET tIME_SHEET = db.TIME_SHEET.Find(id);
                db.TIME_SHEET.Remove(tIME_SHEET);
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
