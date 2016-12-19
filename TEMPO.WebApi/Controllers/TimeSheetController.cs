using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TEMPO.WebApi.Controllers
{
    public class TimeSheetController : ApiController
    {
        // GET: api/TimeSheet
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TimeSheet/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TimeSheet
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TimeSheet/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TimeSheet/5
        public void Delete(int id)
        {
        }
    }
}
