using System.ComponentModel.DataAnnotations;

namespace RetailCare.Models
{
    public class BrandModel
    {
        [Key]
        public int GROUPID { get; set; }

        public int GROUPCODE { get; set; }
        [Display(Name = "Brand Title")]
        [Required(ErrorMessage = "Please Brand Title")]
        public string? GROUPNAME { get; set; }
        public int PRODUCTID { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string? ENTRYPC { get; set; }
        public string? MODIFIEDPC { get; set; }
        public int COMPANYID { get; set; }
    }
}
