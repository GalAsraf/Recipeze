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
    }
}
