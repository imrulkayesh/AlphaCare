namespace RetailCare.Models
{
    public class ITEMCLASS
    {
            public int ID { get; set; }
            public string? ITEMCLASSID { get; set; }
            public string? ITEMCLASSNAME { get; set; }
            public int? COMPANYID { get; set; }
            public string ENTRYBY { get; set; }
            public string MODIFIEDBY { get; set; }
            public string ENTRYDATE { get; set; }
            public string MODIFIEDDATE { get; set; }
            public int ISACTIVE { get; set; }
    }
}
