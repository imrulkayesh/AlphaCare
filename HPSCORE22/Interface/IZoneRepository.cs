using RetailCare.Models;

namespace RetailCare.Interface
{
    public interface IZoneRepository
    {
        public List<ZoneModel> GetAllZoneDetails();
    }
}
