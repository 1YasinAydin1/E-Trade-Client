//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCexample.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BasketContent
    {
        public int ID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Count { get; set; }
        public int BasketID { get; set; }
        public Nullable<int> CommentID { get; set; }
        public Nullable<bool> CommentStatus { get; set; }
    
        public virtual Basket Basket { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Product Product { get; set; }
    }
}
