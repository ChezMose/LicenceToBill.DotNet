using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Web.Mvc;

using LicenceToBill.Api.Tools;


namespace LicenceToBill.Web.DemoRest
{
    public class PayboxController : Controller
    {
        #region Pages

        [HttpGet]
        [Authenticated]
        public ActionResult Index()
        {
            // create the model
            var model = new ModelPayboxTest();
            model.BodyRequest = "VERSION=00104&DATEQ=12092013151651&TYPE=56&NUMQUESTION=15165151&SITE=1999888&RANG=99&CLE=1999888I&MONTANT=100&DEVISE=978&REFABONNE=" + Guid.NewGuid().ToString().Substring(0, 8) + "@test-licencetobill.com-9624a659&PORTEUR=1111222233334444&DATEVAL=0914&CVV=123&ACTIVITE=027&DIFFERE=000";

            return View(model);
        }

        [HttpPost]
        [Authenticated]
        public ActionResult Index(TypeUrlPaybox? type, string ip, string bodyRequest)
        {
            // create the model
            var model = new ModelPayboxTest
                            {
                                Type = type,
                                Ip = ip
                            };

            // if we have a body
            if(!string.IsNullOrEmpty(bodyRequest)
                && type.HasValue)
            {
                // push the body into the model
                model.BodyRequest = bodyRequest;
                // compute the url
                model.Url = this.GetUrl(type.Value, ip);

                // start a timer
                var timer = Stopwatch.StartNew();

                try
                {
                    // HACK
                    if(type.Value == TypeUrlPaybox.PppsPreprodPlus)
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    var response = RequestFluent.Create(model.Url)
                        .Method(HttpVerbs.Post)
                        .ContentType("application/x-www-form-urlencoded")
                        .Send(bodyRequest);

                    model.Status = response.StatusHttp;
                    model.BodyResponse = response.GetBodyAsString();
                }
                catch(Exception exc)
                {
                    model.Status = HttpStatusCode.InternalServerError;
                    model.BodyResponse = exc.ToString();
                }
                finally
                {
                    // stop the timer
                    timer.Stop();
                    // store elapsed time
                    model.ElapsedMilliseconds = (int)timer.ElapsedMilliseconds;
                }

                try
                {
                    var responsePing = RequestFluent.Create(ConfigurationManager.AppSettings["TestPingUrl"] ?? "http://dev-ltb-demo.poweron.fr/Paybox/Ping")
                                    .Send();

                    model.IpOutgoing = responsePing.GetBodyAsString();
                }
                catch(Exception exc)
                {
                    model.IpOutgoing = "failure (" + exc.Message + ")";
                }
            }
            // if no body
            else
            {
                model.BodyRequest = "VERSION=00104&DATEQ=12092013151651&TYPE=56&NUMQUESTION=15165151&SITE=1999888&RANG=99&CLE=1999888I&MONTANT=100&DEVISE=978&REFABONNE=" + Guid.NewGuid().ToString().Substring(0, 8) + "@test-licencetobill.com-9624a659&PORTEUR=1111222233334444&DATEVAL=0914&CVV=123&ACTIVITE=027&DIFFERE=000";
            }
            return View(model);
        }

        /// <summary>
        /// Enum => url
        /// </summary>
        private string GetUrl(TypeUrlPaybox type, string ip)
        {
            if(!string.IsNullOrEmpty(ip))
            {
                return "https://" + ip + "/PPPS.php";
            }
            else
            {
                switch (type)
                {
                    case TypeUrlPaybox.PppsPreprodPlus:
                    case TypeUrlPaybox.PppsPreprod:
                        return "https://preprod-ppps.paybox.com/PPPS.php";

                    case TypeUrlPaybox.Ppps:
                        return "https://ppps.paybox.com/PPPS.php";

                    case TypeUrlPaybox.Ppps1:
                        return "https://ppps1.paybox.com/PPPS.php";

                    case TypeUrlPaybox.Ppps2:
                        return "https://ppps2.paybox.com/PPPS.php";

                    case TypeUrlPaybox.TestHttps:
                        return "https://code.google.com/apis/console";
                }
            }
            return null;
        }

        #endregion

        #region Ping

        /// <summary>
        /// Execute a traceroute for max given amount of time
        /// </summary>
        public ActionResult Ping()
        {
            string ip = null;

            try
            {

                var request = this.Request;
                // get the forwarded ip
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                // if none
                if (string.IsNullOrEmpty(ip))
                {
                    // get url
                    ip = request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch(Exception exc)
            {
                ip = "Exception - " + exc.Message;
            }

            var response = this.Response;
            response.Clear();
            this.Response.ContentType = "plain/text";
            response.Write(ip);
            
            return null;
        }

        #endregion

        #region Traceroute

        /// <summary>
        /// Execute a traceroute for max given amount of time
        /// </summary>
        [Authenticated]
        public ActionResult Traceroute(string ipOrHost, int? timeout, int? maxHops)
        {
            var model = new ModelPayboxTraceroute();

            // fill data
            model.IpOrHost = ipOrHost;
            model.Timeout = timeout;
            model.MaxHops = maxHops ?? 30;

            // if we got an url
            if(!string.IsNullOrEmpty(model.IpOrHost))
            {
                // set timeout
                Server.ScriptTimeout = timeout ?? 600;
                // results
                model.Status = new List<string>();

                try
                {
                    // open ip address
                    var ipAddress = Dns.GetHostEntry(model.IpOrHost).AddressList[0];

                    // prepare ping
                    using (var pingSender = new Ping())
                    {
                        var pingOptions = new PingOptions();
                        var stopWatch = new Stopwatch();

                        pingOptions.DontFragment = true;
                        pingOptions.Ttl = 1;
                        model.Status.Add(
                            string.Format(
                                "Tracing route to {0} over a maximum of {1} hops:",
                                ipAddress,
                                model.MaxHops));

                        for (int i = 1; i < model.MaxHops + 1; i++)
                        {
                            stopWatch.Reset();
                            stopWatch.Start();
                            var pingReply = pingSender.Send(
                                ipAddress,
                                5000,
                                new byte[32], pingOptions);
                            stopWatch.Stop();

                            // if succeeded
                            if(pingReply != null)
                            {

                                model.Status.Add(
                                    string.Format("[{0}] - {1}ms - {2} ({3})",
                                                  i,
                                                  stopWatch.ElapsedMilliseconds,
                                                  pingReply.Address,
                                                  pingReply.Status
                                                  ));

                                if(pingReply.Status == IPStatus.Success)
                                {
                                    model.Status.Add("Trace complete.");
                                    break;
                                }
                            }
                            // if failed
                            else
                                model.Status.Add(
                                    string.Format("[{0}]  - {1}ms - (no reply)",
                                                  i,
                                                  stopWatch.ElapsedMilliseconds
                                        ));

                            pingOptions.Ttl++;
                        }
                    }
                }
                catch(Exception exc)
                {
                    model.Status.Add(exc.ToString());
                }
            }
            return View(model);
        }

        #endregion

        #region TypeUrlPaybox definition

        /// <summary>
        /// Url types
        /// </summary>
        public enum TypeUrlPaybox
        {
            PppsPreprod,
            Ppps,
            Ppps1,
            Ppps2,
            PppsPreprodPlus,
            TestHttps
        }

        #endregion
    }
}
