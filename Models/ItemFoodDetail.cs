//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Takeaway.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemFoodDetail
    {
        public long ID { get; set; }
        public long ItemFoodID { get; set; }
        public long AddItemFoodID { get; set; }
        public int CategoryTypeID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
    
        public virtual CategoryType CategoryType { get; set; }
        public virtual ItemFood ItemFood { get; set; }
    }
}