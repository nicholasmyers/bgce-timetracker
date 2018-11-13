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
    public class PayPeriodController : Controller
    {
        private trackerEntities db = new trackerEntities();

        // GET: PayPeriod
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(db.PAY_PERIOD.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PayPeriod/Details/5
        public ActionResult Details(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PAY_PERIOD pAY_PERIOD = db.PAY_PERIOD.Find(id);
                if (pAY_PERIOD == null)
                {
                    return HttpNotFound();
                }
                return View(pAY_PERIOD);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PayPeriod/Create
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

        // POST: PayPeriod/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ppID,start_date,end_date,active,created_on,created_by")] PAY_PERIOD pAY_PERIOD)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    db.PAY_PERIOD.Add(pAY_PERIOD);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(pAY_PERIOD);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PayPeriod/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PAY_PERIOD pAY_PERIOD = db.PAY_PERIOD.Find(id);
                if (pAY_PERIOD == null)
                {
                    return HttpNotFound();
                }
                return View(pAY_PERIOD);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PayPeriod/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ppID,start_date,end_date,active,created_on,created_by")] PAY_PERIOD pAY_PERIOD)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(pAY_PERIOD).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(pAY_PERIOD);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PayPeriod/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PAY_PERIOD pAY_PERIOD = db.PAY_PERIOD.Find(id);
                if (pAY_PERIOD == null)
                {
                    return HttpNotFound();
                }
                return View(pAY_PERIOD);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PayPeriod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.IsAuthenticated)
            {
                PAY_PERIOD pAY_PERIOD = db.PAY_PERIOD.Find(id);
                db.PAY_PERIOD.Remove(pAY_PERIOD);
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
