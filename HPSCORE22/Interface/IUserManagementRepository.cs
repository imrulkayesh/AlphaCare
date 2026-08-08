using RetailCare.Models;

namespace RetailCare.Interface
{
    public interface IUserManagementRepository
    {
        public List<UserTypeModel> GetAllUserType();
        public List<DepartmentModel> GetAllDepartment();
        public List<DesignationModel> GetAllDesignation();
        public UserInfoModel CheckUserUnique(int CompanyID, string UserID);
        public int InsertUser(UserModel model);
        public List<UserModel> GetAllUserList(int CompanyID);
        public bool InsertUserCompany(UserCampany model);
        public UserModel GetUserDetailsUsingID(int UserCode);
        public bool UpdateUser(UserModel model);

        // User Permission
        public List<Menus> GetAllMenuList();
        public List<ParentMenus> GetAllParentsMenu();
        public bool DeletePreviousPermission(string UserID);
        public bool ADDPermissionWiseMenuPermission(UserWiseMenuPer UserWiseMenu);
    }
}
