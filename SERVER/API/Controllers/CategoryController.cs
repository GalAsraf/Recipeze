using DAL;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/category")]
    public class CategoryController : ApiController
    {
        [Route("getAllCategories")]
        [HttpGet]
        public IHttpActionResult GetAllCategories()
        {
            return Ok(BL.CategoryBL.GetAllCategories());
        }

        //[Route("GetSelectedCategories/{userId}/{selectedCategory}")]
        //[HttpGet]
        //public IHttpActionResult GetSelectedCategories(int userId, string selectedCategory)
        //{
        //    List<Allergy> allergiesForUser = BL.CategoryBL.GetAllergyForUser(userId);

        //    string searchLine = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedCategory));
        //    string res = BL.WebScraping.GoogleSearch.CustomSearch(searchLine, userId, allergiesForUser);
        //    List<DTO.Recipe> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(res, allergiesForUser);

        //    return Ok(result);
        //}

        [Route("GetSelectedCategories/{userId}/{selectedCategory}")]
        [HttpPost]
        public IHttpActionResult GetSelectedCategories(int userId, string selectedCategory, int[] whatChecked)
        {
            List<string> allergiesForUser = BL.CategoryBL.GetAllergies(whatChecked);
            string searchLine = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedCategory));
            string res = BL.WebScraping.GoogleSearch.CustomSearch(searchLine, userId, allergiesForUser);
            List<DTO.Recipe> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(res, allergiesForUser);

            return Ok(result);
        } 
    }
}
