using RetailCare.Models;

namespace AlphaCare.Models.Dashboard_Model
{
    public class UserDashboardViewModel
    {
        public UserInfoModel Userinformation { get; set; }=new UserInfoModel();
        public SummaryCardModel SummaryCards { get; set; }=new SummaryCardModel();
        public DueSummaryCardModel DueSummaryCards { get; set; }=new DueSummaryCardModel();
        public List<Top10TechnicianModel> Top10TechnicianList { get; set; }=new List<Top10TechnicianModel>();
        public List<Top10TechnicianModel> Lowest10TechnicianList { get; set; } = new List<Top10TechnicianModel>();
        public List<Top10TechnicianModel> Top5Showroom { get; set; } = new List<Top10TechnicianModel>();
    }
    public class SummaryCardModel
    {
        public int CurrentMonthCount { get; set; }
        public int TodayCount { get; set; }
        public int PendingFeedbackCount { get; set; }
        public int OldPendingCount { get; set; }
    }
    public class DueSummaryCardModel
    {
        public double THIS_MONTH_SOLVED_PERCENTAGE { get; set; }
        public double LAST_MONTH_SOLVED_PERCENTAGE { get; set; }
        public double SOLVE_GROWTH_PERCENTAGE { get; set; }
        public double ACTIVE_TECHNICIAN_COUNT { get; set; }
    }
    public class Top10TechnicianModel
    {
        public string? TECHNICIANNAME { get; set; }
        public string? STAFFID { get; set; }
        public int CURRENT_MONTH_SOLVE { get; set; }
        public int PREVIOUS_MONTH_SOLVE { get; set; }
        public int SOLVE_GROWTH { get; set; }
        public decimal SOLVE_GROWTH_PERCENTAGE { get; set; }
    }
    public class ChartModels
    {
        public string? CurrentMonthLabel { get; set; }
        public int CurrentMonthValue { get; set; }
        public string? PreviousMonthLabel { get; set; }
        public int PreviousMonthValue { get; set; }
    }
    public class PieChartModel
    {
        public int TotalSolve { get; set; }
        public int TotalUnsolved { get; set; }
        public int ToalCancelled { get; set; }
    }
    public class TimeTrandModel
    {
        public string? ReportedDate { get; set; }
        public int TotalTickets { get; set; }
        public int SolvedTickets { get; set; }
    }
}
