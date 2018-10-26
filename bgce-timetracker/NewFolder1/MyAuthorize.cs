using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;
using RedirectToRouteResult = System.Web.Http.Results.RedirectToRouteResult;

namespace bgce_timetracker.NewFolder1
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MyAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (Session["userID"] == null) throw new ArgumentNullException("httpContext");

            bool allow = true;

            return allow;
        }
    }
}