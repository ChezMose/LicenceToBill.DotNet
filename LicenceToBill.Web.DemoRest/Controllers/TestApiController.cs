using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

using LicenceToBill.Api;
using LicenceToBill.Api.Tools;

namespace LicenceToBill.Web.DemoRest
{
    [Authenticated]
    public partial class TestApiController : Controller
    {
        #region Actions

        /// <summary>
        /// Test API : home
        /// </summary>
        public ActionResult Index()
        {
            return this.View();
        }

        #endregion

        #region Ressource - Users

        /// <summary>
        /// Users list
        /// </summary>
        [HttpGet]
        public ActionResult Users()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.Users);
            // set available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // display the view
            return this.View("Call", model);
        }
        [HttpPost]
        public PartialViewResult Users(HttpVerbs? method, int? page, int? sizePage)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                // perform the call
                List<UserV2> users;
                var response = LicenceManager.TryListUsers(out users);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Users = users;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }

        #endregion

        #region Ressource - UserByKey

        /// <summary>
        /// Given user
        /// </summary>
        [HttpGet]
        public ActionResult UserByKey()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.UserByKey, "{key_user}");
            // set available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get, HttpVerbs.Post});

            // list available keys
            model.KeyUsers = this.ListKeyUsers();
            // list available languages
            model.Lcids = this.ListLcids();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Given user
        /// </summary>
        [HttpPost]
        public PartialViewResult UserByKey(HttpVerbs? method, string keyUser, int? lcid)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                // perform the call
                UserV2 user;
                var response = LicenceManager.TryGetUser(keyUser, out user);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.User = user;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
            // if 'Post'
            else if(method == HttpVerbs.Post)
            {
                // get user's name
                var nameUser =
                    (from item in this.ListKeyUsers()

                     where item.Item2 == keyUser

                     select item.Item1
                    ).FirstOrDefault();

                // start the timer
                var timer = Stopwatch.StartNew();

                // perform the call
                UserV2 user;
                var response = LicenceManager.TryPostUser(keyUser, nameUser, lcid, out user);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.User = user;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion

        #region Ressource - UsersByFeature

        /// <summary>
        /// Given user
        /// </summary>
        [HttpGet]
        public ActionResult UsersByFeature()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.UsersByFeature, "{key_feature}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // list available keys
            model.KeyFeatures = this.ListKeyFeatures();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Given user
        /// </summary>
        [HttpPost]
        public PartialViewResult UsersByFeature(HttpVerbs? method, string keyFeature)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                // perform the call
                List<UserV2> users;
                var response = LicenceManager.TryListUsersByFeature(keyFeature, out users);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Users = users;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion

        #region Ressource - FeaturesByLcid

        /// <summary>
        /// Features localized
        /// </summary>
        [HttpGet]
        public ActionResult FeaturesByLcid()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.FeaturesByLcid, "{lcid}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // list available languages
            model.Lcids = this.ListLcids();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Features localized
        /// </summary>
        [HttpPost]
        public PartialViewResult FeaturesByLcid(HttpVerbs? method, int? lcid)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
            // start the timer
            var timer = Stopwatch.StartNew();

                // perform the call
                List<FeatureV2> features;
                var response = LicenceManager.TryListFeatures(lcid, out features);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Features = features;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion
        
        #region Ressource - FeaturesByUser

        /// <summary>
        /// Features by user
        /// </summary>
        [HttpGet]
        public ActionResult FeaturesByUser()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.FeaturesByUser, "{key_user}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // list available keys
            model.KeyUsers = this.ListKeyUsers();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Features by user
        /// </summary>
        [HttpPost]
        public PartialViewResult FeaturesByUser(HttpVerbs? method, string keyUser)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                    // perform the call
                List<FeatureV2> features;
                var response = LicenceManager.TryListFeaturesByUser(keyUser, out features);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Features = features;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion
        
        #region Ressource - FeaturesByUser

        /// <summary>
        /// Features by user
        /// </summary>
        [HttpGet]
        public ActionResult LimitationByFeatureByUser()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.LimitationByUserByFeature, "{key_user}", "{key_feature}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // list available keys
            model.KeyUsers = this.ListKeyUsers();
            model.KeyFeatures = this.ListKeyFeatures();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Features by user
        /// </summary>
        [HttpPost]
        public PartialViewResult LimitationByFeatureByUser(HttpVerbs? method, string keyUser, string keyFeature)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                // perform the call
                FeatureV2 feature;
                var response = LicenceManager.TryGetLimitation(keyFeature, keyUser, out feature);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Feature = feature;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion

        #region Ressource - OffersByLcid

        /// <summary>
        /// Offers localized
        /// </summary>
        [HttpGet]
        public ActionResult OffersByLcid()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.OffersByLcid, "{lcid}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // list available languages
            model.Lcids = this.ListLcids();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Offers localized
        /// </summary>
        [HttpPost]
        public PartialViewResult OffersByLcid(HttpVerbs? method, int? lcid)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
            // start the timer
            var timer = Stopwatch.StartNew();

                // perform the call
                List<OfferV2> offers;
                var response = LicenceManager.TryListOffers(lcid, out offers);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Offers = offers;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion

        #region Ressource - OffersByUser

        /// <summary>
        /// Offers by user
        /// </summary>
        [HttpGet]
        public ActionResult OffersByUser()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.OffersByUser, "{key_user}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // list available keys
            model.KeyUsers = this.ListKeyUsers();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Offers by user
        /// </summary>
        [HttpPost]
        public PartialViewResult OffersByUser(HttpVerbs? method, string keyUser)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                // perform the call
                List<OfferV2> offers;
                var response= LicenceManager.TryListOffersByUser(keyUser, out offers);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Offers = offers;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion

        #region Ressource - DealsByUser

        /// <summary>
        /// Deals by user
        /// </summary>
        [HttpGet]
        public ActionResult DealsByUser()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.DealsByUser, "{key_user}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Get});

            // list available keys
            model.KeyUsers = this.ListKeyUsers();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// Deals by user
        /// </summary>
        [HttpPost]
        public PartialViewResult DealsByUser(HttpVerbs? method, string keyUser)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Get)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                // perform the call
                List<DealV2> deals;
                var response = LicenceManager.TryListDealsByUser(keyUser, out deals);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Deals = deals;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion

        #region Ressource - Trial

        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        [HttpGet]
        public ActionResult Trial()
        {
            var model = new ModelTestApiCall();

            // build the url
            model.UrlRessource = LtbUrl.Build(LtbResource.Trial, "{key_user}");
            // list available methods
            model.Methods = new List<HttpVerbs>(new [] {HttpVerbs.Post});

            // list available keys
            model.KeyUsers = this.ListKeyUsers();
            // list available offers
            model.KeyOffers = this.ListKeyOffers();
            // list languages
            model.Lcids = this.ListLcids();

            // display the view
            return this.View("Call", model);
        }

        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        [HttpPost]
        public PartialViewResult Trial(HttpVerbs? method, string keyOffer, string keyUser, int? lcid)
        {
            var model = new ModelTestApiResult();

            // if 'Get'
            if(method == HttpVerbs.Post)
            {
                // start the timer
                var timer = Stopwatch.StartNew();

                // get user's name
                var nameUser =
                    (from item in this.ListKeyUsers()

                        where item.Item2 == keyUser

                        select item.Item1
                    ).FirstOrDefault();

                // perform the call
                List<FeatureV2> features;
                var response = LicenceManager.TryPostTrial(keyOffer, keyUser, nameUser, lcid, out features);

                // stop the timer
                timer.Stop();
                model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                // get the content
                model.Features = features;
                // get the code
                model.ResponseBody = response.GetBodyAsString();
                // get the exception message
                model.Message = response.MessageError;
                // get the status code
                model.StatusCode = response.StatusHttp;
            }
                // if method is not supported
            else
                model.Message = "Method not supported";

            return this.PartialView("Result", model);
        }
        
        #endregion

        #region Inner tools

        /// <summary>
        /// list available users
        /// </summary>
        private List<Tuple<string, string>> ListKeyUsers()
        {
            return
                (from item in Authenticator.Identities

                 orderby item.Key

                 select new Tuple<string, string>(item.Key, item.Value)
                ).ToList();

        }
        /// <summary>
        /// list available features
        /// </summary>
        private List<Tuple<string, string>> ListKeyFeatures()
        {
            // list features
            var features = LicenceManager.ListFeatures();

            return
                (from item in features

                 orderby item.TitleLocalized

                 select new Tuple<string, string>(item.TitleLocalized, item.KeyFeature)
                ).ToList();
        }
        /// <summary>
        /// list available offers
        /// </summary>
        private List<Tuple<string, string>> ListKeyOffers()
        {
            // list features
            var features = LicenceManager.ListOffers();

            return
                (from item in features

                 orderby item.TitleLocalized

                 select new Tuple<string, string>(item.TitleLocalized, item.KeyOffer)
                ).ToList();
        }
        /// <summary>
        /// list misc languages
        /// </summary>
        private List<Tuple<string, int>> ListLcids()
        {
            var result = new List<Tuple<string, int>>();

            result.Add(new Tuple<string, int>("English", 9));
            result.Add(new Tuple<string, int>("Dutch", 19));
            result.Add(new Tuple<string, int>("French", 12));
            result.Add(new Tuple<string, int>("German", 7));
            result.Add(new Tuple<string, int>("Italian", 16));
            result.Add(new Tuple<string, int>("Portuguese", 22));
            result.Add(new Tuple<string, int>("Spanish", 10));
            result.Add(new Tuple<string, int>("Swedish", 29));


            return result;
        }

        #endregion
    }
}
