using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class RestaurantVM : User
    {
        public RestaurantVM()
        {
            RestaurantDataList = new List<RestaurantData>();
        }

        public int ResID { get; set; }
        [DisplayName("إسم  المطعم  بالأنجليزية")]
        [Required(ErrorMessage = "  إدخل إسم  المطعم بالأنجليزية ")]
        public string RestaurantNameEn { get; set; }
        [DisplayName("إسم  المطعم  بالعربية")]
        [Required(ErrorMessage = "  إدخل إسم  المطعم بالعربية ")]
        public string RestaurantName { get; set; }

        [DisplayName("وصف  المطعم  بالعربية")]
       // [Required(ErrorMessage = "  إدخل وصف  المطعم بالعربية ")]
        public string RestaurantDescrption { get; set; }

        [DisplayName("وصف  المطعم  بالأنجليزية")]
        //[Required(ErrorMessage = "  إدخل وصف  المطعم بالأنجليزية ")]
        public string RestaurantDescrptionEn { get; set; }
        [DisplayName("رقم  الهاتف  ")]
        [Required(ErrorMessage = "  إدخل رقم  الهاتف  ")]

        public string RestaurantPhone { get; set; }
        public Nullable<int> ChainRestaurants_id { get; set; }
        //[DisplayName("تكلفة التوصيل ")]
        //[Required(ErrorMessage = "  إدخل تكلفة التوصيل  ")]
        //public Nullable<int> FeeTypeID { get; set; }

        [DisplayName("طريقة التوصيل ")]
        [Required(ErrorMessage = "  إدخل طريقة التوصيل  ")]
        public int DeliveryWayID { get; set; }

        [DisplayName("حالة المطعم ")]
        [Required(ErrorMessage = "  إدخل حالة المطعم  ")]
        public int RestaurantStatus { get; set; }


        //[DisplayName("من ")]
        //[Required(ErrorMessage = "  إدخل من  ")]
        //public string Openingtime { get; set; }

        //[DisplayName("إلى ")]
        //[Required(ErrorMessage = "  إدخل إلى  ")]
        //public string Closetime { get; set; }


        //[DisplayName("مفتوح/مغلق ")]
        //[Required(ErrorMessage = "  إدخل مفتوح/مغلق  ")]
        //public Nullable<int> RestaurantOpeningID { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_modified { get; set; }
        [DisplayName("صاحب المطعم ")]
        [Required(ErrorMessage = "  إدخل صاحب المطعم  ")]
        public string UserID { get; set; }

        [DisplayName("البلد")]
        [Required(ErrorMessage = "اختر البلد")]
        public int? CountryID { get; set; }


        [DisplayName("المدينه")]
        [Required(ErrorMessage = "اخنر المدينه")]
        public int? CityID { get; set; }

        [DisplayName("المنطقه")]
        [Required(ErrorMessage = "اختر المنطقه")]
        public int? RegionID { get; set; }

        [DisplayName("العنوان")]
        [Required(ErrorMessage = "ادخل العنوان")]
        public string Address { get; set; }




        public Nullable<System.DateTime> MembershipExpired { get; set; }
        //[Required(ErrorMessage = "ادخل خط الطول")]
        public string MapLatitude { get; set; }

        //[Required(ErrorMessage = "ادخل خط العرض")]
        public string MapLongitude { get; set; }
        public string Logo { get; set; }

        [DisplayName("نجوم المطعم ")]
        [Required(ErrorMessage = "  إدخل نجوم المطعم  ")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "إدخل نجوم المطعم بالأرقام.")]
        public int Stars { get; set; }

        public List<RestaurantData> RestaurantDataList { get; set; }

        public bool wizard { get; set; }
    }

    public class AddressList
    {
        public int count { get; set; }

        public string Address { get; set; }

        public int Region_id { get; set; }

        public string Region_Name { get; set; }

        public string MapLatitude { get; set; }

        public string MapLongitude { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

    }
}