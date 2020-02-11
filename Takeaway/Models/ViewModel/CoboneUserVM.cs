using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class CoboneUserVM 
    {
        public int ID { get; set; }
        [DisplayName("الكوبون")]
        [Required(ErrorMessage = "  إختار الكوبون ")]
        public int CoboneID { get; set; }
       

        [DisplayName("عدد أكواد الكوبون ")]
        [Required(ErrorMessage = "  إدخل عدد أكواد الكوبون  ")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "إدخل عدد أكواد الكوبون بالأرقام.")]
        public int Count { get; set; }

        public bool IsActive { get; set; }

    }
}