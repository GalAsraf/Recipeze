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

        [Route("GetSelectedCategories/{selectedCategory}")]
        [HttpGet]//?
        //trying to get an array that contains the selected categories, how do I use it.
        public IHttpActionResult GetSelectedCategories(string selectedCategory)
        {
            // var searchLine = BL.CategoryBL.GetSelectedItem(int.Parse(selectedCategory));
            string searchLine  = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedCategory));
            string res = BL.WebScraping.GoogleSearch.CustomSearch(searchLine);
            List<String> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(res);

            return Ok(result);


            //todo: we need to get the selectedCategory maybe as an object from 


        }

        private object GoogleSearch(string selectedCategory)
        {
            throw new NotImplementedException();
        }
    }


}
