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
    
    public partial class KeyWordForRecipePart
    {
        public int KeyWordId { get; set; }
        public int PartId { get; set; }
        public string TheKeyWord { get; set; }
    
        public virtual RecipeContentPart RecipeContentPart { get; set; }
    }
}
