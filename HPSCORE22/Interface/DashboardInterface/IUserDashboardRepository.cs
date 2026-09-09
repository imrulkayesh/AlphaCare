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
    }
}
