﻿///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Web.Mvc;

using LicenceToBill.Api.Tools;

namespace LicenceToBill.Api
{
    /// <summary>
    /// LicenceManager
    /// Uses JSON
    /// 
    /// Defensive methods :
    /// assumes everything is not ok, performs all checks before processing and never throws an exception
    /// </summary>
    partial class LicenceManager
    {
        #region Users

        /// <summary>
        /// Try to list all users
        /// </summary>
        public static ResponseEx TryListUsers(out List<UserV2> users)
        {
            ResponseEx result = null;
            users = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.Users);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    users = result.GetBodyAsJson<List<UserV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }
        /// <summary>
        /// Try to list all users
        /// </summary>
        public static ResponseEx TryGetUser(string keyUser, out UserV2 user)
        {
            ResponseEx result = null;
            user = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.UserByKey, keyUser);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    user = result.GetBodyAsJson<UserV2>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }
        /// <summary>
        /// Post a user
        /// </summary>
        public static ResponseEx TryPostUser(string keyUser, string nameUser, int? lcid, out UserV2 user)
        {
            ResponseEx result = null;
            user = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.UserByKey, keyUser);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // build the user to send
                var userToSend = new UserV2
                                     {
                                       KeyUser = keyUser,
                                       NameUser = nameUser,
                                       Lcid = (lcid ?? LtbConstants.LcidDefault)
                                     };

                // create the request and send it
                result = RequestFluent.Create(url)
                    .Method(HttpVerbs.Post)
                    .SendJson(userToSend);

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    user = result.GetBodyAsJson<UserV2>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }
        /// <summary>
        /// List users having access to given feature
        /// </summary>
        public static ResponseEx TryListUsersByFeature(string keyFeature, out List<UserV2> users)
        {
            ResponseEx result = null;
            users = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.UsersByFeature, keyFeature);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    users = result.GetBodyAsJson<List<UserV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }

        #endregion

        #region Features

        /// <summary>
        /// List all features
        /// </summary>
        public static ResponseEx TryListFeatures(int? lcid, out List<FeatureV2> features)
        {
            ResponseEx result = null;
            features = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.FeaturesByLcid, lcid);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    features = result.GetBodyAsJson<List<FeatureV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }
        /// <summary>
        /// List features accessible to given user
        /// </summary>
        public static ResponseEx TryListFeaturesByUser(string keyUser, out List<FeatureV2> features)
        {
            ResponseEx result = null;
            features = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.FeaturesByUser, keyUser);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    features = result.GetBodyAsJson<List<FeatureV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }
        /// <summary>
        /// Get a limitation for a feature and a user
        /// </summary>
        public static ResponseEx TryGetLimitation(string keyFeature, string keyUser, out FeatureV2 feature)
        {
            ResponseEx result = null;
            feature = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.LimitationByUserByFeature, keyUser, keyFeature);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    feature = result.GetBodyAsJson<FeatureV2>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }

        #endregion

        #region Offers

        /// <summary>
        /// List all features
        /// </summary>
        public static ResponseEx TryListOffers(int? lcid, out List<OfferV2> offers)
        {
            ResponseEx result = null;
            offers = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.OffersByLcid, lcid);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    offers = result.GetBodyAsJson<List<OfferV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }
        /// <summary>
        /// List offers with user-specific url for given user
        /// </summary>
        public static ResponseEx TryListOffersByUser(string keyUser, out List<OfferV2> offers)
        {
            ResponseEx result = null;
            offers = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.OffersByUser, keyUser);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    offers = result.GetBodyAsJson<List<OfferV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }

        #endregion

        #region Deals

        /// <summary>
        /// List all features
        /// </summary>
        public static ResponseEx TryListDealsByUser(string keyUser, out List<DealV2> deals)
        {
            ResponseEx result = null;
            deals = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.DealsByUser, keyUser);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // create the request and send it
                result = RequestFluent.Create(url)
                    .Send();

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    deals = result.GetBodyAsJson<List<DealV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }
        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        public static ResponseEx TryPostTrial(string keyOffer, string keyUser, string nameUser, int? lcid, out List<FeatureV2> features)
        {
            ResponseEx result = null;
            features = null;

            // build the url
            string messageError;
            var url = LtbUrl.TryBuild(out messageError, LtbResource.Trial, keyUser);

            // if succeeded
            if(string.IsNullOrEmpty(messageError))
            {
                // build the deal to send
                var dealToSend = new DealV2
                                     {
                                         KeyOffer = keyOffer,
                                         KeyUser = keyUser,
                                         NameUser = nameUser,
                                         LcidUser = (lcid ?? LtbConstants.LcidDefault)
                                     };

                // create the request and send it
                result = RequestFluent.Create(url)
                    .Method(HttpVerbs.Post)
                    .SendJson(dealToSend);

                // if we have a response
                if((result != null)
                    // with a body
                    && (result.HasBody))
                {
                    // get the content from the response body
                    features = result.GetBodyAsJson<List<FeatureV2>>();
                }
            }
            // if url building failed
            else
                result = ResponseEx.Failure(url, messageError);

            return result;
        }

        #endregion
    }
}