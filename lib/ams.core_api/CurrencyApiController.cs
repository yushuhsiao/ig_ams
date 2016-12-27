using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ams.Data;
using ams.Controllers;

namespace ams.Controllers
{
    [Route("~/sys/currency/{action}")]
    public class CurrencyApiController : _ApiController
    {
        /// <summary>
        /// list table
        /// </summary>
        [HttpPost]
        public object list(object args) { return null; }

        /// <summary>
        /// list table with history
        /// </summary>
        [HttpPost]
        public object history(object args) { return null; }

        [HttpPost]
        public Currency get(object args) { return null; }

        [HttpPost]
        public Currency set(object args) { return null; }

        [HttpPost]
        public Currency[] setall(object[] args) { return null; }
    }
}
