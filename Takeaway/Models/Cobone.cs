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
    
    public partial class Cobone
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cobone()
        {
            this.CoboneUsers = new HashSet<CoboneUser>();
        }
    
        public int ID { get; set; }
        public string CoboneNameAr { get; set; }
        public string CoboneNameEn { get; set; }
        public int CoboneTypeID { get; set; }
        public decimal CoboneTypeValue { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    
        public virtual CoboneType CoboneType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoboneUser> CoboneUsers { get; set; }
    }
}