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
    public class LocationsController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: Locations
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(db.LOCATIONs.ToList());
            } 
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LOCATION lOCATION = db.LOCATIONs.Find(id);
                if (lOCATION == null)
                {
                    return HttpNotFound();
                }
                return View(lOCATION);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "locationID,name,address,street,city,state,country")] LOCATION lOCATION)
        {
            if(Request.IsAuthenticated)
            { 
                if (ModelState.IsValid)
                {
                    db.LOCATIONs.Add(lOCATION);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(lOCATION);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LOCATION lOCATION = db.LOCATIONs.Find(id);
                if (lOCATION == null)
                {
                    return HttpNotFound();
                }
                return View(lOCATION);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "locationID,name,address,street,city,state,country")] LOCATION lOCATION)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(lOCATION).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(lOCATION);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LOCATION lOCATION = db.LOCATIONs.Find(id);
                if (lOCATION == null)
                {
                    return HttpNotFound();
                }
                return View(lOCATION);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.IsAuthenticated)
            {
                LOCATION lOCATION = db.LOCATIONs.Find(id);
                db.LOCATIONs.Remove(lOCATION);
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
