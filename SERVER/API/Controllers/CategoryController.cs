using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors("*","*","*")]
    [RoutePrefix("api/category")]
    public class CategoryController : ApiController
    {
[Route("getAllCategories")]
[HttpGet]
public IHttpActionResult GetAllCategories()
        {
            return Ok(BL.CategoryBL.GetAllCategories());
        }
    }
}
