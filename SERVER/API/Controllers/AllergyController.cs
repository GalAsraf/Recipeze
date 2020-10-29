using DTO;
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


        //[Route("AddAllergy"), HttpPost]
        //public IHttpActionResult AddAllergy(AllergyDTO allergy)
        //{
        //    BL.AllergyBL.AddAllergy(allergy);
        //    return Ok("run");
        //}


    }


}
