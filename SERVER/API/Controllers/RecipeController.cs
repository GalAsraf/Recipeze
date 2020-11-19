using DTO;
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
    [RoutePrefix("api/recipe")]
    public class RecipeController : ApiController
    {
        [Route("addRecipeToCookbook/{userId}")]
        [HttpPost]
        public IHttpActionResult addRecipeToCookbook(int userId, RecipeDTO recipe)
        {
            BL.RecipeBL.addRecipeToCookbook(userId, recipe);
            return Ok("added successfully!");
        }

        [Route("deleteRecipeFromCookbook/{userId}")]
        [HttpPost]
        public IHttpActionResult deleteRecipeFromCookbook(int userId, RecipeDTO recipe)
        {
            BL.RecipeBL.deleteRecipeFromCookbook(userId, recipe);
            return Ok("deleted successfully!");
        }

        [Route("getUserCookbook/{userId}")]
        [HttpGet]
        public IHttpActionResult getUserCookbook(int userId)
        {            
            return Ok(BL.RecipeBL.getUserCookbook(userId));
        }


    }
}