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
    
    public partial class Restaurant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Restaurant()
        {
            this.Categories = new HashSet<Category>();
            this.CuisineRestaurants = new HashSet<CuisineRestaurant>();
            this.PackageRestaurants = new HashSet<PackageRestaurant>();
            this.RestaurantAppointments = new HashSet<RestaurantAppointment>();
            this.UserComments = new HashSet<UserComment>();
            this.MinDistancePrices = new HashSet<MinDistancePrice>();
            this.RestaurantRates = new HashSet<RestaurantRate>();
            this.RestaurantDatas = new HashSet<RestaurantData>();
        }
    
        public int ID { get; set; }
        public string RestaurantNameEn { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantDescrption { get; set; }
        public string RestaurantDescrptionEn { get; set; }
        public string RestaurantPhone { get; set; }
        public Nullable<int> ChainRestaurants_id { get; set; }
        public int DeliveryWayID { get; set; }
        public int RestaurantStatus { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_modified { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> MembershipExpired { get; set; }
        public string MapLatitude { get; set; }
        public string MapLongitude { get; set; }
        public string Logo { get; set; }
        public int Stars { get; set; }
        public Nullable<int> Country_Id { get; set; }
        public Nullable<int> City_Id { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Address { get; set; }
        public bool OneMachine { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ChainRestaurant ChainRestaurant { get; set; }
        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuisineRestaurant> CuisineRestaurants { get; set; }
        public virtual DeliveryWay DeliveryWay { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PackageRestaurant> PackageRestaurants { get; set; }
        public virtual Region Region { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RestaurantAppointment> RestaurantAppointments { get; set; }
        public virtual RestaurantStatu RestaurantStatu { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserComment> UserComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MinDistancePrice> MinDistancePrices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RestaurantRate> RestaurantRates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RestaurantData> RestaurantDatas { get; set; }
    }
}
