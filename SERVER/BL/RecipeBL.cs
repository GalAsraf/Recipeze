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
                
                foreach (var item in db.CookbookRecipes)
                {
                    if(item.User.UserId == userId)
                        if(item.recipeName == recipe.RecipeName && item.recipeImage == recipe.PictureSource)
                        {
                            db.CookbookRecipes.Remove(item);
                            break;
                        }
                }
               // db.CookbookRecipes.Remove(CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId));
               // db.SaveChanges();
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

        public static bool checkIfRecipeExist(int userId, RecipeDTO recipe)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //var user = db.Users.Where(a => a.UserId == userId).ToList();
                //var recp = CONVERTERS.RecipeConverter.ConvertRecipeToDAL(recipe, userId);
                //var c = db.CookbookRecipes.Where(a => a.userId == user[0].UserId).ToList();// && a.recipeName == recp.recipeName && a.recipeImage == recp.recipeImage).ToList(); 
                //if(c[0].recipeImage==recipe.PictureSource && c[0].recipeName == recipe.RecipeName)
                //    return true;
                //else
                    return false;
                             
            }
        }
    }
}

   