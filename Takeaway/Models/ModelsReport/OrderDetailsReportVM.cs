using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class OrderDetailsReportVM
    {
      
        public List<OrderDetailsReportRow> UserOrderList { get; set; }
    }
    public class OrderDetailsReportRow
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public long ItemFoodID { get; set; }
        public string FoodName { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalPrice { get; set; }
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }

    }

}