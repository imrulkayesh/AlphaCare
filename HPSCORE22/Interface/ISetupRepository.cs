using AlphaCare.Repositories;

namespace AlphaCare.Interface
{
    public interface ISetupRepository
    {
        public SMSSendingModel GetCompanySMSApiDetals(int CompanyID);
    }
}
