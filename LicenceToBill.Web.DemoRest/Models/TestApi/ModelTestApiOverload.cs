using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace LicenceToBill.Web.DemoRest
{
    /// <summary>
    /// Model for page 'test api overload'
    /// </summary>
    public class ModelTestApiOverload
    {
        /// <summary>
        /// Host url
        /// </summary>
        public string UrlHost { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Total time
        /// </summary>
        public int TimeTotal { get; set; }

        /// <summary>
        /// Target url
        /// </summary>
        public List<Call> Calls { get; set; }

        /// <summary>
        /// Call
        /// </summary>
        public class Call
        {
            /// <summary>
            /// Call start date
            /// </summary>
            public DateTime DateStart { get; set; }
            /// <summary>
            /// Returned status
            /// </summary>
            public HttpStatusCode StatusCode { get; set; }
            /// <summary>
            /// Call duration
            /// </summary>
            public long ElapsedMilliseconds { get; set; }
            /// <summary>
            /// Test type
            /// </summary>
            public TypeTestApi Type { get; set; }
        }
        /// <summary>
        /// Test type
        /// </summary>
        public enum TypeTestApi
        {
            Read,
            Write
        }
        /// <summary>
        /// Test type
        /// </summary>
        public enum TypeTest
        {
            Small,
            Micro,
            Medium,
            Large,
            Huge,
            RandomSmall,
            RandomMedium,
            RandomLarge,
        }
    }
}