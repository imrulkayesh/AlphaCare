namespace AlphaCare.Models.Dashboard_Model
{
    public class UserDashboardViewModel
    {
        public List<SummaryCardModel> SummaryCards { get; set; }=new List<SummaryCardModel>();
        public DueSummaryCardModel DueSummaryCards { get; set; }=new DueSummaryCardModel();
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
}
