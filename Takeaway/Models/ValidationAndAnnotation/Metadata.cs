using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public partial class CountryMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم البلد بالإنجليزية")]
        [Required(ErrorMessage = "  إدخل إسم البلد بالإنجليزية ")]
        public string CountryEng { get; set; }
        [DisplayName("إسم البلد بالعربية")]
        [Required(ErrorMessage = " إدخل إسم البلد بالعربية ")]
        public string CountryAr { get; set; }
        [DisplayName("التفعيل")]
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
    }

    public partial class CityMetadata
    {
        public int ID { get; set; }

        [DisplayName("إسم المدينة بالإنجليزية")]
        [Required(ErrorMessage = " إدخل إسم المدينة بالإنجليزية ")]
        public string CityEn { get; set; }
        [DisplayName("إسم المدينة بالعربية")]
        [Required(ErrorMessage = " إدخل إسم المدينة بالعربية ")]
        public string CityAr { get; set; }
        [DisplayName("إسم البلد ")]
        [Required(ErrorMessage = " إختار إسم البلد ")]
        public int CountryID { get; set; }
        [DisplayName("إسم المنطقة ")]
        public Nullable<int> RegionID { get; set; }
        [DisplayName("التفعيل")]
        public bool IsActive { get; set; }
        [DisplayName("إلغاء")]
        public bool IsDelete { get; set; }
    }

    public partial class RegionMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم المنطقة بالإنجليزية")]
        [Required(ErrorMessage = " إدخل إسم المنطقة بالإنجليزية ")]
        public string RegionEn { get; set; }
        [DisplayName("إسم المنطقة بالعربية")]
        [Required(ErrorMessage = "بالعربية إدخل إسم المنطقة ")]
        public string RegionAr { get; set; }
        [DisplayName("إسم المدينة ")]
        [Required(ErrorMessage = " إختار إسم المدينة ")]
        public Nullable<int> CityID { get; set; }
        [DisplayName("إسم البلد ")]
        [Required(ErrorMessage = " إختار إسم البلد ")]
        public Nullable<int> CountryID { get; set; }

        [DisplayName("الرقم البريدى")]
        [Required(ErrorMessage = "ادخل الرقم البريدى")]
        public string PostalCode { get; set; }

        public string NotesAr { get; set; }
        public string NotesEn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
    }

    //public partial class PayTypeMetadata
    //{
    //    public int ID { get; set; }
    //    [DisplayName("إسم العموله بالعربية")]
    //    [Required(ErrorMessage = " إدخل إسم العموله بالعربية ")]
    //    public string PayNameAr { get; set; }
    //    [DisplayName("إسم العموله بالإنجليزية")]
    //    [Required(ErrorMessage = " إدخل إسم العموله بالإنجليزية ")]
    //    public string PayNameEn { get; set; }
    //    [DisplayName("تفعيل")]
    //    public bool IsActive { get; set; }
    //    public bool IsDetete { get; set; }
    //}

    public partial class FoodTypeMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم تصنيف البحث بالعربية")]
        [Required(ErrorMessage = "ادخل إسم تصنيف البحث بالعربية")]
        public string FoodTypeNameAr { get; set; }
        [DisplayName("إسم تصنيف البحث بالإنجليزية")]
        [Required(ErrorMessage = " إدخل إسم تصنيف البحث بالإنجليزية ")]
        public string FoodTypeNameEn { get; set; }
        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }

    }
    public partial class RestaurantStatuMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم حالة المطعم  بالعربية")]
        [Required(ErrorMessage = "  إدخل إسم حالة المطعم بالعربية ")]
        public string RestaurantStatusAr { get; set; }
        [DisplayName("إسم حالة المطعم بالإنجليزية")]
        [Required(ErrorMessage = "  إدخل حالة المطعم بالإنجليزية ")]
        public string RestaurantStatusEn { get; set; }
        [DisplayName("الوصف بالعربى")]
        public string DescriptionAr { get; set; }
        [DisplayName("الوصف بالانجليزى")]
        public string DescriptionEn { get; set; }
        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
    }
    public partial class OrderStatuMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم حالة الطلب  بالعربية")]
        [Required(ErrorMessage = "  إدخل إسم حالة الطلب بالعربية ")]
        public string OrderStatus_ar { get; set; }
        [DisplayName("إسم حالة المطعم  بالإنجليزية")]
        [Required(ErrorMessage = "  إدخل إسم حالة الطلب بالإنجليزية ")]
        public string OrderStatus_En { get; set; }
        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }

        [DisplayName("تضاف للحساب")]
        public bool IsDone { get; set; }

        public bool IsDetete { get; set; }

    }

    public partial class CuisineMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم الطبق  بالعربية")]
        [Required(ErrorMessage = "  إدخل إسم الطبق بالعربية ")]
        public string CuisineAr { get; set; }
        [DisplayName("إسم الطبق  بالإنجليزية")]
        [Required(ErrorMessage = "  إدخل إسم الطبق بالإنجليزية ")]
        public string CuisineEn { get; set; }
        [DisplayName("الوصف بالعربى")]
        public string DescriptionAr { get; set; }
        [DisplayName("الوصف بالانجليزى")]
        public string DescriptionEn { get; set; }
        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }

    }

    public partial class FeeTypeMetadata
    {
        public int ID { get; set; }
        [DisplayName(" نوع العموله  بالعربية")]
        [Required(ErrorMessage = "  إدخل  نوع العموله بالعربية ")]
        public string FeeNameAr { get; set; }

        [DisplayName(" نوع العموله  بالإنجليزية")]
        [Required(ErrorMessage = "  إدخل  نوع العموله بالإنجليزية ")]
        public string FeeNameEn { get; set; }
        [DisplayName("قيمه العموله")]
        public decimal FeeValue { get; set; }
        [DisplayName("تعميم")]
        public bool IsGeneral { get; set; }
        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }

    }

    public partial class ItemFoodMetadata
    {
        public long ID { get; set; }
        [DisplayName(" اسم الوجبة  بالعربية")]
        [Required(ErrorMessage = "  إدخل  اسم الوجبة بالعربية ")]
        public string FoodName { get; set; }
        [DisplayName(" اسم الوجبة  بالإنجليزية")]
        [Required(ErrorMessage = "  إدخل  اسم الوجبة بالإنجليزية ")]
        public string FoodNameEn { get; set; }
        [DisplayName("الوصف بالعربى")]
        public string Description { get; set; }
        [DisplayName("الوصف بالانجليزى")]
        public string DescriptionEn { get; set; }
        [DisplayName("إسم الصنف ")]
        [Required(ErrorMessage = " إختار إسم الصنف ")]
        public int CategoryID { get; set; }
        [DisplayName("إسم نوع الصنف ")]
        [Required(ErrorMessage = " إختار نوع الصنف  ")]
        public int CategoryTypeID { get; set; }
        [DisplayName("ليست للبيع")]
        public bool NotforSale { get; set; }
        [DisplayName("السعر")]
        public decimal Price { get; set; }
        [DisplayName("الصورة")]
        public string Image { get; set; }
        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }

    }
    public partial class OrderMetadata
    {

        [DisplayName("كود الطلب")]
        public int ID { get; set; }
        [DisplayName(" اسم المستخدم  ")]
        [Required(ErrorMessage = "  اختر  اسم المستخدم  ")]
        public string DeliveryUserID { get; set; }
        [DisplayName(" فرع المطعم  ")]
        [Required(ErrorMessage = "  اختر   فرع المطعم  ")]
        public int RestaurantDataID { get; set; }
        [DisplayName(" حالة الطلب  ")]
        [Required(ErrorMessage = "  اختر  حالة الطلب  ")]
        public int OrderStatusID { get; set; }
        [DisplayName(" السعر الكلي   ")]
        [Required(ErrorMessage = "  إدخل  السعر الكلي  ")]
        public decimal TotalPrice { get; set; }
        [DisplayName(" تاريخ الطلب   ")]
        [Required(ErrorMessage = "  إدخل  تاريخ الطلب  ")]
        public System.DateTime OrderDate { get; set; }
        [DisplayName(" وقت التوصيل المتوقع   ")]
        public Nullable<System.TimeSpan> Delivery_estimation { get; set; }
        [DisplayName(" الوصف")]
        public string Description { get; set; }
        //[DisplayName(" نوع الدفع  ")]
        //[Required(ErrorMessage = "  اختر   نوع الدفع ")]
        //public int PayType { get; set; }
        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }


    }

    public partial class RestaurantMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم  المطعم  بالأنجليزية")]
        [Required(ErrorMessage = "  إدخل إسم  المطعم بالأنجليزية ")]
        public string RestaurantNameEn { get; set; }
        [DisplayName("إسم  المطعم  بالعربية")]
        [Required(ErrorMessage = "  إدخل إسم  المطعم بالعربية ")]
        public string RestaurantName { get; set; }

        [DisplayName("وصف  المطعم  بالعربية")]
        //Required(ErrorMessage = "  إدخل وصف  المطعم بالعربية ")]
        public string RestaurantDescrption { get; set; }
        [DisplayName("وصف  المطعم  بالأنجليزية")]
        //[Required(ErrorMessage = "  إدخل وصف  المطعم بالأنجليزية ")]
        public string RestaurantDescrptionEn { get; set; }
        [DisplayName("رقم  المطعم  ")]
        [Required(ErrorMessage = "  إدخل رقم  المطعم  ")]

        public string RestaurantPhone { get; set; }
        public Nullable<int> ChainRestaurants_id { get; set; }

        //[DisplayName("طريقة الدفع ")]
        //[Required(ErrorMessage = "  إدخل طريقة الدفع  ")]
        //public Nullable<int> FeeTypeID { get; set; }

        [DisplayName("طريقة التوصيل ")]
        [Required(ErrorMessage = "  إدخل طريقة التوصيل  ")]
        public int DeliveryWayID { get; set; }

        [DisplayName("حالة المطعم ")]
        [Required(ErrorMessage = "  إدخل حالة المطعم  ")]
        public int RestaurantStatus { get; set; }


        //[DisplayName("من ")]
        //[Required(ErrorMessage = "  إدخل من  ")]
        //public System.TimeSpan Openingtime { get; set; }

        //[DisplayName("إلى ")]
        //[Required(ErrorMessage = "  إدخل إلى  ")]
        //public Nullable<System.TimeSpan> Closetime { get; set; }

        //[DisplayName("مفتوح/مغلق ")]
        //[Required(ErrorMessage = "  إدخل مفتوح/مغلق  ")]
        //public Nullable<int> RestaurantOpeningID { get; set; }

        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_modified { get; set; }
        [DisplayName("صاحب المطعم ")]
        [Required(ErrorMessage = "  إدخل صاحب المطعم  ")]
        public string UserID { get; set; }
        public Nullable<System.DateTime> MembershipExpired { get; set; }
        public string MapLatitude { get; set; }
        public string MapLongitude { get; set; }
        public string Logo { get; set; }

        [DisplayName("نجوم المطعم ")]
        [Required(ErrorMessage = "  إدخل نجوم المطعم  ")]
        public int Stars { get; set; }
    }

    public partial class PackageMetadata
    {
        public int ID { get; set; }
        [DisplayName("إسم الباقة بالعربية")]
        [Required(ErrorMessage = "  إدخل إسم الباقة بالعربية ")]
        public string PackageNameAr { get; set; }

        [DisplayName("إسم الباقة بالأنجليزية")]
        [Required(ErrorMessage = "  إدخل إسم الباقة بالأنجليزية ")]
        public string PackageNameEn { get; set; }

        [DisplayName(" السعر")]
        [Required(ErrorMessage = "  إدخل السعر ")]
        public decimal PackagePrice { get; set; }

        [DisplayName(" الفترة")]
        [Required(ErrorMessage = "  إدخل الفترة ")]
        public int PackagePeriod { get; set; }

        [DisplayName(" تاريخ البداية")]
        [Required(ErrorMessage = "  إدخل تاريخ البداية ")]
        public System.DateTime PackageStartDate { get; set; }

        [DisplayName(" تاريخ النهاية")]
        [Required(ErrorMessage = "  إدخل تاريخ النهاية ")]
        public System.DateTime PackageEndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetete { get; set; }
    }

    public partial class OfferMetadata
    {
        public int ID { get; set; }

        [DisplayName(" العرض")]
        [Required(ErrorMessage = "  إدخل العرض ")]
        public int SubjectID { get; set; }

        [DisplayName(" نوع العرض")]
        [Required(ErrorMessage = "  إدخل نوع العرض ")]
        public int OfferType { get; set; }


        [DisplayName(" نوع الخصم")]
        [Required(ErrorMessage = "  إدخل نوع الخصم ")]
        public int FeeType { get; set; }
        [DisplayName(" قيمة الخصم")]
        [Required(ErrorMessage = "  إدخل قيمة الخصم ")]
        public decimal FeeValue { get; set; }

        [DisplayName(" تاريخ البداية")]
        [Required(ErrorMessage = "  إدخل تاريخ البداية ")]
        public System.DateTime StartDate { get; set; }

        [DisplayName(" تاريخ النهاية")]
        [Required(ErrorMessage = "  إدخل تاريخ النهاية ")]
        public System.DateTime EndDate { get; set; }

        public string UserID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
    public partial class SizeMetadata
    {
        public int ID { get; set; }

        [DisplayName("الحجم باللغه العربيه")]
        [Required(ErrorMessage = "  إدخل الحجم باللغه العربيه ")]
        public string SizeAR { get; set; }

        [DisplayName("الحجم باللغه الانجليزيه")]
        [Required(ErrorMessage = "  إدخل الحجم باللغه العربيه ")]
        public string SizeEN { get; set; }


        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }


    }
    public partial class CategoryMetadata
    {
        public int ID { get; set; }

        [DisplayName("إسم القائمه بالعربية")]
        [Required(ErrorMessage = " إدخل إسم قائمه الطعام بالعربية ")]
        public string CategoryName { get; set; }

        [DisplayName("إسم القائمه بالإنجليزية")]
        [Required(ErrorMessage = " إدخل إسم قائمه الطعام بالإنجليزية ")]
        public string CategoryNameEn { get; set; }

        [DisplayName("وصف القائمه بالعربية")]
        [Required(ErrorMessage = " إدخل وصف القائمه بالعربية ")]
        public string DescriptionAr { get; set; }

        [DisplayName("وصف القائمه بالإنجليزية")]
        [Required(ErrorMessage = " إدخل وصف القائمه بالإنجليزية ")]
        public string DescriptionEn { get; set; }


        [DisplayName("المطعم")]
        [Required(ErrorMessage = "اختر المطعم")]
        public string RestaurantID { get; set; }

        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }

        public bool IsDetete { get; set; }
    }


    public partial class CoboneMetadata
    {
        public int ID { get; set; }
        
        [DisplayName("اسم الكوبون بالعربية")]
        [Required(ErrorMessage = "  إدخل اسم الكوبون بالعربية ")]
        public string CoboneNameAr { get; set; }
        [DisplayName("اسم الكوبون بالإنجليزية")]
        [Required(ErrorMessage = "  إدخل اسم الكوبون بالإنجليزية ")]
        public string CoboneNameEn { get; set; }
        [DisplayName(" نوع الكوبون")]
        [Required(ErrorMessage = "  إدخل نوع الكوبون ")]
        public int CoboneTypeID { get; set; }
        [DisplayName(" قيمة الكوبون")]
        [Required(ErrorMessage = "  إدخل قيمة الكوبون ")]
        public decimal CoboneTypeValue { get; set; }
        [DisplayName("الفترة باليوم")]
        [Required(ErrorMessage = " إدخل الفترة باليوم  ")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "إدخل الفترة باليوم .")]
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }

    public partial class UserMetadata
    {
        [DisplayName("تليفون العميل")]
        public string ContactPhone { get; set; }
        [DisplayName("اسم العميل")]
        public string ContactName { get; set; }

    }
    public partial class UserDataMetadata
    {
        [DisplayName("عنوان العميل")]
        public string Address { get; set; }
       

    }
}