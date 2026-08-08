namespace RetailCare.Models
{
    public class FeedBackReportModel
    {
        public int FeedbackId { get; set; }
        public int TicketID { get; set; }
        public int? ItemId { get; set; }
        public int? ProductId { get; set; }
        public int? PrModelId { get; set; }
        public int? ProductQty { get; set; }
        public int? ActualProblemId { get; set; }
        public int? StatusId { get; set; }
        public string? SolvedBy { get; set; }
        public string? UsedSpareParts { get; set; }
        public string? SubProblem { get; set; }
        public string? Remarks { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WorkingDate { get; set; }
        public string? SerialNo { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerContactNo { get; set; }
        public string? CustomerAddress { get; set; }
        public string? ItemName { get; set; }
        public int? AssignZoneId { get; set; }
        public int? TechnicianId { get; set; }
        public int? SupervisorId { get; set; }
        public string? ZoneName { get; set; }
        public string? Expr1 { get; set; }
        public string? ProductName { get; set; }
        public int? ModelCode { get; set; }
        public string? PrModelName { get; set; }
        public string? StatusName { get; set; }
        public string? TechnicianName { get; set; }
        public int? StaffID { get; set; }
        public string? SupervisorName { get; set; }
        public string? ProblemName { get; set; }
        public string? AssistId1 { get; set; }
        public string? AssistId2 { get; set; }
        public int? Duration { get; set; }
        public int? Tech1StaffId { get; set; }
        public string? SolvedBy1 { get; set; }
        public int? Tech2StaffId { get; set; }
        public string? SolvedBy2 { get; set; }
        public int? Tech3StaffId { get; set; }
        public string? SolvedBy3 { get; set; }
        public string? ComTypeName { get; set; }
        public string? WaName { get; set; }
        public string? BarcodeNo { get; set; }
        public string? TransactionId { get; set; }
        public int? BillAmount { get; set; }

    }
}
