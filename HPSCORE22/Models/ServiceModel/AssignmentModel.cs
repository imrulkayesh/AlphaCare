namespace RetailCare.Models.ServiceModel
{
    public class AssignmentModel
    {
        public int ASSIGNID { get; set; }
        public int ASSIGNCODE { get; set; }
        public string TICKETID { get; set; }
        public DateTime ASSIGNDATE { get; set; }
        public DateTime FINISHDATE { get; set; }
        public int STATUSID { get; set; }
        public int TECHNICIANID { get; set; }
        public int ASSIGNZONEID { get; set; }
        public int ZONECODE { get; set; }
        public int SUPERVISORID { get; set; }
        public int COMPANYID { get; set; }
        public int GROUPID { get; set; }
        public int CUSTOMERID { get; set; }
        public string? CUSTOMERNAME { get; set; }
        public string CUSTOMERCONTACTNO { get; set; }
        public string? CUSTOMERADDRESS { get; set; }
        public string? PRODUCTNAME { get; set; }
        public string? PROBLEMNAME { get; set; }
        public string? OTHERRECEIVER { get; set; }
        public string? REMARKS { get; set; }
        public int ISASSIGN { get; set; }
        public int SENDFEEDBACK { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public string? ENTRYPC { get; set; }
        public string? MODIFIEDPC { get; set; }
        public int PRODUCTID { get; set; }
        public int PROBLEMID { get; set; }
        public string SHOWROOMCODE { get; set; }

        // Reporting Fields
        public string? STATUSNAME { get; set; }
        public string? PRMODELNAME { get; set; }
        public string? ZONENAME { get; set; }
        public string? TECHNICIANNAME { get; set; }
        public string? ITEMNAME { get; set; }
        public DateTime? COMPLAINDATE { get; set; }
        public int ITEMID { get; set; }

        // New Table Column
        //public int BillAmount { get; set; }
        //public string? BillNote { get; set; }
        //public DateTime BillDate { get; set; }
        //public string? BillFlag { get; set; }
        //public string? UserId { get; set; }
        //public int NetAmount { get; set; }
        //public string? Discount { get; set; }
        //public decimal Amount { get; set; }
        //public string fsp { get; set; }
        //public string? staffid { get; set; }
        //public string? CancleReason { get; set; }
        //public string? CancelFirstApv { get; set; }
        //public DateTime CancelApvdate { get; set; }
        //public string? CancelFinalApv { get; set; }
        //public DateTime CancelFinalApvDatetime { get; set; }
        //public string? CancelStatus { get; set; }
        //public DateTime CancleApplyDate { get; set; }
        //public string? CancleRejectReason { get; set; }
        //public int CompanyIdNew { get; set; }
        //public int ServiceOperationId { get; set; }
    }
}
