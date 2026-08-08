namespace RetailCare.Models
{
    public class CompanyModel
    {
        public int COMPANYID { get; set; }
        public string? COMPANYCODE { get; set; }
        public string? COMPANYNAME { get; set; }
        public string? TOKENSYSNTAX { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public string ENTRYDATE { get; set; }
        public string MODIFIEDDATE { get; set; }
        public int ISACTIVE { get; set; }
    }
}
