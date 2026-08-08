using RetailCare.Models;

namespace RetailCare.Interface
{
    public interface IBrandRepository
    {
        public List<BrandModel> GetAllBrandList(int BrandID);
        public BrandModel GetBrandUsngBrandID(int BrandID);
        public int InsertGroup(BrandModel model);
        public bool UpdateGroup(BrandModel model);
    }
}
