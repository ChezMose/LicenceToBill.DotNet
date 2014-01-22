using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace LicenceToBill.Web.DemoRest
{
    /// <summary>
    /// Model for page 'test paybox'
    /// </summary>
    public class ModelPayboxTraceroute
    {
        /// <summary>
        /// IP or host to trace
        /// </summary>
        public string IpOrHost { get; set; }
        /// <summary>
        /// Global timeout
        /// </summary>
        public int? Timeout { get; set; }
        /// <summary>
        /// Max hops to destination
        /// </summary>
        public int? MaxHops { get; set; }

        /// <summary>
        /// Returned status
        /// </summary>
        public List<string> Status { get; set; }
    }
}