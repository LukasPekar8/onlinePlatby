//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace onlinePlatby.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PropertyCombination
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductPropertyValue { get; set; }
        public int ProductPropertyId { get; set; }
    
        public virtual ProductProperty ProductProperty { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
