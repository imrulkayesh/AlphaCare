namespace RetailCare.Models
{
    public class ProblemModel
    {
        public int PROBLEMID { get; set; }
        public string? PROBLEMCODE { get; set; }
        public string? PROBLEMNAME { get; set; }
        public int? PRODUCTID { get; set; }
        public int? ACTIVE { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public int? COMPANYID { get; set; }
    }
    public class Subproblem
    {
        public int SUBPROBID { get; set; }
        public int PROBLEMID { get; set; }
        public string? SUBPROBLEMNAME { get; set; }
        public int? ACTIVE { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public int? COMPANYID { get; set; }
    }
}
