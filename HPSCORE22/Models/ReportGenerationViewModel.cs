using RetailCare.Models.CRMModels;
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
    }
    public class ReportGenerationViewModel
    {
        public FilteringOption FilteringOption { get; set; } = new FilteringOption();
        public List<CompalinModel> ComplainList { get; set; } = new List<CompalinModel>();
        public List<FeedBackReportModel> FeedbackReport { get; set; } = new List<FeedBackReportModel>();
        public List<StatusModel> StatusList { get; set; } = new List<StatusModel>();
    }

}
