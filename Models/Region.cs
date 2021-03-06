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
    
    public partial class Region
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Region()
        {
            this.Restaurants = new HashSet<Restaurant>();
            this.UserDatas = new HashSet<UserData>();
            this.MinDistancePrices = new HashSet<MinDistancePrice>();
            this.RestaurantDatas = new HashSet<RestaurantData>();
        }
    
        public int ID { get; set; }
        public string RegionEn { get; set; }
        public string RegionAr { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string NotesAr { get; set; }
        public string NotesEn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
        public string PostalCode { get; set; }
    
        public virtual City City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Restaurant> Restaurants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserData> UserDatas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MinDistancePrice> MinDistancePrices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RestaurantData> RestaurantDatas { get; set; }
    }
}
