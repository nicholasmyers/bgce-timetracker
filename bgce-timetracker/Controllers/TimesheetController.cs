using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
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

                var tIME_SHEET = db.TIME_SHEET.Include(t => t.PAY_PERIOD1).Include(t => t.USER).Include(t => t.USER1);

                //return a filtered list of time sheets if a userID is received, otherwise just return the full list
                if (userID != null)
                {
                    return View(tIME_SHEET.Where(i => i.employee == userID).ToList());
                }
                else
                {
                    return View(tIME_SHEET.ToList());
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




        public ActionResult OnPostExport()
        {
            System.Diagnostics.Debug.WriteLine("9999999999999999999999999999999999999999999");
            //string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Test.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Host, sFileName);
            FileInfo file = new FileInfo(sFileName);
            var memory = new MemoryStream();
            using (var fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Test");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Date");
                row.CreateCell(1).SetCellValue("Clock In Time");
                row.CreateCell(2).SetCellValue("Clock Out Time");
                row.CreateCell(3).SetCellValue("Hours Worked");
                row.CreateCell(4).SetCellValue("Overtime Hours");
                row.CreateCell(5).SetCellValue("PTO Earned");
                row.CreateCell(6).SetCellValue("");
                row.CreateCell(7).SetCellValue("");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("12/31");
                row.CreateCell(1).SetCellValue("10am");
                row.CreateCell(2).SetCellValue("10pm");
                row.CreateCell(3).SetCellValue("12");
                row.CreateCell(4).SetCellValue("4");
                row.CreateCell(5).SetCellValue(".03");
                row.CreateCell(6).SetCellValue("test");
                row.CreateCell(7).SetCellValue("test2");


                workbook.Write(fs);
            }
            using (var stream = new FileStream(sFileName, FileMode.Open))
            {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

    }
}
