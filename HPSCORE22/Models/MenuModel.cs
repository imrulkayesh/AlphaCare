namespace AlphaCare.Models
{
    public class MenuModel
    {
        public int MENUID { get; set; }
        public string? PARENTMENUID { get; set; }
        public string? TITLE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string URL { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public string ENTRYDATE { get; set; }
        public string MODIFIEDDATE { get; set; }
        public string MENUFLAG { get; set; }
        public string MENUORDER { get; set; }
        public string PAGENO { get; set; }
        public string PAGEICON { get; set; }
        public int ACTIVE { get; set; }
    }
    public class PARENTMENUModel
    {
        public int PARENTMENUID { get; set; }
        public string? PARENTMENUNAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public string ENTRYBY { get; set; }
        public string MODIFIEDBY { get; set; }
        public string ENTRYDATE { get; set; }
        public string MODIFIEDDATE { get; set; }
        public string MENUFLAG { get; set; }
        public string MENUORDER { get; set; }
        public string PAGENO { get; set; }
        public string PAGEICON { get; set; }
        public int ACTIVE { get; set; }
    }
    public class RoleWiseMenuPermission
    {
        public int PARENTMENUID { get; set; }
        public string? PARENTMENUNAME { get; set; }
        public int MENUID { get; set; }
        public string? TITLE { get; set; }
        public string URL { get; set; }
        public string PAGEICON { get; set; }
        public int MENU_PARENT_ID { get; set; }
    }
}
