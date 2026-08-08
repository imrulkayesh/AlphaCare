namespace RetailCare.Models
{
    public class UserManagementViewModel
    {
      public UserModel CreateNewUser { get; set; }=new UserModel();
      public List<UserTypeModel>UserTypeList { get; set; } = new List<UserTypeModel>();
      public List<DesignationModel> DesignationList { get; set; } = new List<DesignationModel>();
      public List<DepartmentModel> DeparmentList { get; set; } = new List<DepartmentModel>();
      public List<ZoneModel> ZoneList { get; set; } = new List<ZoneModel>();
     public List<UserModel>  UserList { get; set; } = new List<UserModel>();
    }
    public class UserWiseMenuPermissionViewModel
    {
        public List<Menus> Menus { get; set; }=new List<Menus>();
        public List<ParentMenus>ParentMenus { get; set; }= new List<ParentMenus>();
        public UserWiseMenuPer UserWiseMenus { get; set; } = new UserWiseMenuPer();
        public List<UserModel> UserDetails { get; set; } = new List<UserModel>();
        public List<int> CheckingMenuList { get; set; } = new List<int>();
    }
}
