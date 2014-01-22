using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Mvc;

using LicenceToBill.Api;
using LicenceToBill.Api.Tools;

namespace LicenceToBill.Web.DemoRest
{
    partial class TestApiController
    {
        #region Actions

        /// <summary>
        /// Test API : home
        /// </summary>
        [HttpGet]
        public ActionResult Overload()
        {
            var model = new ModelTestApiOverload();

            // set default host
            model.UrlHost = "http://dev-ltb-api.poweron.fr";

            return this.View(model);
        }
        /// <summary>
        /// Test API : home
        /// </summary>
        [HttpPost]
        public ActionResult Overload(string urlHost, ModelTestApiOverload.TypeTest? typeTest)
        {
            var model = new ModelTestApiOverload();

            // timeout : 600 sec (15mn)
            HttpContext.Server.ScriptTimeout = 600;

            // store host
            model.UrlHost = urlHost;
            // if valid
            if(!(string.IsNullOrEmpty(urlHost))
                && this.BasicAuth.ContainsKey(urlHost))
            {
                // update static host
                _UrlHost = urlHost;

                // save test type mode
                _TypeTest = (typeTest ?? ModelTestApiOverload.TypeTest.Small);

                // instanciate
                _CallsPerThread = new Dictionary<int, List<ModelTestApiOverload.Call>>();
                _MessagesPerThread = new Dictionary<int, List<string>>();

                // warm up
                this.Warmup(urlHost);

                // get start date
                var start = DateTime.UtcNow;

                // get test metrics
                int countReaders, countWriters;
                this.GetTestSize(_TypeTest.Value, out countReaders, out countWriters);

                // create a list
                var threads = new Thread[countReaders + countWriters];
                // loop - start writers
                for(int i = 0; i < countReaders;i++)
                {
                    var thread = new Thread(StartReader);
                    thread.Start();
                    threads[i] = thread;
                }
                // loop - start readers
                for(int i = countReaders; i < countReaders + countWriters;i++)
                {

                    var thread = new Thread(StartWriter);
                    thread.Start();
                    threads[i] = thread;
                }

                // check for threads end
                int countAlive;
                do
                {
                    // wait for a thread to end
                    Thread.CurrentThread.Join(10000);

                    // number of threads alive
                    countAlive = 0;
                    // loop
                    foreach (var thread in threads)
                    {
                        // if alive
                        if ((thread != null)
                            && (thread.IsAlive))
                            countAlive++;
                    }
                }
                while (countAlive > 0);

                // compute total time
                model.TimeTotal = (int) (DateTime.UtcNow - start).TotalSeconds;
                // aggregate calls
                model.Calls = new List<ModelTestApiOverload.Call>(_CallsPerThread.SelectMany(c => c.Value));
                // store a copy of the calls (for Csv output)
                _CallsPrevious = model.Calls;

                // aggregate messages
                model.Message = string.Join(" - ",  _MessagesPerThread.SelectMany(l => l.Value));

                // clear
                _CallsPerThread.Clear();
                _MessagesPerThread.Clear();

                _CallsPerThread = null;
                _MessagesPerThread = null;
            }
            else
                model.Message = "Invalid host";

            return this.View(model);
        }
        /// <summary>
        /// Test API : home
        /// </summary>
        [HttpGet]
        public ActionResult OverloadCsv()
        {
            if(_CallsPrevious != null)
                return new CsvResult(_CallsPrevious, @"calls-" + DateTime.UtcNow.ToString("MM-dd-hh-mm-ss") + ".csv");

            return this.RedirectToAction("Overload");
        }

        #endregion

        #region Inner logic

        /// <summary>
        /// Random generator
        /// </summary>
        private static string _UrlHost = null;
        /// <summary>
        /// Random generator
        /// </summary>
        private static ModelTestApiOverload.TypeTest? _TypeTest = null;
        /// <summary>
        /// Random generator
        /// </summary>
        private static Random _Random = new Random((int) DateTime.UtcNow.Ticks);

