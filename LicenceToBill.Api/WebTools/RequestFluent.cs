///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;

namespace LicenceToBill.Api.Tools
{
    /// <summary>
    /// Fluent class used to build a request
    /// </summary>
    public class RequestFluent
    {
        #region Public Static - Create

        /// <summary>
        /// Create a new request
        /// </summary>
        public static RequestFluent Create(string url)
        {
            RequestFluent result = null;

            // if we got a valid url
            if(HelperUrl.IsUrlValid(url))
                // create a request
                result = new RequestFluent(url);
                // if url is not valid
            else
                // then we have a problem
                throw new ArgumentException(string.Format("Invalid uri '{0}'", url));

            return result;
        }

        #endregion

        #region Public Static - default configuration

        /// <summary>
        /// Default content type, if none is provided
        /// </summary>
        public static ContentType ContentTypeDefault
        {
            get
            {
                // if none at this point
                if(_ContentTypeDefault == null)
                    // instanciate
                    _ContentTypeDefault = new ContentType("application/json");

                return _ContentTypeDefault;
            }
            set
            {
                // if any given
                if(value != null)
                    // then use it as default
                    _ContentTypeDefault = value;
            }
        }
        /// <summary>
        /// Default content type, if none is provided
        /// </summary>
        public static Encoding EncodingDefault
        {
            get
            {
                // if none at this point
                if(_EncodingDefault == null)
                {
                    // instanciate default (UTF8 without BOM)
                    _EncodingDefault = new UTF8Encoding(false);
                }
                return _EncodingDefault;
            }
            set
            {
                // if any given
                if(value != null)
                    // then use it as default
                    _EncodingDefault = value;
            }
        }
        /// <summary>
        /// Set the default content type (string version)
        /// </summary>
        public static void SetDefaultContentType(string contentType)
        {
            // resolve the content type
            var type = ParseContentType(contentType);
            // if succeeded
            if(type != null)
            {
                // then use it as default
                _ContentTypeDefault = type;
            }
        }
        /// <summary>
        /// Set the basic authentication for all further requests (if not specified differently)
        /// </summary>
        public static void SetDefaultBasicAuthentication(string username, string password)
        {
            // if any
            if(!string.IsNullOrEmpty(username))
            {
                // if not instanciated
                if(_HeadersDefault == null)
                    // instanciate headers dictionary
                    _HeadersDefault = new Dictionary<string, string>();

                // concat login:password
                string authorization = username + ':' + (password ?? string.Empty);
                // encode
                authorization = Convert.ToBase64String(EncodingDefault.GetBytes(authorization));

                // set the authorization header
                _HeadersDefault["Authorization"] = "Basic " + authorization;
            }
        }
        /// <summary>
        /// Set a default header for all further requests (if not specified differently)
        /// </summary>
        public static void SetDefaultHeader(string name, string value)
        {
            // if any
            if(!string.IsNullOrEmpty(name))
            {
                // if not instanciated
                if(_HeadersDefault == null)
                    // instanciate headers dictionary
                    _HeadersDefault = new Dictionary<string, string>();

                // set the authorization header
                _HeadersDefault[name] = value;
            }
        }

        #endregion

        #region Private Static

        /// <summary>
        /// Default content type, if none is provided
        /// </summary>
        private static ContentType _ContentTypeDefault = null;
        /// <summary>
        /// Default encoding, if none is provided
        /// </summary>
        private static Encoding _EncodingDefault = null;
        /// <summary>
        /// Default headers
        /// </summary>
        private static Dictionary<string, string> _HeadersDefault = null;

        #endregion

        #region Public fluent - Common

        /// <summary>
        /// Set the HTTP method (or verb)
        /// </summary>
        public RequestFluent Method(HttpVerbs method)
        {
            // set the verb
            this._method = method.ToString();

            return this;
        }
        /// <summary>
        /// Set the HTTP verb (or method)
        /// </summary>
        public RequestFluent Verb(HttpVerbs verb)
        {
            // set the verb
            this._method = verb.ToString();

            return this;
        }
        /// <summary>
        /// Set the Content-Type
        /// </summary>
        public RequestFluent ContentType(string contentType)
        {
            // resolve the content type
            ContentType type = ParseContentType(contentType);
            // if succeeded
            if(type != null)
                // if succeeded, call the overload
                this.ContentType(type);

            return this;
        }
        /// <summary>
        /// Set the Content-Type
        /// </summary>
        public RequestFluent ContentType(ContentType contentType)
        {
            // store
            this._contentType = contentType;

            return this;
        }
        /// <summary>
        /// Set the Content-Type
        /// </summary>
        public RequestFluent ContentType(Encoding encoding)
        {
            // store
            this._encoding = encoding;

            return this;
        }
        /// <summary>
        /// Add an accepted content type
        /// </summary>
        public RequestFluent Accept(string contentType)
        {
            // parse
            var type = ParseContentType(contentType);
            // if succeeded
            if(type != null)
                // store the header
                this.Accept(type);

            return this;
        }
        /// <summary>
        /// Add an accepted content type
        /// </summary>
        public RequestFluent Accept(ContentType contentType)
        {
            // if we already have an accept header
            if(!string.IsNullOrEmpty(this._accept))
            {
                // if we have a content type
                if(contentType != null)
                    // concatenate
                    this._accept += ";" + contentType;
                // if no content type
                else
                    // clear
                    this._accept = null;
            }
            // if no accept header yet, but we have a given content type
            else if(contentType != null)
                // use it as accept header
                this._accept = contentType.ToString();

            return this;
        }
        /// <summary>
        /// Set the user agent
        /// </summary>
        public RequestFluent UserAgent(string userAgent)
        {
            this._userAgent = userAgent;

            return this;
        }

