namespace RetailCare.Models
{
    public class StatusModel
    {
        public int STATUSID { get; set; }
        public string? STATUSCODE { get; set; }
        public string? STATUSNAME { get; set; }
        public int? COMPANYID { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public int ISACTIVE { get; set; }
        public int ISDEFAULT { get; set; }
    }
}
