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

        public static void DeleteRecipeFromCookbook(int userId, RecipeDTO recipe)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
              //  var user = db.Users.Where(a => a.UserId == userId).ToList();
                //Iits not deleting the recipe from cook book
               // user[0].CookbookRecipes.Remove(recipe);
                db.CookbookRecipes.Remove(CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId));
                db.SaveChanges();
                //CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId)
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

        public static bool checkIfRecipeExist(int userId, string recipeName, string recipeImage)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                //var b = user[0].CookbookRecipes.Where(r => r.recipeImage == recipeImage || r.recipeName == recipeName).ToList();
                var c = db.CookbookRecipes.Select(a => a.User.UserId == user[0].UserId &&( a.recipeName == recipeName || a.recipeImage == recipeImage));
                if (c != null)
                    return true;
                else
                    return false;
                //if (b.Count() != 0)
                //    return true;
                //else
                //    return false;
            }
        }
    }
}

   