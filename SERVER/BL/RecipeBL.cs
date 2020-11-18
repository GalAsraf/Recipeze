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
        public static void addRecipeToCookbook(int userId, RecipeDTO recipe)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //we want to know if the addition of the recipe is good
                User user = db.Users.FirstOrDefault(u => u.UserId == userId);
                user.CookbookRecipe.recipeName = recipe.RecipeName;
                user.CookbookRecipe.recipeImage = recipe.PictureSource;
                //it's not working cause I didn't do the database right... list and string confusing
                //recipe.Ingredients.ForEach(i => user.CookbookRecipe.Ingredients.Add(i));
                //recipe.Method.ForEach(m => user.CookbookRecipe.Instructions.Add(m));
                db.SaveChanges();
            }
        }

        public static void deleteRecipeFromCookbook(int userId, RecipeDTO recipe)
        {
            throw new NotImplementedException();
        }



        public static List<RecipeDTO> getUserCookbook(int userId)
        {
            throw new NotImplementedException();

        }
        //{
        //    using (RecipezeEntities db = new RecipezeEntities())
        //    {
        //        //todo:  we need to creat a database for personal cookbook, and here we will retrieve.
        //        //what is here' is incorrect code!!

        //        var user = db.Users.Where(a => a.UserId == userId).ToList();
        //        Console.WriteLine(user[0].CookbookRecipe);
        //        return CONVERTERS.RecipeConverter.ConvertRecipeToDAL(user[0].CookbookRecipe);
        //    }
        //}
    }
}

   