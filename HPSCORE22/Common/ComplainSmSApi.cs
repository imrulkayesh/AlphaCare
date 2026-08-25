using RetailCare.Models.CRMModels;

namespace AlphaCare.Common
{
    public interface IComplainSmSApi
    {
        bool SendSMSAPI(CompalinModel ComplainDetails, string TechnicianName, string TechnicianNumer, string ProductType);
    }
    public class ComplainSmSApi: IComplainSmSApi
    {
        public bool SendSMSAPI(CompalinModel ComplainDetails,string TechnicianName,string TechnicianNumer,string ProductType)
        {
            bool isSend = true;
            try
            {
                string apiUrl = "";
                string GeneratedSMSBody =
                $"TID {ComplainDetails.TICKETCODE},Name:{ComplainDetails.CUSTOMERNAME},Contact:{ComplainDetails.CONTACTNO},Address:{ComplainDetails.LOCATION},Product Type:{ProductType},Technician: {TechnicianName}({TechnicianNumer})";
            }
            catch (Exception ex)
            {
                isSend= false;
            }
            return isSend;
        }
    }
}
