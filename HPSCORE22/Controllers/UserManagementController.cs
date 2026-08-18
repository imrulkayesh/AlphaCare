using AlphaCare.Interface;
using Microsoft.AspNetCore.Mvc;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;
using RetailCare.Models.ServiceModel;

namespace RetailCare.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserManagementRepository _UserManagementRepository;
        private readonly IZoneRepository _ZoneRepository;
        private readonly ICommonMethod _SessionHelper;
        private readonly IMenuSettingManagementRepository _MenuSetup;
        public UserManagementController(IUserManagementRepository userManagementRepository, IZoneRepository zoneRepository, ICommonMethod sessionHelper,
        IMenuSettingManagementRepository MenuSetup)
        {
            _UserManagementRepository = userManagementRepository;
            _ZoneRepository = zoneRepository;
            _SessionHelper = sessionHelper;
            _MenuSetup = MenuSetup;
        }
        public IActionResult CreateUser()
        {
            UserManagementViewModel userManagementViewModel = new UserManagementViewModel();
            userManagementViewModel = GetAllDataUserCreation();
            return View("~/Views/UserManagement/CreateUser.cshtml", userManagementViewModel);
        }
        [HttpPost]
        public IActionResult CreateNewUser([Bind(Prefix = "CreateNewUser")] UserModel UserDetailsSubmit)
        {
            var userdetails = _SessionHelper.GetUser();
            UserManagementViewModel userManagementViewModel = new UserManagementViewModel();
            if(UserDetailsSubmit.USERCODE>0)
            {
                if (UserDetailsSubmit.CheckedBox == true)
                {
                    UserDetailsSubmit.ISACTIVE = 1;
                }
                else
                {
                    UserDetailsSubmit.ISACTIVE = 0;
                }
                UserDetailsSubmit.MODIFIEDBY = userdetails.USERID;
                UserDetailsSubmit.MODIFIEDDATE = DateTime.Now;
                // Default Details start
                UserDetailsSubmit.DEPARTMENTID = 0;
                UserDetailsSubmit.DESIGNATIONID = 0;
                UserDetailsSubmit.ZONEID = 0;
                // Default Details End
                var InsertionUserTable = _UserManagementRepository.UpdateUser(UserDetailsSubmit);
                if(InsertionUserTable)
                {
                    TempData["SuccessMSG"] = "Data have been Updated";
                    return RedirectToAction("CreateUser", "UserManagement");
                }
                else
                {
                    TempData["ERRORMSG"] = "!!! Error !!!!!";
                    userManagementViewModel = GetAllDataUserCreation();
                    userManagementViewModel.CreateNewUser = UserDetailsSubmit;
                    return View("~/Views/UserManagement/CreateUser.cshtml", userManagementViewModel);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var CheckUniqueName = _UserManagementRepository.CheckUserUnique(userdetails.COMPANYID, UserDetailsSubmit.USERID);
                    if (CheckUniqueName != null)
                    {
                        TempData["ERRORMSG"] = "This User Name Already Exist";
                        userManagementViewModel = GetAllDataUserCreation();
                        userManagementViewModel.CreateNewUser = UserDetailsSubmit;
                        return View("~/Views/UserManagement/CreateUser.cshtml", userManagementViewModel);
                    }
                    else
                    {
                        if (UserDetailsSubmit.CheckedBox == true)
                        {
                            UserDetailsSubmit.ISACTIVE = 1;
                        }
                        else
                        {
                            UserDetailsSubmit.ISACTIVE = 0;
                        }
                        UserDetailsSubmit.ENTRYBY = userdetails.USERID;
                        UserDetailsSubmit.ENTRYDATE = DateTime.Now;
                        UserDetailsSubmit.COMPANYID = userdetails.COMPANYID;
                        // Default Details start
                        UserDetailsSubmit.DEPARTMENTID = 0;
                        UserDetailsSubmit.DESIGNATIONID=0;
                        UserDetailsSubmit.ZONEID=0;
                        // Default Details End
                        var UserCompanyPermisison = new UserCampany()
                        {
                            USERID = UserDetailsSubmit.USERID,
                            COMPANYID = userdetails.COMPANYID,
                            ISACTIVE = (int)UserDetailsSubmit.ISACTIVE,
                            ENTRYBY = userdetails.USERID
                        };
                        var InsertionUserTable = _UserManagementRepository.InsertUser(UserDetailsSubmit);
                        var InsertionMapping= _UserManagementRepository.InsertUserCompany(UserCompanyPermisison);
                        if (InsertionUserTable > 0)
                        {
                            TempData["SuccessMSG"] = "Data have been added";
                            return RedirectToAction("CreateUser", "UserManagement");
                        }
                        else
                        {
                            TempData["ERRORMSG"] = "!!!! Error !!!!";
                            userManagementViewModel = GetAllDataUserCreation();
                            userManagementViewModel.CreateNewUser = UserDetailsSubmit;
                            return View("~/Views/UserManagement/CreateUser.cshtml", userManagementViewModel);
                        }
                    }
                }
                userManagementViewModel = GetAllDataUserCreation();
                userManagementViewModel.CreateNewUser = UserDetailsSubmit;
                return View("~/Views/UserManagement/CreateUser.cshtml", userManagementViewModel);
            }
            return RedirectToAction("CreateUser", "UserManagement");
        }
        public IActionResult EditUserData(int id)
        {
            UserManagementViewModel userManagementViewModel = new UserManagementViewModel();
            userManagementViewModel = GetAllDataUserCreation();
            userManagementViewModel.CreateNewUser = _UserManagementRepository.GetUserDetailsUsingID(id);
            return View("~/Views/UserManagement/CreateUser.cshtml", userManagementViewModel);
        }
        private UserManagementViewModel GetAllDataUserCreation()
        {
            var userdetails = _SessionHelper.GetUser();
            var userManagementViewModel = new UserManagementViewModel()
            {
                ZoneList = _ZoneRepository.GetAllZoneDetails(),
                DeparmentList = _UserManagementRepository.GetAllDepartment(),
                UserTypeList = _UserManagementRepository.GetAllUserType(),
                DesignationList = _UserManagementRepository.GetAllDesignation(),
                UserList = _UserManagementRepository.GetAllUserList(userdetails.COMPANYID),
            };
            return userManagementViewModel;
        }
        
        // Role Wise Menu Permission 
        public IActionResult CreateUserPermission()
        {
            var MenusViewModel = GetAllDataUserWiseMenu();
            return View("~/Views/UserManagement/CreateUserPermission.cshtml", MenusViewModel); 
        }
        [HttpPost]
        public IActionResult SaveUserPermission(int userID, List<int> CheckingMenuList)
        {
            var userdetails = _SessionHelper.GetUser();
            if (CheckingMenuList.Count > 0)
            {
                var DeletePreviousPermission = _UserManagementRepository.DeletePreviousPermission(userID);
                if (DeletePreviousPermission)
                {
                    foreach (var menu in CheckingMenuList)
                    {
                        var UserWiseMenu = new UserWiseMenuPer()
                        {
                            ROLEID = userID,
                            MENUID=menu,
                            ACTIVE=1,
                            ENTRYDATE = DateTime.Now,
                            ENTRYBY = userdetails.USERID
                        };
                        _UserManagementRepository.ADDPermissionWiseMenuPermission(UserWiseMenu);
                    }
                    TempData["SuccessMSG"] = "Data have been added";
                    return RedirectToAction("CreateUserPermission", "UserManagement");
                }
                else
                {
                    TempData["ERRORMSG"] = "!!! Error !!!!!";
                    var MenusViewModel = GetAllDataUserWiseMenu();
                    MenusViewModel.CheckingMenuList = CheckingMenuList;
                    return View("~/Views/UserManagement/CreateUserPermission.cshtml", MenusViewModel);
                }
            }
            else
            {
                TempData["ERRORMSG"] = "!!! Error !!!!!";
                var MenusViewModel = GetAllDataUserWiseMenu();
                MenusViewModel.CheckingMenuList= CheckingMenuList;
                return View("~/Views/UserManagement/CreateUserPermission.cshtml", MenusViewModel);
            }
            return RedirectToAction("CreateUserPermission", "UserManagement");
        }
        private UserWiseMenuPermissionViewModel GetAllDataUserWiseMenu()
        {
            var Menudetails = new UserWiseMenuPermissionViewModel()
            {
                Menus= _UserManagementRepository.GetAllMenuList(),
                ParentMenus= _UserManagementRepository.GetAllParentsMenu(),
                RoleList = _MenuSetup.GetAllRoles()
            };
            return Menudetails;
        }
        public JsonResult GetALlUserPermission(int UserId)
        {
            var data = _UserManagementRepository.GetAllRoleWiseMenus(UserId);
            if (data == null || !data.Any())
            {
                ViewData["Message"] = "No data found. Please select class.";
                return Json(new
                {
                    success = false,
                    message = "No data found. Please select class."
                });
            }
            return Json(new
            {
                success = true,
                data = data
            });
        }
        // User Wise Menu Permission & Role Wise Menu Permission
        public IActionResult CreateRole()
        {
            RolewiseMenuPermissionViewModel rolesetup = new RolewiseMenuPermissionViewModel();
            rolesetup.RoleList = _MenuSetup.GetAllRoles();
            return View("~/Views/UserManagement/CreateRole.cshtml", rolesetup);
        }
        public IActionResult AddNewRole([Bind(Prefix = "RoleDetails")] RoleModel RoleData)
        {
            var userdetails = _SessionHelper.GetUser();
            if(RoleData.TYPEID>0)
            {
                var UpdateRole = _MenuSetup.UpdateRole(RoleData);
                if (UpdateRole)
                {
                    TempData["SuccessMSG"] = "Data have been Updated";
                    return RedirectToAction("CreateRole", "UserManagement");
                }
                else
                {
                    TempData["ERRORMSG"] = "!!! Error !!!!!";
                    return View("~/Views/UserManagement/CreateRole.cshtml", UpdateRole);
                }
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var ChekcUniqueRole = _MenuSetup.GetAllRoles().Where(x=>x.TYPENAME== RoleData.TYPENAME).FirstOrDefault();
                    if(ChekcUniqueRole != null)
                    {
                        var InsertRole= _MenuSetup.AddNewRole(RoleData);
                        if(InsertRole)
                        {
                            TempData["SuccessMSG"] = "Data have been added";
                            return RedirectToAction("CreateRole", "UserManagement");
                        }
                        else
                        {
                            TempData["ERRORMSG"] = "!!! Error !!!!!";
                            return View("~/Views/UserManagement/CreateRole.cshtml", RoleData);
                        }
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "This Role Name Already Exist";
                        return View("~/Views/UserManagement/CreateRole.cshtml", RoleData);
                    }
                }
            }
            return View("~/Views/UserManagement/CreateRole.cshtml", RoleData);
        }
        public IActionResult EditRoleType(int id)
        {
            RolewiseMenuPermissionViewModel rolesetup = new RolewiseMenuPermissionViewModel();
            rolesetup.RoleDetails = _MenuSetup.GetAllRoles().Where(x=>x.TYPEID== id).FirstOrDefault();
            rolesetup.RoleList = _MenuSetup.GetAllRoles();
            return View("~/Views/UserManagement/CreateRole.cshtml", rolesetup);
        }
    }
}
