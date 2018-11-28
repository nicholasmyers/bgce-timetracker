using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bgce_timetracker.Models;
using bgce_timetracker.Services;

namespace bgce_timetracker.Controllers
{
    public class UsersController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: Users
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var uSERs = db.USERs.Include(u => u.LOCATION1).Include(u => u.PAID_STAFF).Include(u => u.UNIT_DIRECTOR).Include(u => u.USER2).Include(u => u.VOLUNTEER);
                return View(uSERs.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                USER uSER = db.USERs.Find(id);
                if (uSER == null)
                {
                    return HttpNotFound();
                }
                return View(uSER);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult UserPortal()
        {
            if (Request.IsAuthenticated)
            {
                var uSERs = db.USERs.Include(u => u.LOCATION1).Include(u => u.PAID_STAFF).Include(u => u.UNIT_DIRECTOR).Include(u => u.LOGIN).Include(u => u.VOLUNTEER);
                return View(uSERs.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: Users/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name");
                ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule");
                ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID");
                ViewBag.manager = new SelectList(db.USERs, "userID", "fname");
                ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(USER newUser)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    //newUser.active = true;
                    newUser.created_on = DateTime.Today;
                    newUser.created_by = "Admin";
                    TempData["userModel"] = newUser;
                    TempData.Keep("userModel");  
                    //db.USERs.Add(newUser);
                    //db.SaveChanges();
                    if (newUser.user_type == "Volunteer")
                    {
                        TempData["userID"] = newUser.userID;
                        return RedirectToAction("Create","Logins");

                    }
                    else
                        TempData["userID"] = newUser.userID;
                    TempData["userType"] = newUser.user_type;
                    return RedirectToAction("Create", "PAID_STAFF");

                }

                ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name", newUser.location);
                ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule", newUser.userID);
                ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID", newUser.userID);
                ViewBag.manager = new SelectList(db.USERs, "userID", "fname", newUser.manager);
                ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID", newUser.userID);
                return View(newUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            var editorModel = new EditView();
           
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //USER uSER = db.USERs.Find(id);
                editorModel.User = db.USERs.Find(id);
                editorModel.LUser = db.LOGINs.Find(id);
                editorModel.PUser = db.PAID_STAFF.Find(id);
                TempData["temp"] = editorModel.LUser.password;
                TempData.Keep("temp");
                if (editorModel.User == null)
                {
                    return HttpNotFound();
                }
                ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name", editorModel.User.location);
                ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule", editorModel.User.userID);
                ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID", editorModel.User.userID);
                ViewBag.manager = new SelectList(db.USERs, "userID", "fname", editorModel.User.manager);
                ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID", editorModel.User.userID);
                return View(editorModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditView myModel)
        {
            USER uSER = myModel.User;
            //LOGIN LUser = myModel.LUser;
            //PAID_STAFF PUser = myModel.PUser;

           if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                   
                    db.Entry(myModel.User).State = EntityState.Modified;
                    db.Entry(myModel.LUser).State = EntityState.Modified;
                    if(myModel.PUser != null)db.Entry(myModel.PUser).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.location = new SelectList(db.LOCATIONs, "locationID", "name", uSER.location);
                ViewBag.userID = new SelectList(db.PAID_STAFF, "emplID", "pay_schedule", uSER.userID);
                ViewBag.userID = new SelectList(db.UNIT_DIRECTOR, "emplID", "emplID", uSER.userID);
                ViewBag.manager = new SelectList(db.USERs, "userID", "fname", uSER.manager);
                ViewBag.userID = new SelectList(db.VOLUNTEERs, "volID", "volID", uSER.userID);
                //myModel.LUser = LUser;
                return View(myModel);
            }
            else
            {
               return RedirectToAction("Index", "Home");
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                USER uSER = db.USERs.Find(id);
                if (uSER == null)
                {
                    return HttpNotFound();
                }
                return View(uSER);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.IsAuthenticated)
            {
                USER uSER = db.USERs.Find(id);
                LOGIN LUser = db.LOGINs.Find(id);
                PAID_STAFF PUser = db.PAID_STAFF.Find(id);
                VOLUNTEER VUser = db.VOLUNTEERs.Find(id);

                db.LOGINs.Remove(LUser);
                if(PUser != null)db.PAID_STAFF.Remove(PUser);
                if(VUser != null)db.VOLUNTEERs.Remove(VUser);
                db.USERs.Remove(uSER);
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
