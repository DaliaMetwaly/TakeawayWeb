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
    
    public partial class Package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Package()
        {
            this.PackageRestaurants = new HashSet<PackageRestaurant>();
        }
    
        public int ID { get; set; }
        public string PackageNameAr { get; set; }
        public string PackageNameEn { get; set; }
        public decimal PackagePrice { get; set; }
        public int PackagePeriod { get; set; }
        public System.DateTime PackageStartDate { get; set; }
        public System.DateTime PackageEndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PackageRestaurant> PackageRestaurants { get; set; }
    }
}
