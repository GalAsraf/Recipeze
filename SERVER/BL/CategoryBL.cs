using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CategoryBL
    {
        public static List<CategoryDTO> GetAllCategories()
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                return CONVERTERS.CategoryConverter.ConvertCategoryListToDTO(db.Categories);
            }
        }

        //public static object GoogleSearchString(string selectedCategory)
        //{
        //    return BL.WebScraping.GoogleSearch(selectedCategory);
        //}

        //public static List<CategoryDTO> GetSelectedCategories(string selectedCategory)
        //{
        //    using (RecipezeEntities db = new RecipezeEntities())
        //    {
        //        return CONVERTERS.CategoryConverter.ConvertCategoryToDTO(db.Categories.Select(b=>b.CategoryName==selectedCategory);
        //    }
        //}
    }
}
