using System.ComponentModel.DataAnnotations;
namespace RetailCare.Models.CRMModels
{

    public class CompalinModel
    {
        [Key]
        public int TICKETID { get; set; }
        [Display(Name = "Ticket Code")]
        public string? TICKETCODE { get; set; }
        // Form Input Fileds start
        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Please Enter Customer Name")]
        public string? CUSTOMERNAME { get; set; }
        [Display(Name = "Customer Contact")]
        public string? CONTACTNO { get; set; }
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Please Enter Address")]
        public string? LOCATION { get; set; }   
        [Display(Name = "Status")]
        public int STATUSID { get; set; }
        [Display(Name = "Problem Type")]
        public int PROBLEMTYPEID { get; set; }
        [Display(Name = "Complain Date")]
       // [DataType(DataType.Date)]
        public DateTime? COMPLAINDATE { get; set; }

        [Display(Name = "Assign Technician")]
        [Required(ErrorMessage = "Please select a Technician")]
        public int TECHNICIANID { get; set; }
        [Display(Name = "Showroom Code")]
        [Required(ErrorMessage = "Please select  Showroom")]
        public string? SHOWROOM { get; set; }
        public int? ZONEID { get; set; }
        // Backend Fields
        public int? ISSENDFEEDBACK { get; set; }
        public int COMPANYID { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public string? ENTRYPC { get; set; }
        public string? MODIFIEDPC { get; set; }
        public int? ISACTIVE { get; set; }
        // Reporting Fields
        public string? PROBLEMNAME { get; set; }
        public string? STATUSNAME { get; set; }
        public string? PRODUCTNAME { get; set; }
        public string? PRMODELNAME { get; set; }
        public string? ZONENAME { get; set; }
        public string? TECHNICIANNAME { get; set; }
        public string? ITEMNAME { get; set; }
        // New Table Column
        public DateTime? ASSIGNDATE { get; set; }
        public DateTime? WORKINGDATE { get; set; }
        public int CompletedDiff { get; set; }
        public int PendingDate { get; set; }
    }
    public class ComplainProblemModel
    {
        public string? TICKETCODE { get; set; }
        public int PROBLEMID { get; set; }
        public int QUANTITY { get; set; }
        public string? REMARKS { get; set; }

        // Navigation property to the CompalinModel
        public string? PROBLEMNAME { get; set; }
    }
}
