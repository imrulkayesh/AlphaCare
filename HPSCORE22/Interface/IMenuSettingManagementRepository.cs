using AlphaCare.Models;
using RetailCare.Models;

namespace AlphaCare.Interface
{
    public interface IMenuSettingManagementRepository
    {
        public List<RoleWiseMenuPermission> GetAllRoleWiseMenu(int RoleID);
        public bool AddNewRole(RoleModel RoleManagement);
        public bool UpdateRole(RoleModel RoleManagement);
        public List<RoleModel> GetAllRoles();
    }
}
