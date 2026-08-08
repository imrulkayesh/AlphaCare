using System.ComponentModel.DataAnnotations;
namespace RetailCare.Models.ServiceModel
{
    public class ItemModel
    {
        [Key]
        public int ITEMID { get; set; }
        [Display(Name = "Class Code")]
        [Required(ErrorMessage = "Please Enter Class Code")]
        public int ITEMCODE { get; set; }
        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Please Enter Class Name")]
        public string? ITEMNAME { get; set; }
        [Display(Name = "Description")]
        public string? DESCRIPTION { get; set; }
        public string? INSTALLATIONTYPE { get; set; }
        [Display(Name = "Company Namne")]
        public int COMPANYID { get; set; }
        [Display(Name = "Group Namne")]
        [Required(ErrorMessage = "Please Enter Class Code")]
        public int GROUPID { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string? ENTRYPC { get; set; }
        public string? MODIFIEDPC { get; set; }
        public int? ISACTIVE { get; set; }
        //public int WarrantyStatus { get; set; }
        //public string? ItemNameCRM { get; set; }
    }
}
