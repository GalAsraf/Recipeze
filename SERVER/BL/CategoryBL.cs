using DAL;
using DTO;
using GoogleApi.Entities.Search.Video.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// CategoryBL contains methods which do operations with the database
    /// </summary>
    public class CategoryBL
    {
        /// <summary>
        /// get all categories of food
        /// </summary>
        /// <returns> list of categories </returns>
        public static List<CategoryDTO> GetAllCategories()
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                return CONVERTERS.CategoryConverter.ConvertCategoryListToDTO(db.Categories);
            }
        }

        /// <summary>
        /// get category name from database by category Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns> category name </returns>
        public static string GetCurrentCategory(int categoryId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var c = db.Categories.Where(a => a.CategoryId == categoryId).ToList();
                return c[0].CategoryName;
            }
        }     
    }
}