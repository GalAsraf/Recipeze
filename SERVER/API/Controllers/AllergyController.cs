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
            return Ok(BL.AllergyBL.getAllAllergies());
        }

        [Route("getCurrentUserAllergies/{userId}")]
        [HttpGet]
        public IHttpActionResult getCurrentUserAllergies(int userId)
       {
            return Ok(BL.AllergyBL.getCurrentUserAllergies(userId));
        }


        [Route("defineAllergiesForUser/{userId}")]
        [HttpPost]
        public IHttpActionResult DefineAllergiesForUser(int userId,List<int> allergies)
        {
            return Ok(BL.AllergyBL.DefineAllergiesForUser(userId,allergies));
        }


        //[Route("AddAllergy"), HttpPost]
        //public IHttpActionResult AddAllergy(AllergyDTO allergy)
        //{
        //    BL.AllergyBL.AddAllergy(allergy);
        //    return Ok("run");
        //}


    }


}
