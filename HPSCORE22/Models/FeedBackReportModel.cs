namespace RetailCare.Models
{
    public class FeedBackReportModel
    {
        public int FEEDBACKID { get; set; }
        public string TICKETCODE { get; set; }
        public DateTime? WORKINGDATE { get; set; }
        public string REMARKS { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string CONTACTNO { get; set; }
        public string LOCATION { get; set; }
        public DateTime? COMPLAINDATE { get; set; }
        public string SHOWROOM { get; set; }
        public string STATUSNAME { get; set; }
        public string TECHNICIANNAME { get; set; }
        public string STAFFID { get; set; }
        public string PRODUCTNAME { get; set; }
        public int? QUANTITY { get; set; }
        public string COMPLAINREMARKS { get; set; }
        public string PROBLEMNAME { get; set; }

    }
}
