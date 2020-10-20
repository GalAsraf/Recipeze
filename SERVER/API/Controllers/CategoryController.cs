using System;
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

        [Route("GetSelectedCategories(string selectedCategory)")]
        [HttpPost]//?
        //trying to get an array that contains the selected categories, how do I use it.
        public IHttpActionResult GetSelectedCategories(string selectedCategory)
        {
            //BL.CategoryBL.GetSelectedCategories(selectedCategory)
            //BL.CategoryBL.GoogleSearchString(selectedCategory)
            BL.WebScraping.GoogleSearch.CustomSearch(selectedCategory);
            return Ok(selectedCategory);



            //how do add web reference?
            //licence key?
            //com.google.api.GoogleSearchService s = new TestGoogle.com.google.api.GoogleSearchService();
            //com.google.api.GoogleSearchResult r = s.doGoogleSearch("put your lisence key her ", Txt_Text.Text, 0, 10, false, "", true, "", "", "");
            //int estimatedCount = r.estimatedTotalResultsCount;  //?

            //DataTable dtResults = new DataTable();
            //dtResults.Columns.Add(new DataColumn("Title", typeof(string)));
            //dtResults.Columns.Add(new DataColumn("Summary", typeof(string)));
            //dtResults.Columns.Add(new DataColumn("URL", typeof(string)));
            //for (int i = 0; i < 10; i++)
            //{
            //    DataRow dr = dtResults.NewRow();
            //    dr[0] = r.resultElements[i].title;
            //    dr[1] = r.resultElements[i].snippet;
            //    dr[2] = r.resultElements[i].URL;
            //    dtResults.Rows.Add(dr);
            //}
            //dataGridView1.DataSource = dtResults;

            //selectedCategory

        }

        private object GoogleSearch(string selectedCategory)
        {
            throw new NotImplementedException();
        }
    }


}
