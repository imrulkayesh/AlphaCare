using RetailCare.Models;
using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceModel;

namespace RetailCare.Interface
{
    public interface IReportGenerationRepository
    {
        public List<CompalinModel> GetComplainReport(FilteringOption FilteringValues, int CompanyID);
        public List<FeedBackReportModel> GetFeedBackReport(FilteringOption FilteringValues, int CompanyID);
        public List<FeedbackImageModel> GetAllFeedbackImage(string TicketID);
        public List<TechnicianModel> GetAllTechTotalSolveData(FilteringOption FilteringValues, int CompanyID);
        public List<CompalinModel> GetAllSolveDataTechWise(int techid, int CompanyID, DateTime StartDate, DateTime EndDate);
    }
}
