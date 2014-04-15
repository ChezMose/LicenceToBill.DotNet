///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LicenceToBill.Api
{
    /// <summary>
    /// Offer
    /// </summary>
    [DataContract(Name="offer")]
    public class OfferV2
    {
        /// <summary>
        /// Offer key
        /// </summary>
        [DataMember(Name="key_offer", IsRequired=true)]
        public string KeyOffer { get; set; }

        /// <summary>
        /// Offer localized title
        /// </summary>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string TitleLocalized { get; set; }
        /// <summary>
        /// Offer localized description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string DescriptionLocalized { get; set; }
        /// <summary>
        /// Currency for this offer
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Url for the 'choose payment' page
        /// </summary>
        [DataMember(Name="url_choose_payment", EmitDefaultValue=false)]
        public string UrlForChoosePayment { get; set; }

        /// <summary>
        /// Stages in this offer
        /// </summary>
        [DataMember(Name="stages", EmitDefaultValue=false)]
        public List<StageV2> Stages { get; set; }
    }
}
