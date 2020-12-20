using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class RecipeBL
    {
        public static void AddRecipeToCookbook(int userId, RecipeDTO recipe)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                db.CookbookRecipes.Add(CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId));
                db.SaveChanges();
            }
        }

        public static void DeleteRecipeFromCookbook(int recipeId, int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                  var user = db.Users.Where(a => a.UserId == userId).ToList();
                // user[0].CookbookRecipes.Remove(recipe);

                //i didnt make a check with the id because its null - it has no id
                var recp = user[0].CookbookRecipes.FirstOrDefault(c => c.recipeId==recipeId);
                db.Ingredients.RemoveRange(recp.Ingredients);
                db.Instructions.RemoveRange(recp.Instructions);
                db.CookbookRecipes.Remove(recp);
                db.SaveChanges();
            }
        }

        public static List<RecipeDTO> getUserCookbook(int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                return CONVERTERS.RecipeConverter.ConvertRecipeListToDTO(user[0].CookbookRecipes.ToList());
            }
        }

        public static bool checkIfRecipeExist(int userId, RecipeDTO recipe)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                var recp = CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId);
                var c = db.CookbookRecipes.ToList().FirstOrDefault(a => a.userId == user[0].UserId&&a.recipeName==recipe.RecipeName&&
                a.recipeImage==recipe.PictureSource);// && a.recipeName == recp.recipeName && a.recipeImage == recp.recipeImage).ToList(); 
               
               if(c!=null)
                    return true;
                else
                    return false;
                             
            }
        }
    }
}

   