//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SellingwithEntityframwork.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PD_ORDER
    {
        public string OD_ID { get; set; }
        public string PD_ID { get; set; }
        public Nullable<System.DateTime> OD_DATE { get; set; }
        public Nullable<decimal> OD_QUANTITY { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public string OD_STATUS { get; set; }
    }
}
