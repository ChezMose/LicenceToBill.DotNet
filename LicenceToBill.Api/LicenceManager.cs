///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System.Collections.Generic;
using LicenceToBill.Api.Tools;

namespace LicenceToBill.Api
{
    /// <summary>
    /// LicenceManager
    /// Static version
    /// Uses JSON
    /// 
    /// Common tools
    /// </summary>
    public static partial class LicenceManager
    {
        #region Singleton

        /// <summary>
        /// Singleton
        /// </summary>
        private static ILicenceManager _Current = null;
        /// <summary>
        /// Singleton
        /// </summary>
        public static ILicenceManager Current
        {
            get
            {
                if(_Current == null)
                    _Current = new LicenceManagerDefault();
                return _Current;
            }
        }

        #endregion

        #region Defensive version

        #region Users

        /// <summary>
        /// Try to list all users
        /// </summary>
        public static ResponseEx TryListUsers(out List<UserV2> users)
        {
            return Current.TryListUsers(out users);
        }
        /// <summary>
        /// Try to list all users
        /// </summary>
        public static ResponseEx TryGetUser(string keyUser, out UserV2 user)
        {
            return Current.TryGetUser(keyUser, out user);
        }
        /// <summary>
        /// Post a user
        /// </summary>
        public static ResponseEx TryPostUser(string keyUser, string nameUser, int? lcid, out UserV2 user)
        {
            return Current.TryPostUser(keyUser, nameUser, lcid, out user);
        }
        /// <summary>
        /// List users having access to given feature
        /// </summary>
        public static ResponseEx TryListUsersByFeature(string keyFeature, out List<UserV2> users)
        {
            return Current.TryListUsersByFeature(keyFeature, out users);
        }

        #endregion

        #region Features

        /// <summary>
        /// List all features
        /// </summary>
        public static ResponseEx TryListFeatures(int? lcid, out List<FeatureV2> features)
        {
            return Current.TryListFeatures(lcid, out features);
        }
        /// <summary>
        /// List features accessible to given user
        /// </summary>
        public static ResponseEx TryListFeaturesByUser(string keyUser, out List<FeatureV2> features)
        {
            return Current.TryListFeaturesByUser(keyUser, out features);
        }
        /// <summary>
        /// Get a limitation for a feature and a user
        /// </summary>
        public static ResponseEx TryGetLimitation(string keyFeature, string keyUser, out FeatureV2 feature)
        {
            return Current.TryGetLimitation(keyFeature, keyUser, out feature);
        }

        #endregion

        #region Offers

        /// <summary>
        /// List all features
        /// </summary>
        public static ResponseEx TryListOffers(int? lcid, out List<OfferV2> offers)
        {
            return Current.TryListOffers(lcid, out offers);
        }
        /// <summary>
        /// List offers with user-specific url for given user
        /// </summary>
        public static ResponseEx TryListOffersByUser(string keyUser, out List<OfferV2> offers)
        {
            return Current.TryListOffersByUser(keyUser, out offers);
        }

        #endregion

        #region Deals

        /// <summary>
        /// List all features
        /// </summary>
        public static ResponseEx TryListDealsByUser(string keyUser, out List<DealV2> deals)
        {
            return Current.TryListDealsByUser(keyUser, out deals);
        }
        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        public static ResponseEx TryPostTrial(string keyOffer, string keyUser, string nameUser, int? lcid, out List<FeatureV2> features)
        {
            return Current.TryPostTrial(keyOffer, keyUser, nameUser, lcid, out features);
        }

        #endregion

        #endregion

        #region Offensive version

        #region Users

        /// <summary>
        /// List all users
        /// </summary>
        public static List<UserV2> ListUsers()
        {
            return Current.ListUsers();
        }
        /// <summary>
        /// Get a user
        /// </summary>
        public static UserV2 GetUser(string keyUser)
        {
            return Current.GetUser(keyUser);
        }
        /// <summary>
        /// Post a user
        /// </summary>
        public static UserV2 PostUser(string keyUser, string nameUser, int? lcid = null)
        {
            return Current.PostUser(keyUser, nameUser, lcid);
        }
        /// <summary>
        /// List users having access to given feature
        /// </summary>
        public static List<UserV2> ListUsersByFeature(string keyFeature)
        {
            return Current.ListUsersByFeature(keyFeature);
        }

        #endregion

        #region Features

        /// <summary>
        /// List all features
        /// </summary>
        public static List<FeatureV2> ListFeatures(int? lcid = null)
        {
            return Current.ListFeatures(lcid);
        }

        /// <summary>
        /// List features accessible to given user
        /// </summary>
        public static List<FeatureV2> ListFeaturesByUser(string keyUser)
        {
            return Current.ListFeaturesByUser(keyUser);
        }

        /// <summary>
        /// Get a limitation for a feature and a user
        /// </summary>
        public static FeatureV2 GetLimitation(string keyFeature, string keyUser)
        {
            return Current.GetLimitation(keyFeature, keyUser);
        }
        
        #endregion

        #region Offers

        /// <summary>
        /// List all offers
        /// </summary>
        public static List<OfferV2> ListOffers(int? lcid = null)
        {
            return Current.ListOffers(lcid);
        }
        
        /// <summary>
        /// List offers with user-specific url for given user
        /// </summary>
        public static List<OfferV2> ListOffersByUser(string keyUser)
        {
            return Current.ListOffersByUser(keyUser);
        }

        #endregion

        #region Deals

        /// <summary>
        /// List deals subscribed by given user
        /// </summary>
        public static List<DealV2> ListDealsByUser(string keyUser)
        {
            return Current.ListDealsByUser(keyUser);
        }

        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        public static List<FeatureV2> PostTrial(string keyOffer, string keyUser, string nameUser, int? lcid=null)
        {
            return Current.PostTrial(keyOffer, keyUser, nameUser, lcid);
        }

        #endregion

        #endregion
    }
}