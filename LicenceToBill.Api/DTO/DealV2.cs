///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LicenceToBill.Api
{
    /// <summary>
    /// Deal
    /// </summary>
    [DataContract(Name="deal")]
    public class DealV2
    {
        /// <summary>
        /// deal key
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="key_deal")]
        public string KeyDeal { get; set; }
        /// <summary>
        /// Associated offer key
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="key_offer")]
        public string KeyOffer { get; set; }
        /// <summary>
        /// User owner key
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="key_user")]
        public string KeyUser { get; set; }

        /// <summary>
        /// Deal title
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="title")]
        public string Title { get; set; }
        /// <summary>
        /// Deal description
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="description")]
        public string Description { get; set; }
        /// <summary>
        /// Deal currency
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Start date of the deal
        /// </summary>
        [DataMember(Name="date_start", EmitDefaultValue=false)]
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Renewal date of the deal, if renewal is programmed
        /// </summary>
        [DataMember(Name="date_renewal", EmitDefaultValue=false)]
        public DateTime? DateRenewal { get; set; }
        /// <summary>
        /// End date of the deal, if finite
        /// </summary>
        [DataMember(Name="date_end", EmitDefaultValue=false)]
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Url for the 'deal detail' page
        /// </summary>
        [DataMember(Name="url_deal_detail", EmitDefaultValue=false)]
        public string UrlForDeal { get; set; }

        /// <summary>
        /// Payment type
        /// </summary>
        [DataMember(Name="type_payment", EmitDefaultValue=false)]
        public string TypePayment { get; set; }
        /// <summary>
        /// Deal status
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="status")]
        public string Status { get; set; }

        /// <summary>
        /// Stages in this offer
        /// </summary>
        [DataMember(EmitDefaultValue=false, Name="stages")]
        public List<StageV2> Stages { get; set; }

        /// <summary>
        /// User language (used for POST user + deal)
        /// </summary>
        [DataMember(Name="lcid_user", EmitDefaultValue=false)]
        public int? LcidUser { get; set; }
        /// <summary>
        /// User name (used for POST user + deal)
        /// </summary>
        [DataMember(Name="name_user", EmitDefaultValue=false)]
        public string NameUser { get; set; }
    }
}