        #endregion

        #region Public fluent - Headers

        /// <summary>
        /// Set given header
        /// </summary>
        public RequestFluent Header(string name, string value)
        {
            // if we got a given name
            if(!string.IsNullOrEmpty(name))
            {
                // if no headers for now
                if (this._headers == null)
                    // instanciate
                    this._headers = new Dictionary<string, string>();

                // store
                this._headers[name] = value;
            }
            return this;
        }

        #endregion

        #region Public fluent - url

        /// <summary>
        /// Add the request content
        /// </summary>
        public RequestFluent SetUrlParameter(string parameter, string value)
        {
            this._uri = HelperUrl.SetParameter(this._uri, parameter, value);

            return this;
        }

        #endregion

        #region Public fluent - authentication

        /// <summary>
        /// Set the basic authentication
        /// </summary>
        public RequestFluent BasicAuthentication(string username, string password)
        {
            // if any
            if(!string.IsNullOrEmpty(username))
            {
                // concat login:password
                string authorization = username + ':' + (password ?? string.Empty);
                // encode
                authorization = Convert.ToBase64String(EncodingDefault.GetBytes(authorization));

                // set the authorization header
                return this.Header("Authorization", "Basic " + authorization);
            }

            return this;
        }

        #endregion

        #region Public - Sending

