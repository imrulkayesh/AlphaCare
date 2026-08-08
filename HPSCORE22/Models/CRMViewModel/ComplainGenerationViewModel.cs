using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceModel;

namespace RetailCare.Models.CRMViewModel
{
    public class ComplainGenerationViewModel
    {
        public CompalinModel ComaplainModel { get; set; } = new CompalinModel();
        public CompanyModel CompanyModel { get; set; } = new CompanyModel();
        public List<CompanyModel> CompanyList { get; set; } = new List<CompanyModel>();
        public List<StatusModel> StatusList { get; set; }= new List<StatusModel>();
        public List<ProblemModel> ProblemList { get; set; } = new List<ProblemModel>();
        public List<ProductModel> ProductList { get; set; } = new List<ProductModel>();
        public List<ProductModelsModel> ProductModelList { get; set; } = new List<ProductModelsModel>();
        public List<CompalinModel> ComplainList { get; set; } = new List<CompalinModel>();
        public List<ItemModel> ItemClassList { get; set; } = new List<ItemModel>();
        public List<TechnicianModel> TechnicianList { get; set; } = new List<TechnicianModel>();
        public List<ZoneModel> ZoneList { get; set; }=new List<ZoneModel>();
        public List<ComplainProblemModel> ProblemListAdded { get; set; } = new List<ComplainProblemModel>();
    }
}
