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
    public class PTORequestController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: PTORequest
        public ActionResult Index()
        {
            var pTO_REQUEST = db.PTO_REQUEST.Include(p => p.USER).Include(p => p.USER1);
            return View(pTO_REQUEST.ToList());
        }

        // GET: PTORequest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTO_REQUEST pTO_REQUEST = db.PTO_REQUEST.Find(id);
            if (pTO_REQUEST == null)
            {
                return HttpNotFound();
            }
            return View(pTO_REQUEST);
        }

        // GET: PTORequest/Create
        public ActionResult Create()
        {
            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname");
            ViewBag.requested_by = new SelectList(db.USERs, "userID", "fname");
            return View();
        }

        // POST: PTORequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "requestID,requested_by,total_time_requested,requested_on,approved,approved_on,approved_by,comments")] PTO_REQUEST pTO_REQUEST)
        {
            if (ModelState.IsValid)
            {
                db.PTO_REQUEST.Add(pTO_REQUEST);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname", pTO_REQUEST.approved_by);
            ViewBag.requested_by = new SelectList(db.USERs, "userID", "fname", pTO_REQUEST.requested_by);
            return View(pTO_REQUEST);
        }

        // GET: PTORequest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTO_REQUEST pTO_REQUEST = db.PTO_REQUEST.Find(id);
            if (pTO_REQUEST == null)
            {
                return HttpNotFound();
            }
            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname", pTO_REQUEST.approved_by);
            ViewBag.requested_by = new SelectList(db.USERs, "userID", "fname", pTO_REQUEST.requested_by);
            return View(pTO_REQUEST);
        }

        // POST: PTORequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "requestID,requested_by,total_time_requested,requested_on,approved,approved_on,approved_by,comments")] PTO_REQUEST pTO_REQUEST)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pTO_REQUEST).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname", pTO_REQUEST.approved_by);
            ViewBag.requested_by = new SelectList(db.USERs, "userID", "fname", pTO_REQUEST.requested_by);
            return View(pTO_REQUEST);
        }

        // GET: PTORequest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTO_REQUEST pTO_REQUEST = db.PTO_REQUEST.Find(id);
            if (pTO_REQUEST == null)
            {
                return HttpNotFound();
            }
            return View(pTO_REQUEST);
        }

        // POST: PTORequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PTO_REQUEST pTO_REQUEST = db.PTO_REQUEST.Find(id);
            db.PTO_REQUEST.Remove(pTO_REQUEST);
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