        /// <summary>
        /// Send the request, get the result as a string
        /// </summary>
        public ResponseEx Send(string content = null, int? timeout = null)
        {
            ResponseEx result = null;

            // prepare the request
            string errorMessage;
            var request = InnerPrepareRequest(null, timeout, out errorMessage);

            // if we have a request
            if((request != null)
                // and no error message
                && string.IsNullOrEmpty(errorMessage))
            {
                // if we got a content
                if (!string.IsNullOrEmpty(content))
                {
                    // we really need an encoding here
                    var encoding = (this._encoding ?? EncodingDefault);
                    // convert content as a byte array
                    var buffer = encoding.GetBytes(content);
                    // set length
                    request.ContentLength = buffer.Length;
                    // open a stream
                    using (var stream = request.GetRequestStream())
                    {
                        // push the byte array
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }

            // if we have a request
            if((request != null)
                // and no error message
                && string.IsNullOrEmpty(errorMessage))
            {
                // then send the request and get the response
                result = ResponseEx.SendRequest(request);
            }
            // if we had an error at some point
            else
            {
                // then this is a failure
                result = ResponseEx.Failure(this._uri, errorMessage);
            }
            return result;
        }
        /// <summary>
        /// Send the request, given content is serialized in JSON into the request body
        /// </summary>
        public ResponseEx SendJson<TContent>(TContent content = null, int? timeout = null)
            where TContent:class
        {
            ResponseEx result = null;

            // prepare the request and force JSON
            string errorMessage;
            var request = InnerPrepareRequest(new ContentType("application/json"), timeout, out errorMessage);

            // if we have a request
            if((request != null)
                // and no error message
                && string.IsNullOrEmpty(errorMessage))
            {
                // if we got a content
                if (content != null)
                {
                    // open the stream
                    using (var stream = request.GetRequestStream())
                    {
                        // serialize
                        if (!SerializerForJson.Serialize(content, stream))
                        {
                            // if serialization failed
                            errorMessage = "XML serialization failed";
                        }
                    }
                }
            }

            // if we have a request
            if((request != null)
                // and no error message
                && string.IsNullOrEmpty(errorMessage))
            {
                // then send the request and get the response
                result = ResponseEx.SendRequest(request);
            }
            // if we had an error at some point
            else
            {
                // then this is a failure
                result = ResponseEx.Failure(this._uri, errorMessage);
            }
            return result;
        }
        /// <summary>
        /// Send the request, given content is serialized in XML into the request body
        /// </summary>
        public ResponseEx SendXml<TContent>(TContent content = null, int? timeout = null)
            where TContent:class
        {
            ResponseEx result = null;

            // prepare the request and force XML
            string errorMessage;
            var request = InnerPrepareRequest(new ContentType("application/xml"), timeout, out errorMessage);

            // if we have a request
            if((request != null)
                // and no error message
                && string.IsNullOrEmpty(errorMessage))
            {
                // if we got a content
                if (content != null)
                {
                    // open the stream
                    using (var stream = request.GetRequestStream())
                    {
                        // serialize
                        if (!SerializerForXml.Serialize(content, stream))
                        {
                            // if serialization failed
                            errorMessage = "XML serialization failed";
                        }
                    }
                }
            }

            // if we have a request
            if((request != null)
                // and no error message
                && string.IsNullOrEmpty(errorMessage))
            {
                // then send the request and get the response
                result = ResponseEx.SendRequest(request);
            }
            // if we had an error at some point
            else
            {
                // then this is a failure
                result = ResponseEx.Failure(this._uri, errorMessage);
            }
            return result;
        }

        #endregion

        #region Private - HttpWebRequest preparation

        /// <summary>
        /// Prepare the request
        /// </summary>
        /// <param name="contentType">(optional) the content type, if forced</param>
        /// <param name="timeout">(optional) given request timeout</param>
        /// <param name="errorMessage">the error message, if request preparation failed</param>
        /// <returns>A valid HttpWebRequest, may be null</returns>
        private HttpWebRequest InnerPrepareRequest(ContentType contentType, int? timeout, out string errorMessage)
        {
            HttpWebRequest request = null;
            errorMessage = null;

            try
            {
                // create the request
                request = (WebRequest.Create(this._uri) as HttpWebRequest);
            }
            catch(Exception exc)
            {
                // we have an error
                errorMessage = "HttpRequest creation failed - " + exc.Message;
            }

            // if we have a request
            if((request != null)
                // and no error message
                && string.IsNullOrEmpty(errorMessage))
            {
                // if we got a timeout
                if (timeout.HasValue)
                    // set the value
                    request.Timeout = timeout.Value;

                // if we have a verb
                if(!string.IsNullOrEmpty(this._method))
                    // set it into the request
                    request.Method = this._method;

                // if we have headers
                if((this._headers != null)
                    && (this._headers.Count > 0))
                {
                    // loop
                    foreach (var header in this._headers.Keys)
                        // fill
                        request.Headers[header] = this._headers[header];
                }
                
                // if we have default headers
                if((_HeadersDefault != null)
                    && (_HeadersDefault.Count > 0))
                {
                    // loop
                    foreach(var header in _HeadersDefault.Keys)
                    {
                        // if we have no headers
                        if((this._headers == null)
                            || (this._headers.Count == 0)
                            // or this one was not added
                            || !this._headers.ContainsKey(header))
                            // add the header
                            request.Headers[header] = _HeadersDefault[header];
                    }
                }

                // create the content type
                // use forced content-type, if none, use specified content-type, if none, clone default content-type
                this._contentType = (contentType ?? this._contentType ?? new ContentType(ContentTypeDefault.ToString()));
                // if we have an encoding or a content type
                if (this._encoding != null)
                    // then set the encoding
                    this._contentType.CharSet = this._encoding.HeaderName;
                // now set the content-type in the request
                request.ContentType = this._contentType.ToString();

                // if we got an accept string
                if(!string.IsNullOrEmpty(this._accept))
                    // set it
                    request.Accept = this._accept;

                // if we got a user-agent
                if(this._userAgent != null)
                    // set it in the request
                    request.UserAgent = this._userAgent;
            }
            // if request creation failed and we have no error message (dunno if it can happend, but who knows)
            else if(string.IsNullOrEmpty(errorMessage))
            {
                // if serialization failed
                errorMessage = "HttpRequest creation failed, check the URL";
            }
            return request;
        }
        
        #endregion

        #region Private - Construction

        /// <summary>
        /// Base constructor
        /// </summary>
        private RequestFluent(string uri)
        {
            // create the request
            this._uri = uri;
        }

        #endregion

        #region Private - Inner data

        /// <summary>
        /// Request url
        /// </summary>
        private string _uri = null;
        /// <summary>
        /// HTTP Verb (method)
        /// </summary>
        private string _method = null;
        /// <summary>
        /// HTTP user agent
        /// </summary>
        private string _userAgent = null;
        /// <summary>
        /// Accept string
        /// </summary>
        private string _accept = null;
        /// <summary>
        /// Headers
        /// </summary>
        private Dictionary<string, string> _headers = null;
        /// <summary>
        /// Working content type
        /// </summary>
        private ContentType _contentType = null;
        /// <summary>
        /// Working encoding
        /// </summary>
        private Encoding _encoding = null;

        #endregion
        
        #region Private - Inner tools

        /// <summary>
        /// Resolve given string as a ContentType
        /// </summary>
        private static ContentType ParseContentType(string contentType)
        {
            ContentType result = null;

            // if input is valid
            if(!string.IsNullOrEmpty(contentType))
            {
                try
                {
                    // try to parse that content type
                    result = new ContentType(contentType);
                }
                catch
                {
                    // you may want want to plug your logger here to be aware of the problem
                }
            }
            return result;
        }

        #endregion
    }
}
