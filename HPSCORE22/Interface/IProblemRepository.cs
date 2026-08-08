using RetailCare.Models;

namespace RetailCare.Interface
{
    public interface IProblemRepository
    {
        public List<ProblemModel> GetAllProblemList(int CompanyID);
        public List<ProblemModel> GetAllProblemUsingProductID(int ProductID);
        public List<Subproblem> GetAllSubproblemProblemWise(int ProblemID);
    }
}
