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
        public UserManagementController(IUserManagementRepository userManagementRepository, IZoneRepository zoneRepository, ICommonMethod sessionHelper)
        {
            _UserManagementRepository = userManagementRepository;
            _ZoneRepository = zoneRepository;
            _SessionHelper = sessionHelper;
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
                        var UserCompanyPermisison = new UserCampany()
                        {
                            USERID = userdetails.USERID,
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
        // User Menu Permission 
        public IActionResult CreateUserPermission()
        {
            var MenusViewModel = GetAllDataUserWiseMenu();
            return View("~/Views/UserManagement/CreateUserPermission.cshtml", MenusViewModel); 
        }
        [HttpPost]
        public IActionResult SaveUserPermission(string userID, List<int> CheckingMenuList)
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
                            USERID = userID,
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
            var userdetails = _SessionHelper.GetUser();
            var Menudetails = new UserWiseMenuPermissionViewModel()
            {
                Menus= _UserManagementRepository.GetAllMenuList(),
                ParentMenus= _UserManagementRepository.GetAllParentsMenu(),
                UserDetails= _UserManagementRepository.GetAllUserList(userdetails.COMPANYID)
            };
            return Menudetails;
        }

        // ajax code 
        //private JsonResult GetALlUserPermission(string UserID)
        //{
        //    if (UserID == null)
        //    {
        //        return Json(null);
        //    }
        //    else
        //    {

        //    }
        //}
    }
}
