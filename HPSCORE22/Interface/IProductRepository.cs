using RetailCare.Models;

namespace RetailCare.Interface
{
    public interface IProductRepository
    {
       public List<ProductModel> GetAllProcuctList(int CompanyID);
       public List<ProductModelsModel> GetAllProcuctModelList(int CompanyID);
       public List<ProductModel> GetAllProductListUsingItemID(int ItemID);
       public List<ProductModelsModel> GetALlProductModelUsingProductID(int ProductID);
    }
}
