using RetailCare.Models.ServiceModel;

namespace RetailCare.Interface.ServiceInterface
{
    public interface ITechnicianRepository
    {
        public int AddTechnician(TechnicianModel technician);
        public void AddAssignClass(TECHNICIANSASSIGNPRODUCT Technicians);
        public List<TechnicianModel> GetAllTechnicianData(int CompanyID, int ClassID);
        public bool DeletePreviousPermission(int TechnicioanID);
        public bool UpdateTechnician(TechnicianModel technician);
        public List<TechnicianModel> GetAllTechniciansList(int CompanyID);
        public TechnicianModel GetSingleTechnicians(int CompanyID, int TechniciansID);
        public List<TECHNICIANSASSIGNPRODUCT> GetTechniciansAssignClassList(int TechniciansID);
        public TechnicianModel GetSingleTechnicianUsingStaffID(int StaffID);
    }
}
