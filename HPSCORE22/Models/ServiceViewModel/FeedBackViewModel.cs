using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceModel;

namespace RetailCare.Models.ServiceViewModel
{
    public class FeedBackViewModel
    {
        public FeedabackModel FeedBackDetails { get; set; } = new FeedabackModel();
        public List<AssignmentModel> AssignmentListForFeedback { get; set; }=new List<AssignmentModel>();
        public List<BrandModel> BrandList { get; set; } = new List<BrandModel>();
        public List<StatusModel> StatusList { get; set; } = new List<StatusModel>();
        public List<ProductModel> ProductList { get; set; } = new List<ProductModel>();
        public List<ItemModel> ItemList { get; set; } = new List<ItemModel>();
        public List<string> SelectedSubProblemList { get; set; } = new List<string>();
        public List<TechnicianModel> SolvedBy { get; set; } = new List<TechnicianModel>();
        public List<TechnicianModel> AssistBy1 { get; set; } = new List<TechnicianModel>();
        public List<TechnicianModel> AssistBy2 { get; set; } = new List<TechnicianModel>();
        public List<ProductModelsModel> ProductModelsList { get; set; }= new List<ProductModelsModel>();
        public List<ProblemModel> ProblemList { get; set; } = new List<ProblemModel>();
        public List<CompalinModel> CompalinList { get; set;} = new List<CompalinModel>();
        public CompalinModel CompalinModel { get; set; }=new CompalinModel();
        public List<Subproblem> SubproblemList { get; set; }=new List<Subproblem>();
        public IFormFile formFile { get; set; }
    }
}
