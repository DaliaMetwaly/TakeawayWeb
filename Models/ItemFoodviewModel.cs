
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System;

namespace Takeaway.Models
{

    public class ItemfoodViewModel : ItemFood
    {
        //  [Required]
         public HttpPostedFileBase imgpath { get; set; }  
        public List<SizeList> sizeList { get; set; }
        
    }

    public class SizeList
    {
        public int SizeId { get; set; }
      
        public decimal Price { get; set; }

    }
    //public  class CustomTempData
    //{
    //    public TempDataDictionary TempData2 { get; set; }

    //}

    public  class DrinkList
    {
        public long Id { get; set; }
        [DisplayName("المشروبات")]
        public string DrinkName { get; set; }

    }

    public  class AdditionList
    {
        public long Id { get; set; }
        [DisplayName("الاضافات")]
        public string AdditonName { get; set; } 

    }

    public class OrderViewModel : Order
    {
        [DisplayName("المطعم")]
        [Required(ErrorMessage = "اختر المطعم")]
        public int RestaurantID { get; set; }
        public List<FoodTempList> FoodtempList { get; set; }
        public List<FoodEditTempList> FoodEdittempList { get; set; }
    }

    //public class OrderViewModel
    //{
    //    public int ID { get; set; }
    //    public string DeliveryUserID { get; set; }
    //    public int RestaurantDataID { get; set; }
    //    public int OrderStatusID { get; set; }
    //    public decimal TotalPrice { get; set; }
    //    public System.DateTime OrderDate { get; set; }
    //    public Nullable<System.TimeSpan> Delivery_estimation { get; set; }
    //    public string Description { get; set; }
    //    public bool IsActive { get; set; }
    //    public bool IsDetete { get; set; }

    //    public virtual OrderStatu OrderStatu { get; set; }
    //    public virtual User User { get; set; }

    //    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    //    public virtual RestaurantData RestaurantData { get; set; }

    //    public int RestaurantID { get; set; }
    //    public List<FoodTempList> FoodtempList { get; set; }
    //    public List<FoodEditTempList> FoodEdittempList { get; set; }
    //}


    public class FoodTempList
    {
        public long Id { get; set; }
        [DisplayName("الاضافات")]
        public long ItemfoodAddId { get; set; }
        [DisplayName("المشروبات")]
        public long ItemfoodDrinkId { get; set; }
        [DisplayName("نوع الوجبه")]
        public string FoodName { get; set; }

        [DisplayName("درجة التسوية")]
        public int? CookingCatID { get; set; }

        
        public bool isCooking { get; set; }
        public string FoodCatType { get; set; }
        [DisplayName("السعر")]
        public decimal FoodPrice { get; set; }
        [DisplayName("العدد")]
        public int Foodcount { get; set; }
    }
    public class FoodEditTempList
    {
        public long Id { get; set; }
        // public long ItemfoodAddId { get; set; }
        //public long ItemfoodDrinkId { get; set; }
        public long ItemfoodId { get; set; }
        public long OrderId { get; set; }
        public long? ItemFoodParentID { get; set; }

        public string FoodName { get; set; }
       
        public int? CookingCatID { get; set; }

        public bool isCooking { get; set; }
        public string FoodCatType { get; set; }
        public decimal FoodPrice { get; set; }
        public int Foodcount { get; set; }
    }
}