namespace RetailCare.Models
{
    public class ProductModel
    {
        public int PRODUCTID { get; set; }
        public string? PRODUCTCODE { get; set; }
        public string? PRODUCTNAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? ITEMID { get; set; }
        public string? SERIALNO { get; set; }
        public string? PARTSNO { get; set; }
        public int? PRODUCTQTY { get; set; }
        public int? COST { get; set; }
        public int? UNITPRICE { get; set; }
        public string? INSTALLATIONTYPE { get; set; }
        public int? COMPANYID { get; set; }
        public int? ACTIVE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public string ENTRYDATE { get; set; }
        public string MODIFIEDDATE { get; set; }
    }
    public class ProductModelsModel
    {
        public int PRMODELID { get; set; }
        public string? MODELCODE { get; set; }
        public string? PRMODELNAME { get; set; }
        public int PRODUCTID { get; set; }
        public int? COMPANYID { get; set; }
        public int? ACTIVE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public string ENTRYDATE { get; set; }
        public string MODIFIEDDATE { get; set; }
    }
}
