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
    
    public partial class Basket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Basket()
        {
            this.BasketContent = new HashSet<BasketContent>();
        }
    
        public int ID { get; set; }
        public Nullable<int> accountID { get; set; }
        public Nullable<System.DateTime> orderDate { get; set; }
        public Nullable<int> totalPrice { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> AddressID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BasketContent> BasketContent { get; set; }
    }
}