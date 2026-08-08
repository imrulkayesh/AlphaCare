namespace RetailCare.Models
{
    public class UserInfoModel
    {
        public int? USERCODE { get; set; }
        public string? BU_CODE { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public int STAFFID { get; set; }
        public int USERTYPEID { get; set; }
        public string PASSWORD { get; set; }
        public string? EMAIL { get; set; }
        public string? CONTACTNO { get; set; }
        public string? ADDRESS { get; set; }
        public int COMPANYID { get; set; }
        public int? DEPARTMENTID { get; set; }
        public int? DESIGNATIONID { get; set; }
        public int? ZONEID { get; set; }
        public int? DEPOTID { get; set; }
        public int? DEPOACT { get; set; }
        public int ISACTIVE { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public string BUSINESS_UNIT { get; set; }
        public string OUTLETNAME { get; set; }


        // Complain Generating Entity
        public string EMPLOYEE_CODE { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string CONTACT { get; set; }


    }
}
