using RetailCare.Models;

namespace RetailCare.Interface.CRMInterface
{
    public interface IStatusRepository
    {
        public List<StatusModel> GetAllStatus(int CompanyID);
    }
}
