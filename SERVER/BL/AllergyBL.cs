using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// AllergyBL contains methods that do operations with the database 
    /// </summary>
    public class AllergyBL
    {
        /// <summary>
        /// The method gets all allergies from database
        /// </summary>
        /// <returns> list of allergies </returns>
        public static List<AllergyDTO> getAllAllergies()
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //convert allergy list to DTO
                return CONVERTERS.AllergyConverter.ConvertAllergyListToDTO(db.Allergies);
            }
        }

        /// <summary>
        /// The method gets the allergies for the current user 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<AllergyDTO> getCurrentUserAllergies(int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //get user from database by userId
                var user = db.Users.Where(a => a.UserId == userId).ToList();
                return CONVERTERS.AllergyConverter.ConvertAllergyListToDTO(user[0].Allergies.ToList());
            }
        }

        /// <summary>
        /// define allergies for current user, add new ones and delete old ones
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="allergies"></param>
        /// <returns> true, if allergies added successfully </returns>
        public static bool DefineAllergiesForUser(int userId, List<int> allergies)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //get user from database by Id
                User user = db.Users.FirstOrDefault(u => u.UserId == userId);
                user.Allergies.Clear();
                //update selected allergies the user chose
                allergies.ForEach(
                    a =>
                    {
                        user.Allergies.Add(db.Allergies.FirstOrDefault(al=>al.AllergyCode==a));
                    });
                db.SaveChanges();
                return true;               
            }
        }

        /// <summary>
        /// get substitutes for current user's allergies
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> list of substitutes </returns>
        public static List<SubstitutesDTO> getSubstitutes(int userId)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                //get current user's allergies
                List<AllergyDTO> allergies = getCurrentUserAllergies(userId);
                if (allergies == null)
                    return null;
                List<SubstitutesDTO> substitutes = new List<SubstitutesDTO>();
                //get matching substitutes for each allergy
                allergies.ForEach(a =>
                {
                   var ans = db.Substitutes.Where(s => s.AllergyId == a.AllergyCode).ToList();
                   substitutes.Add(new SubstitutesDTO() { SubstitutesName = ans[0].SubstituteName , SubstitutesIcon = ans[0].icon});
                });
                return substitutes;
            }
            
        }

        /// <summary>
        /// get all allergies from database
        /// </summary>
        /// <param name="whatChecked"></param>
        /// <returns> list of allergies </returns>
        public static List<string> GetAllergies(int[] whatChecked)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                List<string> allergies = new List<string>();
                //add allergies name to list by allergy Id
                foreach (var item in whatChecked)
                {
                    var allergy = db.Allergies.Where(a => a.AllergyCode == item).ToList();
                    allergies.Add(allergy[0].AllergyName);
                }
                return allergies;
            }
        }
    }
}
