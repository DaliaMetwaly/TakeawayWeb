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
    
    public partial class MinDistancePrice
    {
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public int RestaurantDataID { get; set; }
        public int RegionID { get; set; }
        public decimal minPrice { get; set; }
        public decimal deliveryFeeValue { get; set; }
        public string CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    
        public virtual Region Region { get; set; }
        public virtual RestaurantData RestaurantData { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
