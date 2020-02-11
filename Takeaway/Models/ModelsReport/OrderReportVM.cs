using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class OrderReportVM
    {
        public int Counts { get; set; }
        public List<OrderReportRow> OrderList { get; set; }
    }
    public class OrderReportRow
    {
        public string DeliveryUserID { get; set; }
        public string DeliveryUserName { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantID { get; set; }
        public int OrderStatusID { get; set; }
        public string OrderStatusName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal orderFee { get; set; }
        public System.DateTime OrderDate { get; set; }

    }

}