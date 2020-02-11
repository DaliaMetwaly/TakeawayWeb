using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class RestaurantAppointmentVM 
    {
        public int ID1 { get; set; }
        public int ID2 { get; set; }
        public int ID3 { get; set; }
        public int ID4 { get; set; }
        public int ID5 { get; set; }
        public int ID6 { get; set; }
        public int ID7 { get; set; }

        [DisplayName("  السبت  ")]
        public int Saturday  { get; set; }
        [DisplayName("  الاحد  ")]
        public int Sunday    { get; set; }
        [DisplayName("  الاثنين  ")]
        public int Monday    { get; set; }
        [DisplayName("  الثلاثاء  ")]
        public int Tuesday   { get; set; }
        [DisplayName("  الاربعاء  ")]
        public int Wednesday { get; set; }
        [DisplayName("  الخميس  ")]
        public int Thursday  { get; set; }
        [DisplayName("  الجمعه  ")]
        public int Friday  { get; set; }
       
       
        [DisplayName("  المطعم  ")]
        [Required(ErrorMessage = " إختار المطعم  ")]
        public int RestaurantID { get; set; }
        

        public string From1 { get; set; }
        public string to1 { get; set; }
        public string From2 { get; set; }
        public string to2 { get; set; }
        public string From3 { get; set; }
        public string to3 { get; set; }
        public string From4 { get; set; }
        public string to4 { get; set; }
        public string From5 { get; set; }
        public string to5 { get; set; }
        public string From6 { get; set; }
        public string to6 { get; set; }
        public string From7 { get; set; }
        public string to7 { get; set; }


        public string From12 { get; set; }
        public string to12 { get; set; }
        public string From22 { get; set; }
        public string to22 { get; set; }
        public string From32 { get; set; }
        public string to32 { get; set; }
        public string From42 { get; set; }
        public string to42 { get; set; }
        public string From52 { get; set; }
        public string to52 { get; set; }
        public string From62 { get; set; }
        public string to62 { get; set; }
        public string From72 { get; set; }
        public string to72 { get; set; }


        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

    }
}