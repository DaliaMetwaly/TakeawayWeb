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
    
    public partial class RestaurantAppointment
    {
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public int Day { get; set; }
        public System.TimeSpan OpeningTime { get; set; }
        public System.TimeSpan CloseTime { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Restaurant Restaurant { get; set; }
    }
}