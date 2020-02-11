using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Takeaway.Models
{
    public class ChangeUserPassword
    {
        public string UserId { get; set; }
        [Display(Name = "إسم المستخدم")]
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name ="كلمة المرور الجديدة")]
        [Required(ErrorMessage = "أدخل كلمة المرور الجديدة")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور غير متطابقة ")]
        public string RepeatPassword { get; set; }


        [Required(ErrorMessage = "يجب ادخال البريد الالكترونى الحالى ")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "البريد الالكترونى الحالى")]
        public string OldEmail { get; set; }

        [Required(ErrorMessage = "يجب ادخال البريد الالكترونى الجديد ")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "البريد الالكترونى الجديد")]
        public string NewEmail { get; set; }



    }
}