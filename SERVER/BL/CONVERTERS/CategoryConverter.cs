using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    public static class CategoryConverter
    {
        public static Category ConvertCategoryToDAL(CategoryDTO category)
        {
            return new Category
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                MasterCategoryId = category.MasterCategoryId
            };
        }

        public static CategoryDTO ConvertCategoryToDTO(Category category)
        {
            return new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                MasterCategoryId = category.MasterCategoryId
            };
        }

        public static List<CategoryDTO> ConvertCategoryListToDTO(IEnumerable<Category> categories)
        {
            return categories.Select(c => ConvertCategoryToDTO(c)).ToList();
        }

    }
}
