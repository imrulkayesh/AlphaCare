using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceModel;
using System.ComponentModel.DataAnnotations;

namespace RetailCare.Models
{
    public class FilteringOption
    {
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Please Enter start Date")]
        public DateTime? StartDate { get; set; } = DateTime.Now.AddMonths(-1);
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "Please Enter End Date")]
        public DateTime? EndDate { get; set; } = DateTime.Now;
        [Display(Name = "Status")]
        public int? StatusID { get; set; }
        [Display(Name = "Technician")]
        public int? TechnicianID { get; set; }
    }
    public class ConstantValues
    {
        // Need To be changed the URL here  
        public readonly string _ImageAPIURL = "http://pmc.prangroup.com/AlphaCareAPI/";
        // Need to be changed the URL here 
    }
    public class ReportGenerationViewModel
    {
        // filtering option
        public FilteringOption FilteringOption { get; set; } = new FilteringOption();
        public List<StatusModel> StatusList { get; set; } = new List<StatusModel>();
        public List <TechnicianModel> TechnicianList { get; set; } = new List<TechnicianModel>();
        // filtering list
        public List<CompalinModel> ComplainList { get; set; } = new List<CompalinModel>();
        public List<FeedBackReportModel> FeedbackReport { get; set; } = new List<FeedBackReportModel>();
        public ConstantValues ConstantValues { get; set; } = new ConstantValues();
        public List<TechnicianModel> TechniciansListReport { get; set; } = new List<TechnicianModel>();
    }

}
