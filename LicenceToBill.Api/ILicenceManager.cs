using System.Collections.Generic;

using LicenceToBill.Api.Tools;

namespace LicenceToBill.Api
{
    /// <summary>
    /// LicenceManager
    /// 
    /// Class uses to interact with LicenceToBill API v2
    /// Uses JSON
    /// </summary>
    public interface ILicenceManager
    {
        #region Defensive version

        #region Users

        /// <summary>
        /// Try to list all users
        /// </summary>
        ResponseEx TryListUsers(out List<UserV2> users);

        /// <summary>
        /// Try to list all users
        /// </summary>
        ResponseEx TryGetUser(string keyUser, out UserV2 user);

        /// <summary>
        /// Post a user
        /// </summary>
        ResponseEx TryPostUser(string keyUser, string nameUser, int? lcid, out UserV2 user);

        /// <summary>
        /// List users having access to given feature
        /// </summary>
        ResponseEx TryListUsersByFeature(string keyFeature, out List<UserV2> users);

        #endregion

        #region Features

        /// <summary>
        /// List all features
        /// </summary>
        ResponseEx TryListFeatures(int? lcid, out List<FeatureV2> features);

        /// <summary>
        /// List features accessible to given user
        /// </summary>
        ResponseEx TryListFeaturesByUser(string keyUser, out List<FeatureV2> features);

        /// <summary>
        /// Get a limitation for a feature and a user
        /// </summary>
        ResponseEx TryGetLimitation(string keyFeature, string keyUser, out FeatureV2 feature);

        #endregion

        #region Offers

        /// <summary>
        /// List all features
        /// </summary>
        ResponseEx TryListOffers(int? lcid, out List<OfferV2> offers);

        /// <summary>
        /// List offers with user-specific url for given user
        /// </summary>
        ResponseEx TryListOffersByUser(string keyUser, out List<OfferV2> offers);

        #endregion

        #region Deals

        /// <summary>
        /// List all features
        /// </summary>
        ResponseEx TryListDealsByUser(string keyUser, out List<DealV2> deals);

        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        ResponseEx TryPostTrial(string keyOffer, string keyUser, string nameUser, int? lcid, out List<FeatureV2> features);

        #endregion

        #endregion

        #region Offensive version

        #region Users

        /// <summary>
        /// List all users
        /// </summary>
        List<UserV2> ListUsers();

        /// <summary>
        /// Get a user
        /// </summary>
        UserV2 GetUser(string keyUser);

        /// <summary>
        /// Post a user
        /// </summary>
        UserV2 PostUser(string keyUser, string nameUser, int? lcid = null);

        /// <summary>
        /// List users having access to given feature
        /// </summary>
        List<UserV2> ListUsersByFeature(string keyFeature);

        #endregion

        #region Features

        /// <summary>
        /// List all features
        /// </summary>
        List<FeatureV2> ListFeatures(int? lcid = null);

        /// <summary>
        /// List features accessible to given user
        /// </summary>
        List<FeatureV2> ListFeaturesByUser(string keyUser);

        /// <summary>
        /// Get a limitation for a feature and a user
        /// </summary>
        FeatureV2 GetLimitation(string keyFeature, string keyUser);
        
        #endregion

        #region Offers

        /// <summary>
        /// List all offers
        /// </summary>
        List<OfferV2> ListOffers(int? lcid = null);

        /// <summary>
        /// List offers with user-specific url for given user
        /// </summary>
        List<OfferV2> ListOffersByUser(string keyUser);

        #endregion

        #region Deals

        /// <summary>
        /// List deals subscribed by given user
        /// </summary>
        List<DealV2> ListDealsByUser(string keyUser);

        /// <summary>
        /// - Post a user
        /// - Enable a free trial
        /// - returns its available features
        /// </summary>
        List<FeatureV2> PostTrial(string keyOffer, string keyUser, string nameUser, int? lcid = null);

        #endregion

        #endregion
    }
}
