using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Diagnostics;
using System.Security.Claims;
using System.Web.Helpers;
using System.Net;

namespace bgce_timetracker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            ConnectDB();
        }

        public void ConnectDB()
        {
            SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            conn.Open();

            if(conn.State == System.Data.ConnectionState.Open)
            {
                Debug.WriteLine("Connected to DB");
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastErrorInfo = Server.GetLastError();
            Exception errorInfo = null;

            bool isNotFound = false;
            if (lastErrorInfo != null)
            {
                errorInfo = lastErrorInfo.GetBaseException();
                var error = errorInfo as HttpException;
                if (error != null)
                    isNotFound = error.GetHttpCode() == (int)HttpStatusCode.NotFound;
            }
            if (isNotFound)
            {
                Server.ClearError();
                Response.Redirect("~/Home/NotFound");// Do what you need to render in view
            }
        }
    }
}
