using RetailCare.Models.ServiceModel;

namespace RetailCare.Interface.ServiceInterface
{
    public interface IAssignmentManagementRepository
    {
        public int InsertAssignTechnician(AssignmentModel model);
        public List<AssignmentModel> GetAllAssignmentListForFeedback(int CompanyID);
        public AssignmentModel GetSingleTaskAssignList(int CompanyID);
        public AssignmentModel GetSingleTaskUsingTickedID(string TicketID);
        public bool UpdateAssign(AssignmentModel AssignModel);
        public List<AssignmentModel> GetAllAssignmentListForFeedbackTechnicianIDWise(int TechnicianID);

    }
}
