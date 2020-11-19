using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AllergyBL
    {
        public static List<AllergyDTO> getAllAllergies()
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                return CONVERTERS.AllergyConverter.ConvertAllergyListToDTO(db.Allergies);
            }
        }

        public static List<AllergyDTO> getCurrentUserAllergies(int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                Console.WriteLine(user[0].Allergies);
                return CONVERTERS.AllergyConverter.ConvertAllergyListToDTO(user[0].Allergies.ToList());
            }
        }



        public static bool DefineAllergiesForUser(int userId, List<int> allergies)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                User user = db.Users.FirstOrDefault(u => u.UserId == userId);
                //clear not working!
                user.Allergies.Clear();
                allergies.ForEach(
                    a =>
                    {
                        user.Allergies.Add(db.Allergies.FirstOrDefault(al=>al.AllergyCode==a));
                    });
                db.SaveChanges();
                return true;
                
            }
        }

       


        //public static void AddAllergie(AllergyDTO allergy)
        //{
        //    using (RecipezeEntities db = new RecipezeEntities())
        //    {
        //        //allergy has to be added according to user, so I also have to get the current user that's logged in

        //        //db.Allergies.Add(CONVERTERS.AllergyConverter.ConvertAllergyToDAL(allergy));
        //        //db.SaveChanges();
        //    }
        //}
    }
}
