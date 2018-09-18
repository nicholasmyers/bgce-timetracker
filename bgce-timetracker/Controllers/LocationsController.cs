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
            return View(db.LOCATIONs.ToList());
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
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

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "locationID,name,address,street,city,state,country")] LOCATION lOCATION)
        {
            if (ModelState.IsValid)
            {
                db.LOCATIONs.Add(lOCATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lOCATION);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "locationID,name,address,street,city,state,country")] LOCATION lOCATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOCATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lOCATION);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LOCATION lOCATION = db.LOCATIONs.Find(id);
            db.LOCATIONs.Remove(lOCATION);
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
