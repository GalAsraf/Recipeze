﻿using DAL;
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
            //you should check if that kind of code is OK, cause on one search, it gave strang results for chocolate cake recipe....
            string searchLine;
            List<string> allergiesForUser = BL.CategoryBL.GetAllergies(whatChecked);


            //it works in both ways - both if conditions.... I don't know whats better



            //if (selectedCategory.All(c => Char.IsLetter(c)))
            //            if (selectedCategory.Contains(@"[a-zA-Z]"))


           int errorCounter = Regex.Matches(selectedCategory, @"[a-zA-Z]").Count;
            if(errorCounter > 0)
                searchLine = selectedCategory;

            else
                searchLine = BL.CategoryBL.GetCurrentCategory(int.Parse(selectedCategory));

            string res = BL.WebScraping.GoogleSearch.CustomSearch(searchLine, userId, allergiesForUser);
            List<DTO.RecipeDTO> result = BL.WebScraping.GoogleSearch.ParseSearchResultHtml(searchLine, res, allergiesForUser);

            return Ok(result);
        }
    }
}
