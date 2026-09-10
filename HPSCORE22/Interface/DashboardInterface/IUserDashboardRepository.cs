using AlphaCare.Models.Dashboard_Model;
using RetailCare.Models.CRMModels;

namespace AlphaCare.Interface.DashboardInterface
{
    public interface IUserDashboardRepository
    {
        public SummaryCardModel GetComplaintDashboard(int companyId, string ShowrromCode);
        public List<CompalinModel> GetPendingComplain(int CompanyID, string ShowrromCode);
        public List<CompalinModel> GetDueComplain(int CompanyID, string ShowrromCode);
        public DueSummaryCardModel GetDueComplaintDashboard(int companyId, string ShowrromCode);
        Task<List<Top10TechnicianModel>> GetTop10TechnicianSolvedATA(int companyId, string showroom);
        Task<List<ChartModels>> GetTop5ProblemType(int companyId, string showroom);
        Task<List<Top10TechnicianModel>> GetLowest10TechnicianSolvedATA(int companyId, string showroom);
        public PieChartModel GetCurrentDayTicketStatus(int companyId, string ShowrromCode);
        Task<List<Top10TechnicianModel>> GetTop5Showroom(int companyId);
        Task<List<TimeTrandModel>> GetCurrentMonthTimeTrand(int companyId);
    }
}
