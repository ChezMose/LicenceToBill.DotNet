///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System;
using System.Runtime.Serialization;

namespace LicenceToBill.Api
{
    /// <summary>
    /// User
    /// </summary>
    [DataContract(Name="user")]
    public class UserV2
    {
        /// <summary>
        /// User key
        /// </summary>
        [DataMember(Name="key_user", IsRequired=true)]
        public string KeyUser { get; set; }
        /// <summary>
        /// User language
        /// </summary>
        [DataMember(Name="lcid_user", EmitDefaultValue=false)]
        public int? LcidUser { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        [DataMember(Name="name_user", EmitDefaultValue=false)]
        public string NameUser { get; set; }
        /// <summary>
        /// Limitation for this user (if the associated request needs it)
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="limitation")]
        public int? Limitation { get; set; }

        /// <summary>
        /// Start date of the working period
        /// </summary>
        [DataMember(Name="date_period_start", EmitDefaultValue=false)]
        public DateTime? DatePeriodStart { get; set; }
        /// <summary>
        /// End date of the working period
        /// </summary>
        [DataMember(Name="date_period_end", EmitDefaultValue=false)]
        public DateTime? DatePeriodEnd { get; set; }

        /// <summary>
        /// Url for the 'choose an offer' page
        /// </summary>
        [DataMember(Name="url_choose_offer", EmitDefaultValue=false)]
        public string UrlForChooseOffer { get; set; }
        /// <summary>
        /// Url for the 'deals' page
        /// </summary>
        [DataMember(Name="url_deals", EmitDefaultValue=false)]
        public string UrlForDeals { get; set; }
        /// <summary>
        /// Url for the 'invoices' page
        /// </summary>
        [DataMember(Name="url_invoices", EmitDefaultValue=false)]
        public string UrlForInvoices { get; set; }
    }
}
