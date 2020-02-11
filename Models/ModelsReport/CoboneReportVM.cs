using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Takeaway.Models
{
    public class CoboneReportVM
    {
        public int Counts { get; set; }
        public List<CoboneReportRow> CoboneList { get; set; }
    }
    public class CoboneReportRow
    {
        public string CoboneUserID { get; set; }
        public string CoboneUserName { get; set; }
        public string CoboneName { get; set; }
        public int CoboneID { get; set; }
        public string CoboneSerial { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
       

    }

}