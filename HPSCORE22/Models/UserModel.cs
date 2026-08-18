using System.ComponentModel.DataAnnotations;
namespace RetailCare.Models
{
    public class UserModel
    {
        [Key]
        public int? USERCODE { get; set; }
        public string? BU_CODE { get; set; }
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "Please Enter User ID")]
        public string USERID { get; set; }
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please Enter User Name")]
        public string USERNAME { get; set; }
        [Display(Name = "Staff ID")]
        //[Required(ErrorMessage = "Please Enter Staff ID")]
        public int STAFFID { get; set; }
        [Display(Name = "User Role")]
        [Required(ErrorMessage = "Please Select Role")]
        public int USERTYPEID { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        public string PASSWORD { get; set; }
        [Display(Name = "Email Address")]
        public string? EMAIL { get; set; }
        [Display(Name = "Contact NO")]
        public string? CONTACTNO { get; set; }
        [Display(Name = "Address")]
        public string? ADDRESS { get; set; }
        public int COMPANYID { get; set; }
        public int? DEPARTMENTID { get; set; }
        public int? DESIGNATIONID { get; set; }
        public int? ZONEID { get; set; }
        public int? DEPOTID { get; set; }
        public int? DEPOACT { get; set; }
        public int? ISACTIVE { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }

        public bool? CheckedBox { get; set; }
      
    }
    public class UserTypeModel
    {
        public int TYPEID { get; set; }
        public int TYPECODE { get; set; }
        public string TYPENAME { get; set; }
        public string SERVICETYPE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
        public bool ISTRACKER { get; set; }
        public bool ISSERVICE { get; set; }

        public bool? CheckedBox { get; set; }
    }
    public class DesignationModel
    {
        public int DESIGNATIONID { get; set; }
        public int DESIGNATIONCODE { get; set; }
        public string DESIGNATIONNAME { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
    }
    public class DepartmentModel
    {
        public int DEPTID { get; set; }
        public int DEPTCODE { get; set; }
        public string DEPTNAME { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
    }
    public class UserCampany
    {
        public string USERID { get; set; }
        public int COMPANYID { get; set; }
        public int ISACTIVE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
    }

    // UserMenu Permission
    public class ParentMenus
    {
        public int PARENTMENUID { get; set; }
        public string PARENTMENUNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string Url { get; set; }
        public bool ACTIVE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
    }
    public class Menus
    {
        public int MENUID { get; set; }
        public int PARENTMENUID { get; set; }
        public string TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public string URL { get; set; }
        public bool ACTIVE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
    }
    public class UserWiseMenuPer
    {
        public int UWMNUPID { get; set; }
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "Please Select User")]
        public string USERID { get; set; }
        public int MENUID { get; set; }
        public int ACTIVE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
        public int ROLEID { get; set; }
        public bool CheckedBox { get; set; }
    }
    public class RoleModel
    {
        public int TYPEID { get; set; }
        public string TYPECODE { get; set; }
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Please Enter Role Name")]
        public string TYPENAME { get; set; }
        public string SERVICETYPE { get; set; }
        public string? ENTRYBY { get; set; }
        public string? MODIFIEDBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
    }
    public class UserWiseRolePer
    {
        public int UWMNUPID { get; set; }
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "Please Select User")]
        public string MENUID { get; set; }
        public int ROLEID { get; set; }
        public int ACTIVE { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public DateTime MODIFIEDDATE { get; set; }
        public string ENTRYPC { get; set; }
        public string MODIFIEDPC { get; set; }
        public bool CheckedBox { get; set; }
    }
}
