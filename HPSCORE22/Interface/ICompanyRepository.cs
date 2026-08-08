using RetailCare.Models;

namespace RetailCare.Interface
{
    public interface ICompanyRepository
    {
        public CompanyModel GetSingleCompanyDetails(int CompanyID);
    }
}
