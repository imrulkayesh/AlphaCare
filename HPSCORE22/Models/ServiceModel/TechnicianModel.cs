using System.ComponentModel.DataAnnotations;
namespace RetailCare.Models.ServiceModel
{
    public class TechnicianModel
    {
        [Key]
        public int TECHNICIANID { get; set; }
        [Display(Name = "Technician Code")]
        [Required(ErrorMessage = "Please Enter Technician Code")]
        public  string? TECHNICIANCODE { get; set; }
        [Display(Name = "Technician Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        public string? TECHNICIANNAME { get; set; }
        [Display(Name = "Technician StaffID")]
        [Required(ErrorMessage = "Please Enter StaffID")]
        public  int? STAFFID { get; set; }
        [Display(Name = "Technician Contact Number")]
        public string? CONTACTNO { get; set; }
        [Display(Name = "Technician Email Address")]
        public string? EMAIL { get; set; }
        [Display(Name = "Technician Address")]
        public string? ADDRESS { get; set; }
        public int? DEPARTMENTID { get; set; }
        public int? DESIGNATIONID { get; set; }
        public int? SUPERVISORID { get; set; }
        [Display(Name = "Zone Name")]
        public int? ZONEID { get; set; }
        public int? GROUPID { get; set; }
        [Display(Name = "Company Name")]
        public int? COMPANYID { get; set; }
        public int? ACTIVE { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        [Display(Name = "Active")]
        public bool? CheckedBox { get; set; }
        //public string EntryPC { get; set; }
        //public string ModifiedPC { get; set; }
        // For Dropdown List
        public int ITEMID { get; set; }

        // Reporting Items
        public string STATUS { get; set; }
        public string ZONENAME { get; set; }
    }
    public class TECHNICIANSASSIGNPRODUCT
    {
        public int TECHNICIANID { get; set; }
        public int PRODUCTID { get; set; }
        public int ACTIVE { get; set; }
    }
}
