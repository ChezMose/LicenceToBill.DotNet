using System.Web.Mvc;
using System.Web.Security;
using LicenceToBill.Api;

namespace LicenceToBill.Web.DemoRest
{
    /// <summary>
    /// Controller for public pages
    /// </summary>
    public class HomeController : Controller
    {
        #region Home

        /// <summary>
        /// Defaut page
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            // if authenticated
            if(CustomPrincipal.Current.IsAuthenticated)
            {
                // go to the ho eof the demo
                return this.RedirectToAction("Index", "Demo");
            }
            return View();
        }

        #endregion

        #region Login

        /// <summary>
        /// Login page
        /// </summary>
        [HttpGet]
        public ActionResult Login()
        {
            // if already authenticated
            if(CustomPrincipal.Current.IsAuthenticated)
            {
                // redirect to home
                return this.RedirectToAction("Index", "Demo");
            }

            return View(new ModelLogin());
        }
        /// <summary>
        /// Login action
        /// </summary>
        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            // if password is ok
            if (password == "ltb")
            {
                // format the login
                login = login.Trim().ToLower();

                // then authenticate
                if (Authenticator.Authenticate(login))
                {
                    // get the new principal
                    var principal = CustomPrincipal.Current;
                    // post the user's data (to LicenceToBill)
                    LicenceManager.PostUser(principal.KeyUser, principal.NameUser);

                    // if succeeded : redirect to demo
                    return this.RedirectToAction("Index", "Demo");
                }
            }

            // if failed, create a login model
            var model = new ModelLogin
                            {
                                Login = login,
                                Password = password
                            };

            // add a displayable error
            this.ModelState.AddModelError("common", "authentication failed");
            // render the view
            return View(model);
        }
        /// <summary>
        /// Impersonation page
        /// </summary>
        [Authenticated]
        public ActionResult Impersonate(string login)
        {
            // authenticate
            if (Authenticator.Authenticate(login))
            {
                // get the new principal
                var principal = CustomPrincipal.Current;
                // post the user's data (to LicenceToBill)
                LicenceManager.PostUser(principal.KeyUser, principal.NameUser);

                // if succeeded : redirect to demo
                return this.RedirectToAction("Index", "Demo");
            }

            // if authentication failed
            // sign out
            Authenticator.SignOut();
            // redirect to login
            return this.RedirectToAction("Login", "Home");
        }
        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns></returns>
        [Authenticated]
        public ActionResult SignOut()
        {
            // sign out
            FormsAuthentication.SignOut();
            // redirect to login
            return this.RedirectToAction("Login", "Home");
        }

        #endregion

        #region Ajax

        /// <summary>
        /// My profile - ajax version
        /// </summary>
        /// <returns></returns>
        [Authenticated]
        public PartialViewResult MyProfileAjax()
        {
            var model = new ModelMyProfile();

            // get the user's data (via LicenceToBill)
            var user = LicenceManager.GetUser(CustomPrincipal.Current.KeyUser);
            // if found
            if(user != null)
            {
                // push the urls
                model.UrlChooseOffer = user.UrlForChooseOffer;
                model.UrlDeals = user.UrlForDeals;
                model.UrlInvoices = user.UrlForInvoices;
            }

            return this.PartialView("MyProfile", model);
        }

        #endregion

        #region Error pages

        /// <summary>
        /// Error page : not implemented
        /// </summary>
        [HttpGet]
        public ActionResult NotImplemented()
        {
            return View();
        }

        
        #endregion
    }
}
