using RetailCare.Models.CRMModels;

namespace AlphaCare.Common
{
    public interface IComplainSmSApi
    {
        Task<bool> SendSMSAPI(
            CompalinModel ComplainDetails,
            string TechnicianName,
            string TechnicianNumer,
            string ProductType);
    }
    public class ComplainSmSApi : IComplainSmSApi
    {
        private const string UserID = "487428";
        private const string PasswordHash = "00b823eb7b959d189acbb3a2a4f80171";

        public async Task<bool> SendSMSAPI(CompalinModel ComplainDetails,string TechnicianName,string TechnicianNumer, string ProductType)
        {
            try
            {
                string GeneratedSMSBody =
                    $"TID {ComplainDetails.TICKETCODE}," +
                    $"Name:{ComplainDetails.CUSTOMERNAME}," +
                    $"Contact:{ComplainDetails.CONTACTNO}," +
                    $"Address:{ComplainDetails.LOCATION}," +
                    $"Product Type:{ProductType}," +
                    $"Technician:{TechnicianName}({TechnicianNumer})";
                // $"&msisdn={TechnicianNumer}" +
                string apiUrl =
                    $"http://sms.prangroup.com/postman/api/sendsms" +
                    $"?userid={UserID}" +
                    $"&password={PasswordHash}" +
                    $"&msisdn={TechnicianNumer}" +
                    $"&masking=28585" +
                    $"&message={Uri.EscapeDataString(GeneratedSMSBody)}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
