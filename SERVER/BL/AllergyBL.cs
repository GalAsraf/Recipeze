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
