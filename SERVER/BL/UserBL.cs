using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                try
                {
                    var member = db.Users.Where(a => a.Email== email && a.Password == password).ToList();
                    return member[0].UserId;
                }
                catch (Exception e) {
                    return -1;
                }

            }
            
        }
    }
}