        /// <summary>
        /// Calls stored (for Csv output)
        /// </summary>
        private static List<ModelTestApiOverload.Call> _CallsPrevious = null;
        /// <summary>
        /// Per-thread result
        /// </summary>
        private static Dictionary<int, List<ModelTestApiOverload.Call>> _CallsPerThread = null;
        /// <summary>
        /// Per-thread result
        /// </summary>
        private static Dictionary<int, List<string>> _MessagesPerThread = null;

        /// <summary>
        /// Test feature
        /// </summary>
        private static readonly string KeyFeature = "8ea74d22-2bfc-4919-9776-1fc2ca3ba75d";
        /// <summary>
        /// Auth
        /// </summary>
        private static Dictionary<string, Tuple<string, string, string>> _BasicAuth = new Dictionary<string, Tuple<string, string, string>>();
        /// <summary>
        /// Auth
        /// </summary>
        protected Dictionary<string, Tuple<string, string, string>> BasicAuth
        {
            get
            {
                lock (_BasicAuth)
                {
                    if (_BasicAuth.Count == 0)
                    {
                        _BasicAuth.Add("http://dev-ltb-api.poweron.fr", new Tuple<string, string, string>("35c17b63-e3ea-4bf6-aafa-f8ecc4ae90f1", "ba881e58-58e1-4eb9-8740-c0fbc8483cfd", "7762b20d-6cff-49b3-ba52-96f118c0b681"));
                        _BasicAuth.Add("https://test-board.licencetobill.com", new Tuple<string, string, string>("35c17b63-e3ea-4bf6-aafa-f8ecc4ae90f1", "ba881e58-58e1-4eb9-8740-c0fbc8483cfd", "7762b20d-6cff-49b3-ba52-96f118c0b681"));
                        _BasicAuth.Add("https://api.licencetobill.com", new Tuple<string, string, string>("35c17b63-e3ea-4bf6-aafa-f8ecc4ae90f1", "ba881e58-58e1-4eb9-8740-c0fbc8483cfd", "7762b20d-6cff-49b3-ba52-96f118c0b681"));
                    }
                }
                return _BasicAuth;
            }
        }
        /// <summary>
        /// Start a stress test as a reader
        /// </summary>
        protected void StartReader()
        {
            // if we got an host
            if(!string.IsNullOrEmpty(_UrlHost))
            {
                // get it
                var urlHost = _UrlHost;
                // get auth
                var auth = this.BasicAuth[urlHost];

                // get current thread id
                int idThread = Thread.CurrentThread.ManagedThreadId;
                // calls list
                var calls = new List<ModelTestApiOverload.Call>();
                // messages list
                var messages = new List<string>();

                // lock the global dictionary
                lock(_CallsPerThread)
                {
                    // store it
                    _CallsPerThread.Add(idThread, calls);

                    // store it
                    _MessagesPerThread.Add(idThread, messages);
                }


                // get test lengths
                int sleepTime, duration;
                this.GetTestLength(_TypeTest.Value, out sleepTime, out duration);

                // list all users
                List<UserV2> users;
                int tries = 0;
                do
                {
                    // sleep for given sleeptime as we need users to proceed
                    Thread.Sleep(sleepTime*1000);
                    // force sleep time to 30 seconds ,for next loops
                    sleepTime = 30;

                    users = RequestFluent.Create(urlHost + "/v2/users")
                        .BasicAuthentication(auth.Item1, auth.Item2)
                        .Method(HttpVerbs.Get)
                        .ContentType("application/json")
                        .Send()
                        .GetBodyAsJson<List<UserV2>>();

                    tries++;
                }
                // retry until we have users
                while (((users == null)
                       || (users.Count == 0))
                        // or until max retry is reached (mneaning writing caused a problem)
                        && (tries < 10)
                    );
                
                // if we got users
                if((users != null)
                       && (users.Count > 0))
                {
                    // get start date
                    var startGlobal = DateTime.UtcNow;

                    try
                    {
                        // working timer
                        var timer = new Stopwatch();

                        // start the loop
                        while (startGlobal.AddSeconds(duration) > DateTime.UtcNow)
                        {
                            // get start dateime
                            var startLocal = DateTime.UtcNow;

                            timer.Start();

                            // send a random request
                            var status = this.SendRandomReadRequest(urlHost, users);

                            timer.Stop();

                            var call = new ModelTestApiOverload.Call
                                           {
                                               DateStart = startLocal,
                                               Type = ModelTestApiOverload.TypeTestApi.Read,
                                               StatusCode = status,
                                               ElapsedMilliseconds = timer.ElapsedMilliseconds
                                           };

                            timer.Reset();

                            calls.Add(call);
                        }
                    }
                    catch (Exception exc)
                    {
                        messages.Add(exc.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// Start a stress test as a writer
        /// </summary>
        protected void StartWriter()
        {
            // if we got an host
            if(!string.IsNullOrEmpty(_UrlHost))
            {
                // get it
                var urlHost = _UrlHost;
                // get auth
                var auth = this.BasicAuth[urlHost];

                int idThread = Thread.CurrentThread.ManagedThreadId;

                // calls list
                var calls = new List<ModelTestApiOverload.Call>();
                // messages list
                var messages = new List<string>();

                // lock the global dictionary
                lock(_CallsPerThread)
                {
                    // store it
                    _CallsPerThread.Add(idThread, calls);

                    // store it
                    _MessagesPerThread.Add(idThread, messages);
                }

                // get test lengths
                int sleepTime, duration;
                this.GetTestLength(_TypeTest.Value, out sleepTime, out duration);
                
                // get start date
                var startGlobal = DateTime.UtcNow;

                try
                {
                    // working timer
                    var timer = new Stopwatch();
                    // build the deal to send
                    var dealToSend = new DealV2
                                         {
                                             KeyOffer = auth.Item3,
                                             LcidUser = LtbConstants.LcidDefault
                                         };

                    int id = 0;
                    // start the loop
                    while (startGlobal.AddSeconds(duration) > DateTime.UtcNow)
                    {
                        // create an keyUser
                        var keyUser = Guid.NewGuid();
                        // get start dateime
                        var startLocal = DateTime.UtcNow;

                        timer.Start();

                        // update user key
                        dealToSend.KeyUser = keyUser.ToString();
                        dealToSend.NameUser = "user-" + id++;

                        // prepare the base request
                        var response = RequestFluent.Create(urlHost + "/v2/trial/" + keyUser)
                            .BasicAuthentication(auth.Item1, auth.Item2)
                            .Accept("application/json")
                            .Method(HttpVerbs.Post)
                            .SendJson(dealToSend);

                        timer.Stop();

                        var call = new ModelTestApiOverload.Call
                                       {
                                           DateStart = startLocal,
                                           Type = ModelTestApiOverload.TypeTestApi.Write,
                                           StatusCode = response.StatusHttp,
                                           ElapsedMilliseconds = timer.ElapsedMilliseconds
                                       };

                        timer.Reset();

                        calls.Add(call);
                    }
                }
                catch (Exception exc)
                {
                    messages.Add(exc.ToString());
                }
            }
        }
        /// <summary>
        /// Warm up the API
        /// </summary>
        private void Warmup(string urlHost)
        {
            // get auth
            var auth = this.BasicAuth[urlHost];

            // perform a warm up request
            RequestFluent.Create(urlHost + "/v2/users")
                .BasicAuthentication(auth.Item1, auth.Item2)
                .Method(HttpVerbs.Get)
                .ContentType("application/json")
                .Send();
        }
        /// <summary>
        /// Send a random read request
        /// </summary>
        private HttpStatusCode SendRandomReadRequest(string urlHost, List<UserV2> users)
        {
            var result = HttpStatusCode.Gone;

            // get auth
            var auth = this.BasicAuth[urlHost];

            // get a random user
            var keyUser = users[_Random.Next(users.Count)].KeyUser;

            // random an action
            var next = _Random.Next(3);
            // features by user
            if(next == 0)
            {
                // perform the request
                var response = RequestFluent.Create(urlHost + "/v2/features/users/" + keyUser)
                    .BasicAuthentication(auth.Item1, auth.Item2)
                    .Method(HttpVerbs.Get)
                    .ContentType("application/json")
                    .Send();

                result = response.StatusHttp;
            }
            // limitations by feature by user
            else if(next == 1)
            {
                // perform the request
                var response = RequestFluent.Create(urlHost + "/v2/features/" + KeyFeature + "/users/" + keyUser)
                    .BasicAuthentication(auth.Item1, auth.Item2)
                    .Method(HttpVerbs.Get)
                    .ContentType("application/json")
                    .Send();

                result = response.StatusHttp;
            }
            // offers by user
            else if(next == 2)
            {
                // perform the request
                var response = RequestFluent.Create(urlHost + "/v2/offers/users/" + keyUser)
                    .BasicAuthentication(auth.Item1, auth.Item2)
                    .Method(HttpVerbs.Get)
                    .ContentType("application/json")
                    .Send();

                result = response.StatusHttp;
            }
            return result;
        }
        /// <summary>
        /// Get test sizes
        /// </summary>
        private void GetTestSize(ModelTestApiOverload.TypeTest type, out int countReaders, out int countWriters)
        {
            switch(type)
            {
                case ModelTestApiOverload.TypeTest.Micro:
                    countReaders = 3;
                    countWriters = 1;
                    break;

                case ModelTestApiOverload.TypeTest.Small:
                    countReaders = 10;
                    countWriters = 2;
                    break;

                case ModelTestApiOverload.TypeTest.Medium:
                    countReaders = 15;
                    countWriters = 4;
                    break;

                case ModelTestApiOverload.TypeTest.Large:
                    countReaders = 20;
                    countWriters = 6;
                    break;

                case ModelTestApiOverload.TypeTest.Huge:
                    countReaders = 30;
                    countWriters = 10;
                    break;

                case ModelTestApiOverload.TypeTest.RandomSmall:
                    countReaders = 3 + _Random.Next(7);
                    countWriters = 1 + _Random.Next(3);
                    break;

                case ModelTestApiOverload.TypeTest.RandomMedium:
                    countReaders = 8 + _Random.Next(7);
                    countWriters = 2 + _Random.Next(4);
                    break;

                case ModelTestApiOverload.TypeTest.RandomLarge:
                    countReaders = 20 + _Random.Next(20);
                    countWriters = 10 + _Random.Next(10);
                    break;

                default:
                    countReaders = 1;
                    countWriters = 0;
                    break;
            }
        }
        /// <summary>
        /// Get test lengths
        /// </summary>
        private void GetTestLength(ModelTestApiOverload.TypeTest type, out int sleepTime, out int duration)
        {
            switch(type)
            {
                case ModelTestApiOverload.TypeTest.Micro:
                    sleepTime = 1;
                    duration = 60;
                    break;

                case ModelTestApiOverload.TypeTest.Small:
                    sleepTime = 10;
                    duration = 120;
                    break;

                case ModelTestApiOverload.TypeTest.Medium:
                    sleepTime = 20;
                    duration = 240;
                    break;

                case ModelTestApiOverload.TypeTest.Large:
                    sleepTime = 40;
                    duration = 420;
                    break;

                case ModelTestApiOverload.TypeTest.Huge:
                    sleepTime = 60;
                    duration = 600;
                    break;

                case ModelTestApiOverload.TypeTest.RandomSmall:
                    sleepTime = 5 + _Random.Next(10);
                    duration = 60 + _Random.Next(120);
                    break;

                case ModelTestApiOverload.TypeTest.RandomMedium:
                    sleepTime = 10 + _Random.Next(20);
                    duration = 120 + _Random.Next(240);
                    break;

                case ModelTestApiOverload.TypeTest.RandomLarge:
                    sleepTime = 20 + _Random.Next(60);
                    duration = 200 + _Random.Next(400);
                    break;

                default:
                    sleepTime = 1;
                    duration = 60;
                    break;
            }
        }


        #endregion
    }
}
