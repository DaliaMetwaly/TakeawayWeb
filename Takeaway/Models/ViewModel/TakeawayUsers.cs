using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class TakeawayUser  
    {
        public TakeawayUser()
        {
           // UserDataList = new List<UserData>();
        }
        public string ID { get; set; }

        public int isClient { get; set; }
        [DisplayName("اسم العميل")]
        [Required(ErrorMessage = "ادخل الاسم")]
        public string ContactName { get; set; }

        [DisplayName("إسم المستخدم")]
        [Required(ErrorMessage = " إدخل إسم المستخدم ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " إدخل كلمة المرور ")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متوافقة")]
        public string ConfirmPassword { get; set; }

        [DisplayName("البريد الالكترونى")]
        [Required(ErrorMessage = "ادخل البريد الالكترونى")]
        public string ContactEmail { get; set; }

        [DisplayName("التليفون")]
        [Required(ErrorMessage = "ادخل التليفون")]
        public string ContactPhone { get; set; }

        [DisplayName("طريقه الدفع")]
        [Required(ErrorMessage = "اختر طريقه الدفع")]
        public int PayTypeID { get; set; }

        [DisplayName("الصلاحية")]
        [Required(ErrorMessage = "اختر الصلاحيه")]
        public string RoleID { get; set; }

        [DisplayName("تفعيل")]
        public bool IsActive { get; set; }

        public bool IsDetete { get; set; }


        [DisplayName("عنوان العميل")]
        public List<string> Address { get; set; }

        public List<int> Region_id { get; set; }
        public List<string> RegionName { get; set; }
        public List<string> MapLatitude { get; set; }
        public List<string> MapLongitude { get; set; }
        //public List<UserData> UserDataList  { get; set; }

    }
}