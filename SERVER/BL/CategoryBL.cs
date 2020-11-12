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
    public class CategoryBL
    {
        public static List<CategoryDTO> GetAllCategories()
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                return CONVERTERS.CategoryConverter.ConvertCategoryListToDTO(db.Categories);
            }
        }

        public static string GetCurrentCategory(int categoryId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var c = db.Categories.Where(a => a.CategoryId == categoryId).ToList();
                return c[0].CategoryName;
            }
        }

    public static List<string> GetAllergies(int[] whatChecked)
    {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                List<string> allergies = new List<string>();
                foreach (var item in whatChecked)
                {
                    var allergy = db.Allergies.Where(a => a.AllergyCode == item).ToList();
                    allergies.Add(allergy[0].AllergyName);
                }
                return allergies;
                //var member = db.Users.Where(a => a.UserId==userId).ToList();
                //return member[0].Allergies.ToList();
            }
        }

        //public static object GetSelectedItem(int selectedCategory)
        //{
        //    using (RecipezeEntities db = new RecipezeEntities())
        //    {
        //        return CONVERTERS.CategoryConverter.ConvertCategoryListToDTO(db.Categories);
        //    }
        //}

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
