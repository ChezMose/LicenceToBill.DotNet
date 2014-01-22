using System;
using System.Net;
using System.Web.Mvc;

namespace LicenceToBill.Web.DemoRest
{
    /// <summary>
    /// Model for page 'test paybox'
    /// </summary>
    public class ModelPayboxTest
    {
        /// <summary>
        /// Url type
        /// </summary>
        public SelectList Types
        {
            get { return new SelectList(Enum.GetValues(typeof (PayboxController.TypeUrlPaybox))); }
        }
        /// <summary>
        /// Url type
        /// </summary>
        public PayboxController.TypeUrlPaybox? Type { get; set; }

        /// <summary>
        /// Url sent to
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Content to send
        /// </summary>
        public string BodyRequest { get; set; }

        /// <summary>
        /// Returned status
        /// </summary>
        public HttpStatusCode? Status { get; set; }
        /// <summary>
        /// Content received
        /// </summary>
        public string BodyResponse { get; set; }

        /// <summary>
        /// Elapsed time
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// Outgoing ip address
        /// </summary>
        public string IpOutgoing { get; set; }
    }
}