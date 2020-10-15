using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    public static class AllergyConverter
    {
        public static Allergy ConvertAllergyToDAL(AllergyDTO allergy)
        {
            return new Allergy
            {
                AllergyCode = allergy.AllergyCode,
                AllergyName = allergy.AllergyName,
                IsPersonal = allergy.IsPersonal
            };
        }

        public static AllergyDTO ConvertAllergyToDTO(Allergy allergy)
        {
            return new AllergyDTO
            {
                AllergyCode = allergy.AllergyCode,
                AllergyName = allergy.AllergyName,
                IsPersonal = allergy.IsPersonal
            };
        }

        public static List<AllergyDTO> ConvertAllergyListToDTO(IEnumerable<Allergy> allergies)
        {
            return allergies.Select(c => ConvertAllergyToDTO(c)).ToList();
        }
    }
}
