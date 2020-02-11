using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Takeaway.Models;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    [MetadataType(typeof(CountryMetadata))]
    public partial class Country
    { }

    [MetadataType(typeof(CityMetadata))]
    public partial class City
    {
    }
    [MetadataType(typeof(RegionMetadata))]
    public partial class Region
    {
    }
    //[MetadataType(typeof(PayTypeMetadata))]
    //public partial class PayType
    //{
    //}
    [MetadataType(typeof(FoodTypeMetadata))]
    public partial class FoodType
    {
    }

    [MetadataType(typeof(RestaurantStatuMetadata))]
    public partial class RestaurantStatu
    {
    }
    [MetadataType(typeof(OrderStatuMetadata))]
    public partial class OrderStatu
    {
    }

    [MetadataType(typeof(FeeTypeMetadata))]
    public partial class FeeType
    {
    }

    [MetadataType(typeof(CuisineMetadata))]
    public partial class Cuisine
    {
    }

    [MetadataType(typeof(ItemFoodMetadata))]
    public partial class ItemFood
    {
    }
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    [MetadataType(typeof(PackageMetadata))]
    public partial class Package
    {
    }

    [MetadataType(typeof(RestaurantMetadata))]
    public partial class Restaurant
    {
    }

    [MetadataType(typeof(OfferMetadata))]
    public partial class Offer
    {
    }

    [MetadataType(typeof(SizeMetadata))]
    public partial class Size
    {
    }

    [MetadataType(typeof(CoboneMetadata))]
    public partial class Cobone
    {
    }
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }
    [MetadataType(typeof(UserDataMetadata))]
    public partial class UserData
    {
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }
}