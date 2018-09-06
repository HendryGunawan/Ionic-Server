using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //public HttpResponseMessage Get()
        //{
        //    Student stud = new Student();
        //    stud.id = 3;
        //    stud.nama = "Hendry";


        //    if (stud == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.Forbidden, "wkwk");
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, stud);
        //}

        //// GET api/values/5
        //public string GetData(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
