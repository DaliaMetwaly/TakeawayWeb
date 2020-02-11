using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class RestaurantRatesReportVM
    {
       
        public List<RestaurantRatesReportVMtRow> restaurantList { get; set; }
    }
    public class RestaurantRatesReportVMtRow
    {
        public string RestaurantName { get; set; }
        public decimal Rate { get; set; }

    }

}