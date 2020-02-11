using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class ActiveCustomersReportVM
    {
       
        public List<ActiveCustomersReportVMtRow> customerList { get; set; }
    }
    public class ActiveCustomersReportVMtRow
    {
        public string CustomerName { get; set; }
        public int NumofOrders { get; set; }

    }

}