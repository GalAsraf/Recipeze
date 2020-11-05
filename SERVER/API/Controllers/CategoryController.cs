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

        [Route("GetSelectedCategories/{userId}/{selectedCategory}")]
        [HttpGet]
        public IHttpActionResult GetSelectedCategories(int userId, string selectedCategory)
        {
            List<Allergy> allergiesForUser = BL.CategoryBL.GetAllergyForUser(userId);

            string searchLine = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedCategory));
            string res = BL.WebScraping.GoogleSearch.CustomSearch(searchLine, userId, allergiesForUser);
            List<DTO.Recipe> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(res, allergiesForUser);

            return Ok(result);
        }

        //[Route("getselectedcategories/{selectedcategory}")]
        //[HttpGet]
        ////trying to get an array that contains the selected categories, how do i use it.
        //public IHttpActionResult getselectedcategories(string selectedcategory)
        //{
        //    // var searchline = bl.categorybl.getselecteditem(int.parse(selectedcategory));
        //    string searchline = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedcategory));
        //    string res = BL.WebScraping.GoogleSearch.CustomSearch(searchline);
        //    List<DTO.Recipe> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(res);

        //    return Ok(result);


        //    //todo: we need to get the selectedcategory maybe as an object from 


        //}


    }


}
