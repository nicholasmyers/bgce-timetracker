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
    public class LoginsController : Controller
    {

        private trackerEntities db = new trackerEntities();
        /*/-----------------------------------------------------------------------------------------------------------------------------------------------Work in Progress
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------^^ Work In Progress
        /*/// GET: Logins
        public ActionResult Index()
        {
            var lOGINs = db.LOGINs.Include(l => l.USER);
            return View(lOGINs.ToList());
        } 
        public ActionResult Authorize()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(bgce_timetracker.Models.LOGIN userModel)
        {
            using (trackerEntities db = new trackerEntities())
            {
                var userDetails = db.LOGINs.Where(x => x.username == userModel.username && x.password == userModel.password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong Username or password";
                    return View("Login", userModel);
                }
                else
                {
                    Session["userID"] = userDetails.userID;
                    return RedirectToAction("Index", "Home");
                }
                }
                
        }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGIN lOGIN = db.LOGINs.Find(id);
            if (lOGIN == null)
            {
                return HttpNotFound();
            }
            return View(lOGIN);
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.USERs, "userID", "fname");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LOGIN newuser)
        {
            using (trackerEntities db = new trackerEntities())
            {
                int hash = newuser.password.GetHashCode();
                //newuser.password_salt = hash; //password salt needs to be int ??
                db.LOGINs.Add(newuser);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Success!";
            return View("Create", new LOGIN());
            //return View("Create", new LOGIN());
        }

            // GET: Logins/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGIN lOGIN = db.LOGINs.Find(id);
            if (lOGIN == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.USERs, "userID", "fname", lOGIN.userID);
            return View(lOGIN);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,password,password_salt,password_last_set,is_locked_out,is_password_expired")] LOGIN lOGIN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOGIN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.USERs, "userID", "fname", lOGIN.userID);
            return View(lOGIN);
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGIN lOGIN = db.LOGINs.Find(id);
            if (lOGIN == null)
            {
                return HttpNotFound();
            }
            return View(lOGIN);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LOGIN lOGIN = db.LOGINs.Find(id);
            db.LOGINs.Remove(lOGIN);
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
