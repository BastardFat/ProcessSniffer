using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdminServer.Controllers
{
    [RoutePrefix("sniffer")]
    public class SnifferController : ApiController
    {
        
        [HttpGet]
        [ActionName("test")]
        public string Test()
        {
            return "OK";
        }

    }
}
