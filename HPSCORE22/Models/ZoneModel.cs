
namespace RetailCare.Models
{
    public class ZoneModel
    {
        public int ZONEID { get; set; }
        public int ZONECODE { get; set; }
        public string? ZONENAME { get; set; }
        public string? CONTACTNO { get; set; }
        public int THANAID { get; set; }
        public int DISTRICTID { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public string? ENTRYPC { get; set; }
        public string? MODIFIEDPC { get; set; }
        public int ISACTIVE { get; set; }
        public int COMPANYID { get; set; }
    }
}
