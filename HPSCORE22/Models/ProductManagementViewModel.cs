using RetailCare.Models.ServiceModel;

namespace RetailCare.Models
{
    public class ProductManagementViewModel
    {
        public BrandModel Brand { get; set; }=new BrandModel();
        public List<BrandModel> BrandsList { get; set; } = new List<BrandModel>();
        public ItemModel ItemDetails { get; set; }=new ItemModel();
        public List<ItemModel> Items { get; set; }= new List<ItemModel>();
    }
}
