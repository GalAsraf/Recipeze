using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    /// <summary>
    /// RecipeConverter is in charge of converting between different layers and the database
    /// </summary>
    public class RecipeConverter
    {
        public static CookbookRecipe ConvertRecipeToDAL(RecipeDTO recipe, int userId)
        {
            return new CookbookRecipe
            {
                userId = userId,
                recipeName = recipe.RecipeName,
                recipeImage = recipe.PictureSource,
               
                Ingredients = recipe.Ingredients.Select((val, index) => new Ingredient { IngredientText = val, Index = index }).ToList(),
                Instructions = recipe.Method.Select((val, index) => new Instruction { InstructionText = val, Index = index }).ToList(),
            };
        }

        public static RecipeDTO ConvertRecipeToDTO(CookbookRecipe recipe)
        {
            return new RecipeDTO
            {
                RecipeId=recipe.recipeId,
                RecipeName = recipe.recipeName,
                PictureSource = recipe.recipeImage,
                Ingredients = recipe.Ingredients.OrderBy(i=>i.Index).Select(r=>r.IngredientText).ToList(),
                Method = recipe.Instructions.OrderBy(i => i.Index).Select(r => r.InstructionText).ToList()
            };
        }


        public static List<RecipeDTO> ConvertRecipeListToDTO(IEnumerable<CookbookRecipe> recipes)
        {
            return recipes.Select(c => ConvertRecipeToDTO(c)).ToList();
        }

    }
}
