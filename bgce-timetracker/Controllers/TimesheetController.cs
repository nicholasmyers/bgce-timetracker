using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        [HttpPost]
        public ActionResult index(TimeSheetSearch TSLModel , string answer)
        {
            DateTime sDate = TSLModel.StartDate;
            DateTime eDate = TSLModel.EndDate;
            
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

                int j = 1;
                if (answer.Equals("Export All Entries"))
                {
                    int x = 0;
                    row.CreateCell(x++).SetCellValue("Employee ID");
                    row.CreateCell(x++).SetCellValue("Last Name");
                    row.CreateCell(x++).SetCellValue("First Name");
                    row.CreateCell(x++).SetCellValue("Date");
                    row.CreateCell(x++).SetCellValue("Clock In Time");
                    row.CreateCell(x++).SetCellValue("Clock Out Time");
                    row.CreateCell(x++).SetCellValue("Hours Worked");
                    row.CreateCell(x++).SetCellValue("Time Type");
                    row.CreateCell(x++).SetCellValue("Overtime Hours");
                    row.CreateCell(x++).SetCellValue("PTO Earned");
                    row.CreateCell(x++).SetCellValue("");
                    row.CreateCell(x++).SetCellValue("");

                    var time = (db.TIME_SHEET_ENTRY).ToList();
                    foreach (var entry in time)
                    {
                        string fname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.fname).FirstOrDefault();
                        string lname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.lname).FirstOrDefault();
                        row = excelSheet.CreateRow(j);
                        int i = 0;
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.employee));
                        row.CreateCell(i++).SetCellValue(lname);
                        row.CreateCell(i++).SetCellValue(fname);
                        if (entry.date.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.date));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.date));
                        }
                        if (entry.clock_in_time.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_in_time));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_in_time));
                        }
                        if (entry.clock_out_time.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_out_time));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_out_time));
                        }
                        if (entry.hours_worked.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.hours_worked));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.hours_worked));
                        }
                        row.CreateCell(i++).SetCellValue(entry.time_type);
                        if (entry.overtime_hours_worked.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.overtime_hours_worked));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.overtime_hours_worked));
                        }
                        if (entry.pto_earned.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pto_earned));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.pto_earned));
                        }
                        j++;
                    }
                }  
                
                if(answer.Equals("Export Range With Entries"))
                {
                    int x = 0;
                    row.CreateCell(x++).SetCellValue("Employee ID");
                    row.CreateCell(x++).SetCellValue("Last Name");
                    row.CreateCell(x++).SetCellValue("First Name");
                    row.CreateCell(x++).SetCellValue("Date");
                    row.CreateCell(x++).SetCellValue("Clock In Time");
                    row.CreateCell(x++).SetCellValue("Clock Out Time");
                    row.CreateCell(x++).SetCellValue("Hours Worked");
                    row.CreateCell(x++).SetCellValue("Time Type");
                    row.CreateCell(x++).SetCellValue("Overtime Hours");
                    row.CreateCell(x++).SetCellValue("PTO Earned");
                    row.CreateCell(x++).SetCellValue("");
                    row.CreateCell(x++).SetCellValue("");

                    DateTime date1 = new DateTime(2018, 11, 13);
                    DateTime date2 = new DateTime(2018, 11, 24);
                    var time = db.TIME_SHEET_ENTRY.Where(i => i.date >= date1 && i.date <= date2).ToList();
                    foreach (var entry in time)
                    {
                        string fname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.fname).FirstOrDefault();
                        string lname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.lname).FirstOrDefault();
                        row = excelSheet.CreateRow(j);
                        int i = 0;
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.employee));
                        row.CreateCell(i++).SetCellValue(lname);
                        row.CreateCell(i++).SetCellValue(fname);
                        if (entry.date.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.date));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.date));
                        }
                        if (entry.clock_in_time.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_in_time));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_in_time));
                        }
                        if (entry.clock_out_time.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_out_time));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_out_time));
                        }
                        if (entry.hours_worked.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.hours_worked));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.hours_worked));
                        }
                        row.CreateCell(i++).SetCellValue(entry.time_type);
                        if (entry.overtime_hours_worked.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.overtime_hours_worked));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.overtime_hours_worked));
                        }
                        if (entry.pto_earned.HasValue)
                        {
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pto_earned));
                            //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.pto_earned));
                        }
                        j++;
                    }
                }
                if(answer.Equals("Export Range"))
                {
                    int x = 0;
                    row.CreateCell(x++).SetCellValue("Employee ID");
                    row.CreateCell(x++).SetCellValue("Last Name");
                    row.CreateCell(x++).SetCellValue("First Name");
                    row.CreateCell(x++).SetCellValue("Total Entries");
                    row.CreateCell(x++).SetCellValue("Total Hours");
                    row.CreateCell(x++).SetCellValue("Total Overtime");
                    row.CreateCell(x++).SetCellValue("Total PTO Earned");
                    row.CreateCell(x++).SetCellValue("Total PTO Used");
                    row.CreateCell(x++).SetCellValue("Total Unpaid Time");
                    row.CreateCell(x++).SetCellValue("Total Pay Earned");
                    row.CreateCell(x++).SetCellValue("");
                    DateTime date1 = new DateTime(2018, 11, 13);
                    DateTime date2 = new DateTime(2018, 11, 24);
                    var periodRange = db.PAY_PERIOD.Where(i => i.start_date >= date1 && i.start_date <= date2).ToList();
                    foreach(var period in periodRange)
                    {
                        var sheets = db.TIME_SHEET.Where(i => i.active).ToList();
                        foreach (var entry in sheets)
                        {
                            row = excelSheet.CreateRow(j);
                            string fname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.fname).FirstOrDefault();
                            string lname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.lname).FirstOrDefault();

                            int i = 0;
                            row.CreateCell(i++).SetCellValue(entry.employee);
                            row.CreateCell(i++).SetCellValue(lname);
                            row.CreateCell(i++).SetCellValue(fname);
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_entries));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_hours_worked));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_overtime_worked));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_pto_earned));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_pto_used));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_unpaid_time));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pay_earned));
                            j++;
                        }
                    }
                }

                workbook.Write(fs);
                
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);

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

        [HttpPost]
        // Post: Timesheet/Open
        public ActionResult Open(string answer)
        {
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

                int j = 1;
                if(answer.Equals("Export All With Entries"))
                {
                    int x = 0;
                    row.CreateCell(x++).SetCellValue("Employee ID");
                    row.CreateCell(x++).SetCellValue("Last Name");
                    row.CreateCell(x++).SetCellValue("First Name");
                    row.CreateCell(x++).SetCellValue("Date");
                    row.CreateCell(x++).SetCellValue("Clock In Time");
                    row.CreateCell(x++).SetCellValue("Clock Out Time");
                    row.CreateCell(x++).SetCellValue("Hours Worked");
                    row.CreateCell(x++).SetCellValue("Time Type");
                    row.CreateCell(x++).SetCellValue("Overtime Hours");
                    row.CreateCell(x++).SetCellValue("PTO Earned");
                    row.CreateCell(x++).SetCellValue("");
                    row.CreateCell(x++).SetCellValue("");
                    var sheets = db.TIME_SHEET.Where(i => i.active).ToList();
                    foreach (var id in sheets)
                    {
                        int ID = id.timesheetID;
                        var fname = db.USERs.Where(i => i.userID == id.employee).Select(i => i.fname).FirstOrDefault();
                        var lname = db.USERs.Where(i => i.userID == id.employee).Select(i => i.lname).FirstOrDefault();
                        var time = db.TIME_SHEET_ENTRY.Where(i => i.time_sheet == ID).ToList();
                        foreach (var entry in time)
                        {
                            row = excelSheet.CreateRow(j);
                            int i = 0;
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.employee));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(lname));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(fname));
                            if (entry.date.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.date));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.date));
                            }
                            if (entry.clock_in_time.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_in_time));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_in_time));
                            }
                            if (entry.clock_out_time.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_out_time));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_out_time));
                            }
                            if (entry.hours_worked.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.hours_worked));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.hours_worked));
                            }
                            row.CreateCell(i++).SetCellValue(entry.time_type);
                            if (entry.overtime_hours_worked.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.overtime_hours_worked));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.overtime_hours_worked));
                            }
                            if (entry.pto_earned.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pto_earned));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.pto_earned));
                            }
                            j++;
                        }
                    }
                }
                if (answer.Equals("Export Previous Period With Entries"))
                {
                    int x = 0;
                    row.CreateCell(x++).SetCellValue("Employee ID");
                    row.CreateCell(x++).SetCellValue("Last Name");
                    row.CreateCell(x++).SetCellValue("First Name");
                    row.CreateCell(x++).SetCellValue("Date");
                    row.CreateCell(x++).SetCellValue("Clock In Time");
                    row.CreateCell(x++).SetCellValue("Clock Out Time");
                    row.CreateCell(x++).SetCellValue("Hours Worked");
                    row.CreateCell(x++).SetCellValue("Time Type");
                    row.CreateCell(x++).SetCellValue("Overtime Hours");
                    row.CreateCell(x++).SetCellValue("PTO Earned");
                    row.CreateCell(x++).SetCellValue("");
                    row.CreateCell(x++).SetCellValue("");
                    int period = db.TIME_SHEET.Where(i => i.active).Select(i => i.pay_period).FirstOrDefault();
                    period -= 1;
                    var sheets = db.TIME_SHEET.Where(i => i.pay_period == period).ToList();
                    foreach (var id in sheets)
                    {
                        int ID = id.timesheetID;
                        var fname = db.USERs.Where(i => i.userID == id.employee).Select(i => i.fname).FirstOrDefault();
                        var lname = db.USERs.Where(i => i.userID == id.employee).Select(i => i.lname).FirstOrDefault();
                        var time = db.TIME_SHEET_ENTRY.Where(i => i.time_sheet == ID).ToList();
                        foreach (var entry in time)
                        {
                            row = excelSheet.CreateRow(j);
                            int i = 0;
                            row.CreateCell(i++).SetCellValue(Convert.ToString(entry.employee));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(lname));
                            row.CreateCell(i++).SetCellValue(Convert.ToString(fname));
                            if (entry.date.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.date));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.date));
                            }
                            if (entry.clock_in_time.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_in_time));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_in_time));
                            }
                            if (entry.clock_out_time.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_out_time));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_out_time));
                            }
                            if (entry.hours_worked.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.hours_worked));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.hours_worked));
                            }
                            row.CreateCell(i++).SetCellValue(entry.time_type);
                            if (entry.overtime_hours_worked.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.overtime_hours_worked));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.overtime_hours_worked));
                            }
                            if (entry.pto_earned.HasValue)
                            {
                                row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pto_earned));
                                //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.pto_earned));
                            }
                            j++;
                        }
                    }
                }
                if(answer.Equals("Export All"))
                {
                    int x = 0;
                    row.CreateCell(x++).SetCellValue("Employee ID");
                    row.CreateCell(x++).SetCellValue("Last Name");
                    row.CreateCell(x++).SetCellValue("First Name");
                    row.CreateCell(x++).SetCellValue("Total Entries");
                    row.CreateCell(x++).SetCellValue("Total Hours");
                    row.CreateCell(x++).SetCellValue("Total Overtime");
                    row.CreateCell(x++).SetCellValue("Total PTO Earned");
                    row.CreateCell(x++).SetCellValue("Total PTO Used");
                    row.CreateCell(x++).SetCellValue("Total Unpaid Time");
                    row.CreateCell(x++).SetCellValue("Total Pay Earned");
                    row.CreateCell(x++).SetCellValue("");
                    var sheets = db.TIME_SHEET.Where(i => i.active).ToList();
                    foreach (var entry in sheets)
                    {
                        row = excelSheet.CreateRow(j);
                        string fname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.fname).FirstOrDefault();
                        string lname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.lname).FirstOrDefault();
                        
                        int i = 0;
                        row.CreateCell(i++).SetCellValue(entry.employee);
                        row.CreateCell(i++).SetCellValue(lname);
                        row.CreateCell(i++).SetCellValue(fname);
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_entries));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_hours_worked));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_overtime_worked));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_pto_earned));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_pto_used));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_unpaid_time));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pay_earned));
                        j++;
                    }
                }
                if(answer.Equals("Export Previous Period"))
                {
                    int x = 0;
                    row.CreateCell(x++).SetCellValue("Employee ID");
                    row.CreateCell(x++).SetCellValue("Last Name");
                    row.CreateCell(x++).SetCellValue("First Name");
                    row.CreateCell(x++).SetCellValue("Total Entries");
                    row.CreateCell(x++).SetCellValue("Total Hours");
                    row.CreateCell(x++).SetCellValue("Total Overtime");
                    row.CreateCell(x++).SetCellValue("Total PTO Earned");
                    row.CreateCell(x++).SetCellValue("Total PTO Used");
                    row.CreateCell(x++).SetCellValue("Total Unpaid Time");
                    row.CreateCell(x++).SetCellValue("Total Pay Earned");
                    row.CreateCell(x++).SetCellValue("");
                    int period = db.TIME_SHEET.Where(i => i.active).Select(i => i.pay_period).FirstOrDefault();
                    period -= 1;
                    var sheets = db.TIME_SHEET.Where(i => i.pay_period == period).ToList();
                    foreach (var entry in sheets)
                    {
                        row = excelSheet.CreateRow(j);
                        string fname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.fname).FirstOrDefault();
                        string lname = db.USERs.Where(w => w.userID == entry.employee).Select(w => w.lname).FirstOrDefault();

                        int i = 0;
                        row.CreateCell(i++).SetCellValue(entry.employee);
                        row.CreateCell(i++).SetCellValue(lname);
                        row.CreateCell(i++).SetCellValue(fname);
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_entries));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_hours_worked));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_overtime_worked));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_pto_earned));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_pto_used));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.total_unpaid_time));
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pay_earned));
                        j++;
                    }
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

                int x = 0;
                row.CreateCell(x++).SetCellValue("Employee ID");
                row.CreateCell(x++).SetCellValue("Last Name");
                row.CreateCell(x++).SetCellValue("First Name");
                row.CreateCell(x++).SetCellValue("Date");
                row.CreateCell(x++).SetCellValue("Clock In Time");
                row.CreateCell(x++).SetCellValue("Clock Out Time");
                row.CreateCell(x++).SetCellValue("Hours Worked");
                row.CreateCell(x++).SetCellValue("Time Type");
                row.CreateCell(x++).SetCellValue("Overtime Hours");
                row.CreateCell(x++).SetCellValue("PTO Earned");
                row.CreateCell(x++).SetCellValue("");
                row.CreateCell(x++).SetCellValue("");

                int j = 1;
                int timeID = (int)TempData.Peek("id");
                var time = (db.TIME_SHEET_ENTRY.Where(i => i.time_sheet == timeID)).ToList();
                var empID = db.TIME_SHEET.Where(i => i.timesheetID == timeID).Select(i => i.employee).FirstOrDefault();
                var fname = db.USERs.Where(i => i.userID == empID).Select(i => i.fname).FirstOrDefault();
                var lname = db.USERs.Where(i => i.userID == empID).Select(i => i.lname).FirstOrDefault();
                foreach (var entry in time)
                {
                    row = excelSheet.CreateRow(j);
                    int i = 0;
                    row.CreateCell(i++).SetCellValue(Convert.ToString(entry.employee));
                    row.CreateCell(i++).SetCellValue(Convert.ToString(lname));
                    row.CreateCell(i++).SetCellValue(Convert.ToString(fname));
                    if (entry.date.HasValue)
                    {
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.date));
                        //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.date));
                    }
                    if (entry.clock_in_time.HasValue)
                    {
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_in_time));
                        //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_in_time));
                    }
                    if (entry.clock_out_time.HasValue)
                    {
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.clock_out_time));
                        //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.clock_out_time));
                    }
                    if (entry.hours_worked.HasValue)
                    {
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.hours_worked));
                        //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.hours_worked));
                    }
                    row.CreateCell(i++).SetCellValue(entry.time_type);
                    if (entry.overtime_hours_worked.HasValue)
                    {
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.overtime_hours_worked));
                        //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.overtime_hours_worked));
                    }
                    if (entry.pto_earned.HasValue)
                    {
                        row.CreateCell(i++).SetCellValue(Convert.ToString(entry.pto_earned));
                        //System.Diagnostics.Debug.WriteLine(Convert.ToString(entry.pto_earned));
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
