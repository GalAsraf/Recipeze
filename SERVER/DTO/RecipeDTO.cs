using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RecipeDTO
    {

        public int RecipeId { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Method { get; set; }
        public string RecipeName { get; set; }
        public string PrepTime { get; set; }

        public string TotalTime { get; set; }

        public string Url { get; set; }
        //more things we should add to our site
        public string PictureSource { get; set; }
        //public List<string> MoreDetails { get; set; }


    }
}
