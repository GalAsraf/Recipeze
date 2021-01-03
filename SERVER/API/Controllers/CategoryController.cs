using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Text.RegularExpressions;

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
            //get all allergies
            return Ok(BL.CategoryBL.GetAllCategories());
        }

        [Route("GetSelectedCategories/{userId}/{selectedCategory}")]
        [HttpPost]
        public IHttpActionResult GetSelectedCategories(int userId, string selectedCategory, int[] whatChecked)
        {
            //search the recipe by HTMLAgillityPack web scraping
            string searchLine;
            List<string> allergiesForUser = BL.AllergyBL.GetAllergies(whatChecked);
            int errorCounter = Regex.Matches(selectedCategory, @"[a-zA-Z]").Count;
            if (errorCounter > 0)
                searchLine = selectedCategory;
            else
                searchLine = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedCategory));
            string res = BL.WebScraping.GoogleSearch.CustomSearch(searchLine, allergiesForUser);
            List<DTO.RecipeDTO> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(searchLine, res, allergiesForUser);
            return Ok(result);
        }
    }
}
