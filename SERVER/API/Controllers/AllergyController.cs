using DTO;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/allergy")]
    public class AllergyController : ApiController
    {
        [Route("getAllAllergies")]
        [HttpGet]
        public IHttpActionResult GetAllAllergies()
        {
            //get all allergies
            return Ok(BL.AllergyBL.getAllAllergies());
        }

        [Route("getCurrentUserAllergies/{userId}")]
        [HttpGet]
        public IHttpActionResult getCurrentUserAllergies(int userId)
        {
            //getcurrent user's allergies
            return Ok(BL.AllergyBL.getCurrentUserAllergies(userId));
        }

        [Route("defineAllergiesForUser/{userId}")]
        [HttpPost]
        public IHttpActionResult DefineAllergiesForUser(int userId,List<int> allergies)
        {
            //define allergies for current user
            return Ok(BL.AllergyBL.DefineAllergiesForUser(userId,allergies));
        }

        [Route("getSubstitutes/{userId}")]
        [HttpGet]
        public IHttpActionResult getSubstitutes(int userId)
        {
            //get substitutes for allergies
            return Ok(BL.AllergyBL.getSubstitutes(userId));
        }
    }
}
