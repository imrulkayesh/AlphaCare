using RetailCare.Models;
using RetailCare.Models.ServiceModel;

namespace RetailCare.Interface.ServiceInterface
{
    public interface IItemRepository
    {
        public List<ItemModel> GetAllItemList(int CompanyID);
        public List<ItemModel> GetAllItemListBrandWise(int BrandID);
        public ItemModel GetAllClassUsingID(int BrandID);
        public int InsertClass(ItemModel model);
        public bool UpdateClass(ItemModel model);
    }
}
