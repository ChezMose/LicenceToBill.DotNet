///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System.Configuration;

namespace LicenceToBill.Api.Tools
{
    /// <summary>
    /// Constants used by LTB
    /// </summary>
    public static class LtbConstants
    {
        /// <summary>
        /// LicenceToBill API URL
        /// </summary>
        public static readonly string UrlApi = ConfigurationManager.AppSettings["LicenceToBillUrlApi"];
        /// <summary>
        /// LicenceToBill URL
        /// </summary>
        public static readonly string KeyApi = ConfigurationManager.AppSettings["LicenceToBillKeyApi"];

        /// <summary>
        /// Working business
        /// </summary>
        public static readonly string KeyBusiness = ConfigurationManager.AppSettings["LicenceToBillKeyBusiness"];
        /// <summary>
        /// Default language
        /// (9 for english)
        /// </summary>
        public static readonly int LcidDefault = 9;

        /// <summary>
        /// Working business
        /// </summary>
        public static readonly string Json = "application/json";
        
    }
}