﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TakeawayEntities : DbContext
    {
        public TakeawayEntities()
            : base("name=TakeawayEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ItemFood> ItemFoods { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<ChainRestaurant> ChainRestaurants { get; set; }
        public virtual DbSet<Cobone> Cobones { get; set; }
        public virtual DbSet<CoboneType> CoboneTypes { get; set; }
        public virtual DbSet<CoboneUser> CoboneUsers { get; set; }
        public virtual DbSet<CookingCategory> CookingCategories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Cuisine> Cuisines { get; set; }
        public virtual DbSet<CuisineRestaurant> CuisineRestaurants { get; set; }
        public virtual DbSet<DeliveryWay> DeliveryWays { get; set; }
        public virtual DbSet<FoodType> FoodTypes { get; set; }
        public virtual DbSet<ItemFoodDetail> ItemFoodDetails { get; set; }
        public virtual DbSet<ItemFoodRate> ItemFoodRates { get; set; }
        public virtual DbSet<ItemFoodType> ItemFoodTypes { get; set; }
        public virtual DbSet<MinDistancePrice> MinDistancePrices { get; set; }
        public virtual DbSet<NotificationMsg> NotificationMsgs { get; set; }
        public virtual DbSet<NotificationUser> NotificationUsers { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatu> OrderStatus { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<PackageRestaurant> PackageRestaurants { get; set; }
        public virtual DbSet<RateType> RateTypes { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<RestaurantAppointment> RestaurantAppointments { get; set; }
        public virtual DbSet<RestaurantData> RestaurantDatas { get; set; }
        public virtual DbSet<RestaurantRate> RestaurantRates { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<RestaurantStatu> RestaurantStatus { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserComment> UserComments { get; set; }
        public virtual DbSet<UserData> UserDatas { get; set; }
    }
}
