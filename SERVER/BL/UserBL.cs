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

        public static int GetUserExist(string name, string password)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                try
                {
                    var member = db.Users.Where(a => a.UserName== name && a.Password == password).ToList();
                    return int.Parse(member[0].Password.ToString());
                }
                catch (Exception e) { return 0; }

            }
            
        }
    }
}
