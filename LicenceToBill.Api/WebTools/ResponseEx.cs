///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Net;

namespace LicenceToBill.Api.Tools
{
    /// <summary>
    /// HTTP response extended
    /// </summary>
    public class ResponseEx
    {
        #region Static

        /// <summary>
        /// Create an empty response
        /// Used for error handling
        /// </summary>
        public static ResponseEx Failure(string requestUrl, string errorMessage)
        {
            return new ResponseEx(requestUrl, null, null, errorMessage);
        }

        /// <summary>
        /// Send given request and get its response
        /// </summary>
        public static ResponseEx SendRequest(HttpWebRequest request)
        {
            ResponseEx result = null;

            // if we got a request
            if(request != null)
            {
                HttpWebResponse response = null;
                MemoryStream memStream = null;
                string errorMessage = null;

                try
                {
                    // now get a response
                    response = (request.GetResponse() as HttpWebResponse);
                }
                catch (WebException wexc)
                {
                    // get the response
                    response = (wexc.Response as HttpWebResponse);

                    // if we have no response
                    if(response == null)
                    {
                        // set an error message
                        errorMessage = "Request failed: " + wexc.Message;
                    }
                }
                catch (Exception exc)
                {
                    // set an error message
                    errorMessage = "Request failed: " + exc.Message;
                }

                // if we got a response
                if((response != null)
                    // and no error message
                    && string.IsNullOrEmpty(errorMessage))
                {
                    try
                    {
                        // open a stream
                        using (var responseStream = response.GetResponseStream())
                        {
                            // if any
                            if(responseStream != null)
                            {
                                // create the memory stream
                                memStream = new MemoryStream();
                                // create a buffer
                                byte[] buffer = new byte[1024];
                                int byteCount;

                                // read
                                do
                                {
                                    byteCount = responseStream.Read(buffer, 0, buffer.Length);
                                    memStream.Write(buffer, 0, byteCount);
                                }
                                // read until end of stream
                                while (byteCount > 0);

                                // reset memory stream position
                                memStream.Position = 0;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        // set an error message
                        errorMessage = "Exception while reading the body of the reponse - " + exc.Message;
                    }
                }

                // create the result
                result = new ResponseEx(request.RequestUri.ToString(), response, memStream, errorMessage);
            }
            return result;
        }

        #endregion
        
        #region Reading

        /// <summary>
        /// Read the response as a string
        /// </summary>
        public string GetBodyAsString()
        {
            string result = null;

            // if we got a stream
            if(this.MemStream != null)
            {

                try
                {
                    // reset stream position
                    this.MemStream.Position = 0;
                    // open a stream reader
                    var reader = new StreamReader(this.MemStream);
                    // read the content
                    result = reader.ReadToEnd();
                }
                catch (Exception exc)
                {
                    // add an error
                    this.AddErrorMessage("Could not read response body - " + exc);
                }
            }
            return result;
        }
        /// <summary>
        /// Read the response as a JSON string & deserialize
        /// </summary>
        public TContent GetBodyAsJson<TContent>()
            where TContent:class
        {
            TContent result = null;

            // if we got a body
            if(this.HasBody)
            {

                try
                {
                    // reset stream position
                    this.MemStream.Position = 0;
                    // deserialize
                    result = SerializerForJson.Deserialize<TContent>(this.MemStream);
                    // if none
                    if(result == null)
                        // add an error
                        this.AddErrorMessage("Unexpected result : response has a body, JSON deserialization did not fail, but no content has been deserialized");
                }
                catch (Exception exc)
                {
                    // add an error
                    this.AddErrorMessage("Could not deserialize response body as JSON - " + exc);
                }
            }
            return result;
        }
        /// <summary>
        /// Read the response as an XML string & deserialize
        /// </summary>
        public TContent GetBodyAsXml<TContent>()
            where TContent:class
        {
            TContent result = null;

            // if we got a stream
            if(this.MemStream != null)
            {

                try
                {
                    // reset stream position
                    this.MemStream.Position = 0;
                    // deserialize
                    result = SerializerForXml.Deserialize<TContent>(this.MemStream);
                    // if none
                    if(result == null)
                        // add an error
                        this.AddErrorMessage("Unexpected result : response has a body, XML deserialization did not fail, but no content has been deserialized");
                }
                catch (Exception exc)
                {
                    // add an error
                    this.AddErrorMessage("Could not deserialize response body as XML - " + exc);
                }
            }
            return result;
        }

        #endregion

        #region Public - Properties

        /// <summary>
        /// Request url
        /// </summary>
        public string UrlRequest { get; private set; }

        /// <summary>
        /// Response error message
        /// set only if an error occured
        /// </summary>
        public string MessageError { get; private set; }

        /// <summary>
        /// Wrapped response
        /// Can be null if the request failed
        /// </summary>
        public HttpWebResponse Response { get; private set; }

        /// <summary>
        /// Response content
        /// </summary>
        public HttpStatusCode StatusHttp
        {
            get
            {
                // default status
                var result = HttpStatusCode.BadRequest;

                // if we got a response
                if(this.Response != null)
                    // the return its status code
                    result = this.Response.StatusCode;

                return result;
            }
        }

        /// <summary>
        /// True if response is successful
        /// </summary>
        public bool IsSuccessful
        {
            get
            {
                // success if
                return
                    // no error message
                    string.IsNullOrEmpty(this.MessageError)
                    // we got a response
                    && (this.Response != null)
                    &&
                    (   // status code is 'OK'
                        this.Response.StatusCode == HttpStatusCode.OK
                        || // or status code is 'Create'
                        this.Response.StatusCode == HttpStatusCode.Created
                        || // or status code is 'No content'
                        this.Response.StatusCode == HttpStatusCode.NoContent
                    );
            }
        }
        /// <summary>
        /// True if response is successful and has a body
        /// </summary>
        public bool HasBody
        {
            get
            {
                // success if
                return
                    // no error message
                    string.IsNullOrEmpty(this.MessageError)
                    // we got a response
                    && (this.Response != null)
                    &&
                    (   // status code is 'OK'
                        this.Response.StatusCode == HttpStatusCode.OK
                        || // or status code is 'Create'
                        this.Response.StatusCode == HttpStatusCode.Created
                    )
                    // we have a memory stream
                    && (this.MemStream != null)
                    // the memory stream is not empty
                    && (this.MemStream.Length > 0);
            }
        }

        #endregion

        #region Protected - Construction

        /// <summary>
        /// Base constructor
        /// </summary>
        protected ResponseEx(string requestUrl, HttpWebResponse response, MemoryStream memStream, string errorMessage)
        {
            // store the request url
            this.UrlRequest = requestUrl;
            // store the response
            this.Response = response;
            // store the memory stream
            this.MemStream = memStream;
            // store the message
            this.MessageError = errorMessage;
        }

        #endregion
        
        #region Protected - Inner data

        /// <summary>
        /// Memory stream
        /// contains the response body
        /// </summary>
        protected MemoryStream MemStream { get; private set; }

        #endregion
        
        #region Protected - Inner tools

        /// <summary>
        /// Add an error message to existing one, or set it if none yet
        /// </summary>
        protected void AddErrorMessage(string errorMessage)
        {
            // if no error message for now
            if (string.IsNullOrEmpty(this.MessageError))
                // create one
                this.MessageError = errorMessage;
                // if we already have a message
            else
                // append the error message
                this.MessageError += " --- " + errorMessage;
        }

        #endregion
    }
}
