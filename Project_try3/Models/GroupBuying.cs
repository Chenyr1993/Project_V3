//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_try3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class GroupBuying
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GroupBuying()
        {
            this.Orders = new HashSet<Orders>();
        }
        [Key]
        public string ID { get; set; }
        [DisplayName("店家代碼")]
        public int StoreSN { get; set; }
        [DisplayName("建立日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [DisplayName("開團日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "請選擇日期時間")]
        public System.DateTime Startdate { get; set; }
        [DisplayName("開團人姓名")]
        [Required]
        public int CreatedPerson { get; set; }
        [DisplayName("標題")]
        //[Required(ErrorMessage = "欄位必填")]
        [MaxLength(30, ErrorMessage = "字數上限50字")]
        public string Title { get; set; }
        [DisplayName("描述說明")]
        [MaxLength(200, ErrorMessage = "字數上限200字")]
        public string Description { get; set; }
        [DisplayName("送單時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "請選擇日期時間")]
        public System.DateTime RequireDate { get; set; }

        [DisplayName("關團時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "請選擇日期時間")]
        public System.DateTime CloseDate { get; set; }

        [DisplayName("送貨地址")]
        [Required(ErrorMessage = "欄位必填")]
        [MaxLength(50, ErrorMessage = "字數上限50字")]
        [MinLength(14, ErrorMessage = "字數不可低於14個字")]
        public string ShipAddress { get; set; }
        [DisplayName("取貨方式")]
        [Required(ErrorMessage = "請選擇取貨方式")]
        public int DeliverySN { get; set; }
        [DisplayName("最低成團金額")]
        [Range(0, short.MaxValue, ErrorMessage = "金額不可小於0")]
        public Nullable<decimal> LimitMoney { get; set; }
        [DisplayName("最低成團數量")]
        [Range(0, short.MaxValue, ErrorMessage = "數量不可小於0")]
        public Nullable<int> LimitNumber { get; set; }
        [DisplayName("開團狀態")]
        public bool Continued { get; set; }
        [DisplayName("付款方式")]
        [Required(ErrorMessage = "請選擇取貨方式")]
        public int PaySN { get; set; }

        public virtual Delivery Delivery { get; set; }
        public virtual Members Members { get; set; }
        public virtual PayType PayType { get; set; }
        public virtual Stores Stores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
