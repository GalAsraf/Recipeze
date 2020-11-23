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

        public static void DeleteRecipeFromCookbook(int userId, string recipeName)
        {
            //Im not sure about this function, if that's the way to do it.....
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                var recipe = user[0].CookbookRecipes.Where(r => r.recipeName == recipeName).ToList();
                recipe.Clear();
                //user[0].CookbookRecipes.Remove(recipe[0]);
                //CONVERTERS.RecipeConverter.ConvertRecipeToDTO

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

        public static bool checkIfRecipeExist(int userId, string recipeName)
        {
            //im not so sure about that, we need to check it....
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                var b = user[0].CookbookRecipes.Where(r => r.recipeName == recipeName).ToList();
                //not works in each searching!!
                if (b[0] != null)
                    return true;
                else
                    return false;
            }
        }
    }
}

   