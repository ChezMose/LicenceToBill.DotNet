using System;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace LicenceToBill.Web.DemoRest
{
    public class MvcApplication : HttpApplication
    {
        #region ASP.Net MVC default

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        #endregion

        #region Authentication
        
        /// <summary>
        /// Needed to handle properly the custom authentication
        /// </summary>
        public override void Init()
        {
            base.Init();

            // event handler : at end of the authentication step
            this.PostAuthenticateRequest += this.OnAuthenticateRequest;
        }

        protected void OnAuthenticateRequest(object sender, EventArgs e)
        {
            // perform authentication
            Authenticator.CheckAuthentication(this.Request);
        }

        #endregion
    }
}