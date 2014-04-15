///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Web.Mvc;

using LicenceToBill.Api.Tools;

namespace LicenceToBill.Api
{
    /// <summary>
    /// LicenceManager 
    /// Uses JSON
    /// 
    /// Offensive methods :
    /// assumes everything is ok and throws an exception if a problem occures
    /// </summary>
    partial class LicenceManagerDefault
    {
        #region Users

        /// <summary>
        /// List all users
        /// </summary>
        public List<UserV2> ListUsers()
        {
            List<UserV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.Users);

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<UserV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }
        /// <summary>
        /// Get a user
        /// </summary>
        public UserV2 GetUser(string keyUser)
        {
            UserV2 result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.UserByKey, keyUser);

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<UserV2>();

            // manage failures
            ManageFailures(response);

            return result;
        }
        /// <summary>
        /// Post a user
        /// </summary>
        public UserV2 PostUser(string keyUser, string nameUser, int? lcid = null)
        {
            UserV2 result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.UserByKey, keyUser);

            // build the user to send
            var userToSend = new UserV2
                                 {
                                   KeyUser = keyUser,
                                   NameUser = nameUser,
                                   LcidUser = (lcid ?? LtbConstants.LcidDefault)
                                 };

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Method(HttpVerbs.Post)
                .SendJson(userToSend);

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<UserV2>();

            // manage failures
            ManageFailures(response);

            return result;
        }
        /// <summary>
        /// List users having access to given feature
        /// </summary>
        public List<UserV2> ListUsersByFeature(string keyFeature)
        {
            List<UserV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.UsersByFeature, keyFeature);

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<UserV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }

        #endregion

        #region Features

        /// <summary>
        /// List all features
        /// </summary>
        public List<FeatureV2> ListFeatures(int? lcid = null)
        {
            List<FeatureV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.FeaturesByLcid, (lcid ?? LtbConstants.LcidDefault));
             
            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<FeatureV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }

        /// <summary>
        /// List features accessible to given user
        /// </summary>
        public List<FeatureV2> ListFeaturesByUser(string keyUser)
        {
            List<FeatureV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.FeaturesByUser, keyUser);

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<FeatureV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }

        /// <summary>
        /// Get a limitation for a feature and a user
        /// </summary>
        public FeatureV2 GetLimitation(string keyFeature, string keyUser)
        {
            FeatureV2 result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.LimitationByUserByFeature, keyUser, keyFeature);

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<FeatureV2>();

            // manage failures
            ManageFailures(response);

            return result;
        }
        
        #endregion

        #region Offers

        /// <summary>
        /// List all offers
        /// </summary>
        public List<OfferV2> ListOffers(int? lcid = null)
        {
            List<OfferV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.OffersByLcid, (lcid ?? LtbConstants.LcidDefault));

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<OfferV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }
        
        /// <summary>
        /// List offers with user-specific url for given user
        /// </summary>
        public List<OfferV2> ListOffersByUser(string keyUser)
        {
            List<OfferV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.OffersByUser, keyUser);

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<OfferV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }

        #endregion

        #region Deals

        /// <summary>
        /// List deals subscribed by given user
        /// </summary>
        public List<DealV2> ListDealsByUser(string keyUser)
        {
            List<DealV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.DealsByUser, keyUser);

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Send();

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<DealV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }

        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        public List<FeatureV2> PostTrial(string keyOffer, string keyUser, string nameUser, int? lcid=null)
        {
            List<FeatureV2> result = null;

            // build the url
            var url = LtbUrl.Build(LtbResource.Trial, keyUser);
                        
            // build the deal to send
            var dealToSend = new DealV2
                                 {
                                     KeyOffer = keyOffer,
                                     KeyUser = keyUser,
                                     NameUser = nameUser,
                                     LcidUser = (lcid ?? LtbConstants.LcidDefault)
                                 };

            // create the request and send it
            var response = RequestFluent.Create(url)
                .ContentType(LtbConstants.Json)
                .BasicAuthentication(LtbConstants.KeyBusiness, LtbConstants.KeyApi)
                .Method(HttpVerbs.Post)
                .SendJson(dealToSend);

            // if succeeded
            if(response.HasBody)
                // return that content
                result = response.GetBodyAsJson<List<FeatureV2>>();

            // manage failures
            ManageFailures(response);

            return result;
        }

        #endregion

        #region Inner tools

        /// <summary>
        /// Manage failures for given response
        /// </summary>
        private void ManageFailures(ResponseEx response)
        {
            // if we got a response
            if(response != null)
            {
                // if we have a failure
                if(!response.IsSuccessful)
                {
                    // get request url
                    string requestUrl = response.UrlRequest;
                    // if none specified
                    if (string.IsNullOrEmpty(requestUrl))
                        // set default
                        requestUrl = "(unspecified)";

                    // get response error message
                    string errorMessage = response.MessageError;
                    // if none specified
                    if (string.IsNullOrEmpty(errorMessage))
                        // set default
                        errorMessage = "(no message)";

                    // get response body as a string
                    string responseBody = response.GetBodyAsString();
                    // if none
                    if (string.IsNullOrEmpty(responseBody))
                        // set default
                        responseBody = "(no response body)";

                    // throw an exception
                    throw new ApplicationException(string.Format("LicenceManager error [Http status: {0} - {1}] [Url: {2}] [Error message: {3}] [Response body: {4}]", (int) response.StatusHttp, response.StatusHttp, requestUrl, errorMessage, responseBody));
                }
            }
            // if no response
            else
                // throw an exception
                throw new ApplicationException("No response ");
        }

        #endregion
    }
}