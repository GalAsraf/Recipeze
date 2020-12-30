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
    /// <summary>
    /// UserBL contains methods that do database operations which have to do with the user
    /// </summary>
    public class UserBL
    {
        /// <summary>
        /// adds new user to database
        /// </summary>
        /// <param name="user"> UserDTO </param>
        public static void AddUser(UserDTO user)
        {
            using(RecipezeEntities db = new RecipezeEntities())
            {
                db.Users.Add(CONVERTERS.UserConverter.ConvertUserToDAL(user));
                db.SaveChanges();
            }
        }

        /// <summary>
        /// checks if user already exists in database
        /// </summary>
        /// <param name="email"> string </param>
        /// <param name="password"> string </param>
        /// <returns>userId if exists. If doesn't exist returns -1 </returns>
        public static int GetUserExist(string email, string password)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                try
                {
                    var member = db.Users.Where(a => a.Email == email && a.Password == password).ToList();
                    return member[0].UserId;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return -1;
                }
            }
        } 
    }
}
