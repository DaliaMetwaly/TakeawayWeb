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
    
    public partial class RestaurantData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RestaurantData()
        {
            this.MinDistancePrices = new HashSet<MinDistancePrice>();
            this.Orders = new HashSet<Order>();
        }
    
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int Region_id { get; set; }
        public string MapLatitude { get; set; }
        public string MapLongitude { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MinDistancePrice> MinDistancePrices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Region Region { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
