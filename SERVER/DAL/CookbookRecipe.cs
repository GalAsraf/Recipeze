//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class CookbookRecipe
    {
        public int userId { get; set; }
        public string recipeName { get; set; }
        public string recipeImage { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
    
        public virtual User User { get; set; }
    }
}
