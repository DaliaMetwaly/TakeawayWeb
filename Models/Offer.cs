//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Takeaway.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Offer
    {
        public int ID { get; set; }
        public long SubjectID { get; set; }
        public int OfferType { get; set; }
        public int FeeType { get; set; }
        public decimal FeeValue { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}