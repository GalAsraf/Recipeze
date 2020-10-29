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

        //[Route("GetSelectedCategories/{userId}")]
        //[HttpPost]
        //public IHttpActionResult GetSelectedCategories(int userId, string selectedCategory)
        //{
        //    List<Allergy> allergiesForUser = BL.CategoryBL.GetAllergyForUser(userId);

        //    string searchLine = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedCategory));
        //    string res = BL.WebScraping.GoogleSearch.CustomSearch(searchLine);
        //    List<DTO.Recipe> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(res);

        //    return Ok(result);

        //}

        [route("getselectedcategories/{selectedcategory}")]
        [httpget]//?
        //trying to get an array that contains the selected categories, how do i use it.
        public ihttpactionresult getselectedcategories(string selectedcategory)
        {
            // var searchline = bl.categorybl.getselecteditem(int.parse(selectedcategory));
            string searchline = bl.categorybl.getcurrentcategory(int.parse(selectedcategory));
            string res = bl.webscraping.googlesearch.customsearch(searchline);
            list<dto.recipe> result = bl.webscraping.googlesearch.parsesearchresulthtml(res);

            return ok(result);


            //todo: we need to get the selectedcategory maybe as an object from 


        }

        private object GoogleSearch(string selectedCategory)
        {
            throw new NotImplementedException();
        }
    }


}
