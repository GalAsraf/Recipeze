using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AllergyDTO
    {
        public int AllergyCode { get; set; }
        public string AllergyName { get; set; }
        public Nullable<bool> IsPersonal { get; set; }
    }
}
