using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class RestaurantDailyIncomeReportVM
    {
       
        public List<RestaurantDailyIncomeReportRow> RestaurantDailyIncomeList { get; set; }
    }
    public class RestaurantDailyIncomeReportRow
    {
        public int restaurantID { get; set; }
        public string restaurantName { get; set; }
        public decimal totalPrice { get; set; }
        public decimal Income { get; set; }


    }

}