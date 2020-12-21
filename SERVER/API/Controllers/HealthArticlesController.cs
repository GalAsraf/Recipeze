using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/HealthArticles")]
    public class HealthArticlesController : ApiController
    {

        [Route("GetRandomArticles")]
        [HttpGet]
        public IHttpActionResult GetRandomArticles()
        {
            return Ok(BL.HealthArticlesBL.GetRandomArticles());
        }
    }
}
