using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// RecipeBL contains methods that do database operations that have to do with recipes in cookbook
    /// </summary>
    public class RecipeBL
    {
        /// <summary>
        /// adds a recipe to database
        /// </summary>
        /// <param name="userId"> int </param>
        /// <param name="recipe"> RecipeDTO </param>
        public static void AddRecipeToCookbook(int userId, RecipeDTO recipe)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                db.CookbookRecipes.Add(CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId));
                db.SaveChanges();
            }
        }

        /// <summary>
        /// method removes a recipe from database
        /// </summary>
        /// <param name="recipeId"> int </param>
        /// <param name="userId"> int </param>
        public static void DeleteRecipeFromCookbook(int recipeId, int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //gets the user and recipe from database
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                var recp = user[0].CookbookRecipes.FirstOrDefault(c => c.recipeId==recipeId);
                //remove from database
                db.Ingredients.RemoveRange(recp.Ingredients);
                db.Instructions.RemoveRange(recp.Instructions);
                db.CookbookRecipes.Remove(recp);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// method retrieves all the recipes that belong to current user from database
        /// </summary>
        /// <param name="userId"> int </param>
        /// <returns> list of recipes </returns>
        public static List<RecipeDTO> getUserCookbook(int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                return CONVERTERS.RecipeConverter.ConvertRecipeListToDTO(user[0].CookbookRecipes.ToList());
            }
        }

        /// <summary>
        /// method checks if a recipe, that is supposed to be added, exists already in user's database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipe"></param>
        /// <returns> true - if recipe exist. false - if recipe not exist </returns>

        public static bool checkIfRecipeExist(int userId, RecipeDTO recipe)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                var recp = CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId);
                var c = db.CookbookRecipes.ToList().FirstOrDefault(a => a.userId == user[0].UserId&&a.recipeName==recipe.RecipeName && a.recipeImage==recipe.PictureSource); 
               //if recipe already exists, returns true.
               if(c!=null)
                    return true;
                else
                    return false;
                             
            }
        }
    }
}

   