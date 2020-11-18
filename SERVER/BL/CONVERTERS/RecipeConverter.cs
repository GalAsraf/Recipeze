using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    public class RecipeConverter
    {
        public static CookbookRecipe ConvertRecipeToDAL(RecipeDTO recipe, User userId)
        {
            return new CookbookRecipe
            {
               // userId = userId,
               recipeName = recipe.RecipeName,
               recipeImage = recipe.PictureSource
               //it's not working because it's not the same type - string and list... we need to change it in database
               //Ingredients = recipe.Ingredients,
               //Instructions = recipe.Method
            };
        }

        public static RecipeDTO ConvertRecipeToDTO(CookbookRecipe recipe)
        {
            return new RecipeDTO
            {
                // userId = userId,
                RecipeName = recipe.recipeName,
                PictureSource = recipe.recipeImage,
                //the same, it's not working because it's not the same type - string and list... we need to change it in database
                //Ingredients = recipe.Ingredients,
                //Method = recipe.Instructions

            };
        }



        public static List<RecipeDTO> ConvertRecipeListToDTO(IEnumerable<CookbookRecipe> recipes)
        {
            return recipes.Select(c => ConvertRecipeToDTO(c)).ToList();
        }

    }
}
