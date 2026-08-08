using RetailCare.Models.ServiceModel;

namespace RetailCare.Models.ServiceViewModel
{
    public class TechnicianViewModel
    {
        public TechnicianModel TechnicianDetails { get; set; } = new TechnicianModel();
        public List<ItemModel> ClassList { get; set; }=new List<ItemModel>();
        public List<int> CheckingClassNameList { get; set; } = new List<int>();
        public List<CompanyModel> CompanyList { get; set; } = new List<CompanyModel>();
        public List<ZoneModel> ZoneList { get; set; }= new List<ZoneModel>();
       public List<TechnicianModel> TechnicianList { get;set; } = new List<TechnicianModel>();
       public List<ProductModel> ProductList { get; set; } = new List<ProductModel>();

        // Existing assigned class IDs
        public List<int> SelectedClassIds { get; set; } = new List<int>();
    }
}
