using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class RestaurantReportVM
    {
        public int RestaurantCounts { get; set; }
        public List<RestaurantReportRow> RestaurantList { get; set; }
    }
    public class RestaurantReportRow
    {
        public string RestaurantName { get; set; }
        public string RestaurantPhone { get; set; }
        public int RestaurantStatus { get; set; }
        public string RestaurantStatusName { get; set; }

    }

}