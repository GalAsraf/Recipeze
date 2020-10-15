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
        [RoutePrefix("api/allergy")]
        public class AllergyControler : ApiController
        {
            [Route("getAllAllergies")]
            [HttpGet]
            public IHttpActionResult getAllAllergies()
            {
                return Ok(BL.AllergyBL.getAllAllergies());
            }
        }

    }

