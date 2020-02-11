using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class MinDistancePriceVM 
    {
        public int ID { get; set; }

        [DisplayName("  المطعم  ")]
        [Required(ErrorMessage = " إختار المطعم  ")]
        public int RestaurantID { get; set; }

        [DisplayName("  عنوان المطعم  ")]
        [Required(ErrorMessage = " إختار عنوان المطعم  ")]
        public int RestaurantDataID { get; set; }

        [DisplayName("  المنطقه  ")]
        [Required(ErrorMessage = " إختار المنطقه  ")]
        public int RegionID { get; set; }

        [DisplayName("حد المطعم ")]
        [Required(ErrorMessage = "  إدخل حد المطعم  ")]
        public decimal minPrice { get; set; }

        [DisplayName("تكلفة التوصيل ")]
        [Required(ErrorMessage = "  إدخل تكلفة التوصيل  ")]
        public decimal deliveryFeeValue { get; set; }

        //public int DistanceID1 { get; set; }
        //public int DistanceID2 { get; set; }
        //public int DistanceID3 { get; set; }
        //public int DistanceID4 { get; set; }
        //public int DistanceID5 { get; set; }





        //public int ID2 { get; set; }
        //[DisplayName("حد المطعم ")]
        //[Required(ErrorMessage = "  إدخل حد المطعم  ")]
        //public decimal minPrice2 { get; set; }
        //[DisplayName("تكلفة التوصيل ")]
        //[Required(ErrorMessage = "  إدخل تكلفة التوصيل  ")]
        //public decimal deliveryFeeValue2 { get; set; }

        //public int ID3 { get; set; }
        //[DisplayName("حد المطعم ")]
        //[Required(ErrorMessage = "  إدخل حد المطعم  ")]
        //public decimal minPrice3 { get; set; }
        //[DisplayName("تكلفة التوصيل ")]
        //[Required(ErrorMessage = "  إدخل تكلفة التوصيل  ")]
        //public decimal deliveryFeeValue3 { get; set; }

        //public int ID4 { get; set; }
        //[DisplayName("حد المطعم ")]
        //[Required(ErrorMessage = "  إدخل حد المطعم  ")]
        //public decimal minPrice4 { get; set; }
        //[DisplayName("تكلفة التوصيل ")]
        //[Required(ErrorMessage = "  إدخل تكلفة التوصيل  ")]
        //public decimal deliveryFeeValue4 { get; set; }

        //public int ID5 { get; set; }
        //[DisplayName("حد المطعم ")]
        //[Required(ErrorMessage = "  إدخل حد المطعم  ")]
        //public decimal minPrice5 { get; set; }
        //[DisplayName("تكلفة التوصيل ")]
        //[Required(ErrorMessage = "  إدخل تكلفة التوصيل  ")]
        //public decimal deliveryFeeValue5 { get; set; }

        public string CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

    }

    public class DistanceList
    {
        public int count { get; set; }
        public int Region_id { get; set; }
        public string Region_Name { get; set; }
        public int Restaurant_id { get; set; }
        public int RestaurantData_id { get; set; }
        public string Restaurant_Name { get; set; }
        public decimal deliveryFeeValue { get; set; }
        public decimal minPrice { get; set; }



    }
}