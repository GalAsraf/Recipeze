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
    }
}
