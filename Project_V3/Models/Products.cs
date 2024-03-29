//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_V3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.Orderdetails = new HashSet<Orderdetails>();
        }
    
        public int SN { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<bool> Discontinued { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoType { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int CategorySN { get; set; }
        public int StoreSN { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orderdetails> Orderdetails { get; set; }
        public virtual Stores Stores { get; set; }
    }
}
