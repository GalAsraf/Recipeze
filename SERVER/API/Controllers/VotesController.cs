using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/votes")]
    public class VotesController : ApiController
    {
        [Route("addVote")]
        [HttpPost]
        public IHttpActionResult AddVote(Vote vote)
        {
            //adding a vote
            BL.VotesBL.addVote(vote.siteName);
            return Ok("the site has been voted");
        }
    }
}
