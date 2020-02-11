using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class ChangePasswordVM
    {

        public string UserId { get; set; }

        public string UserName { get; set; }
        public string ErrorMessage { get; set; }

        [Required(ErrorMessage = "يجب ادخال كلمه المرور الحاليه")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمه المرور الحاليه")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage ="يجب ادخال كلمه المرور الجديده")]
        [StringLength(100, ErrorMessage = "كلمه المرور الجديده يجب ان لا تقل عن 6 حروف", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمه المرور الجديدة")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تاكيد كلمه المرور ")]
        [Compare("NewPassword", ErrorMessage = "تاكيد كلمه المرور  لا تطابق كلمه المرور الجديدة.")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "يجب ادخال البريد الالكترونى الحالى ")]
        [DataType(DataType.EmailAddress )]
        [Display(Name = "البريد الالكترونى الحالى")]
        public string OldEmail { get; set; }

        [Required(ErrorMessage = "يجب ادخال البريد الالكترونى الجديد ")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "البريد الالكترونى الجديد")]
        public string NewEmail { get; set; }

        public List<UserData> UserDataList { get; set; }

        


    }
}