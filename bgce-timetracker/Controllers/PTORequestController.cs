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
            if (Request.IsAuthenticated)
            {
                var pTO_REQUEST = db.PTO_REQUEST.Include(p => p.USER).Include(p => p.USER1);
                return View(pTO_REQUEST.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PTORequest/MyRequests
        public ActionResult MyRequests()
        {
            if (Request.IsAuthenticated)
            {
                var pTO_REQUEST = db.PTO_REQUEST.Include(p => p.USER).Include(p => p.USER1);
                return View(pTO_REQUEST.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PTORequest/Pending
        public ActionResult Pending()
        {
            if (Request.IsAuthenticated)
            {
                var pTO_REQUEST = db.PTO_REQUEST.Include(p => p.USER).Include(p => p.USER1).Where(p => p.approved == false);
                return View(pTO_REQUEST.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PTORequest/Approve
        public ActionResult Approve(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PTO_REQUEST pTO_REQUEST = db.PTO_REQUEST.Find(id);
                if (pTO_REQUEST == null)
                {
                    return HttpNotFound();
                } else
                {
                    pTO_REQUEST.approved = true;
                    pTO_REQUEST.approved_by = (int)Session["UserID"];
                    pTO_REQUEST.approved_on = DateTime.Now;
                    Edit(pTO_REQUEST);

                    return RedirectToAction("Pending");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PTORequest/Details/5
        public ActionResult Details(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PTORequest/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.approved_by = new SelectList(db.USERs, "userID", "fname");
                ViewBag.requested_by = new SelectList(db.USERs, "userID", "fname");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PTORequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "requestID,requested_by,total_time_requested,requested_on,approved,approved_on,approved_by,comments,pto_start,pto_end")] PTO_REQUEST pTO_REQUEST)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PTORequest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PTORequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "requestID,requested_by,total_time_requested,requested_on,approved,approved_on,approved_by,comments,pto_start,pto_end")] PTO_REQUEST pTO_REQUEST)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PTORequest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PTORequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.IsAuthenticated)
            {
                PTO_REQUEST pTO_REQUEST = db.PTO_REQUEST.Find(id);
                db.PTO_REQUEST.Remove(pTO_REQUEST);
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
