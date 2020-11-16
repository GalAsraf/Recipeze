using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace BL
{
    public class UserBL
    {
        public static void AddUser(UserDTO user)
        {
            using(RecipezeEntities db = new RecipezeEntities())
            {
                db.Users.Add(CONVERTERS.UserConverter.ConvertUserToDAL(user));
                db.SaveChanges();
            }
        }

        public static int GetUserExist(string email, string password)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //try
                //{
                    var member = db.Users.Where(a => a.Email == email && a.Password == password).ToList();
                    return member[0].UserId;
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e);
                //    return -1;
                //}
            }
            
        }

        public static List<AllergyDTO> getUserCookbook(int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //todo:  we need to creat a database for personal cookbook, and here we will retrieve.
                //what is here' is incorrect code!!

                var user = db.Users.Where(a => a.UserId == userId).ToList();
                Console.WriteLine(user[0].Allergies);
                return CONVERTERS.AllergyConverter.ConvertAllergyListToDTO(user[0].Allergies.ToList());
            }
        }

    }
}
