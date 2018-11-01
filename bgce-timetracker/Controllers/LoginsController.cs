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
using System.Text;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

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
            if (Request.IsAuthenticated)
            {
                var lOGINs = db.LOGINs.Include(l => l.USER);
                return View(lOGINs.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        } 
        public ActionResult Authorize()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(bgce_timetracker.Models.LOGIN userModel, String answer)
        {
            using (trackerEntities db = new trackerEntities())
            {

                var userDetails = db.LOGINs.Where(x => x.username == userModel.username).ToList();
                byte[] ss;
                string hashword;
                //Encoding enc = Encoding.UTF8;
                PasswordHash pass = new PasswordHash();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong Username";
                    return View("Authorize", userModel);
                }
                else foreach (var item in userDetails)
                {
                        string userSaltString = item.password_salt;
                        ss = Convert.FromBase64String(userSaltString);
                    //check the getbytes method used in the creation and login parts. make it consistant **PasswordHash.cs
                    //pass.GetHash(item.password, ss);
                        if(item.password == pass.GetHash(userModel.password,ss))
                        {
                            if (answer.Equals("Log in"))
                            {
                                Session["userID"] = item.userID;
                                var claims = new List<Claim>();
                                claims.Add(new Claim(ClaimTypes.Name, item.username));

                                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                                HttpContext.GetOwinContext().Authentication.SignIn(identity);
                                return RedirectToAction("Index", "Home");
                            }
                            else {
                                TempData["UserID"] = item.userID;
                                return RedirectToAction("clockIn", "TimeSheetEntry");       
                            }
                        }
                }
                userModel.LoginErrorMessage = "Wrong Username or password";
                return View("Authorize", userModel);

            }
                
        }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.USERs, "userID", "fname");
            TempData["u2"] = (int)TempData["userID"];
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
                PasswordHash pass = new PasswordHash();
                pass.Salt = pass.GenerateSalt();

                newuser.userID = (int)TempData["u2"];
                newuser.password = pass.GetHash(newuser.password, pass.Salt);
                newuser.password_salt = Convert.ToBase64String(pass.Salt);
                //int hash = newuser.password.GetHashCode();
                //newuser.password_salt = hash; //password salt needs to be int ??
                db.LOGINs.Add(newuser);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Success!";
            return RedirectToAction("Index", "Home");
            //return View("Create", new LOGIN());
        }

        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,password,password_salt,password_last_set,is_locked_out,is_password_expired")] LOGIN lOGIN)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.IsAuthenticated)
            {
                LOGIN lOGIN = db.LOGINs.Find(id);
                db.LOGINs.Remove(lOGIN);
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
