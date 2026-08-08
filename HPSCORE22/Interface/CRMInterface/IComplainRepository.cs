using RetailCare.Models.CRMModels;

namespace RetailCare.Interface.CRMInterface
{
    public interface IComplainRepository
    {
        public bool AddNewComplain(CompalinModel complain);
        public List<CompalinModel> GetALlComplainList(int CompanyID);
        public CompalinModel GetComplainListUsingID(int TickedID);
        public bool UpdateCompalin(CompalinModel complain);
        public CompalinModel GetComplainUsingTickedID(string TickedID);
        public void InsertComplainDetails(ComplainProblemModel model);
        public List<ComplainProblemModel> GetAllPromlemDetails(string TickedID);
        public bool DeleteProblemList(string TicketCode);
    }
}
