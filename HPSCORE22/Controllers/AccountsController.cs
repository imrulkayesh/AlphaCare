using AlphaCare.Interface;
using AlphaCare.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QCMS.Models;
using QCMS.Repositories;
using RetailCare.Common;
using RetailCare.Models;
using System.Security.Claims;
using System.Text.Json;

namespace AlphaCare.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly ICommonMethod _SessionHelper;
        private readonly ApiService _apiService;
        private readonly IMenuSettingManagementRepository _MenuSetup;
        public AccountsController(UserRepository userRepository, ICommonMethod SessionHelper, ApiService apiService, IMenuSettingManagementRepository MenuSetup)
        {
            _userRepository = userRepository;
            _SessionHelper = SessionHelper;
            _apiService = apiService;
            _MenuSetup = MenuSetup;
        }

        // =========================
        // LOGIN VIEW
        // =========================
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
                // Get business units as select list items
                var businessUnits = await _apiService.GetBusinessUnitsAsSelectListAsync();
                ViewBag.BusinessUnits = businessUnits;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMSG = "Could not load Business Units. Please try again.";
            }

            return View();
        }

        // =========================
        // LOGIN POST
        // =========================
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Username, string Password, string BusinessUnit)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(BusinessUnit))
                {
                    return Json(new { success = false, message = "Please select a Business Unit" });
                }

                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    return Json(new { success = false, message = "Username and Password are required" });
                }

                // Get business unit details
                var businessUnitData = await _apiService.GetBusinessUnitByIdAsync(BusinessUnit);
                if (businessUnitData == null)
                {
                    return Json(new { success = false, message = "Invalid Business Unit selected" });
                }

                // Call login API
                var loginResponse = await _apiService.LoginAsync(
                    businessUnitData.CONN_ID,
                    businessUnitData.SCHEMA_ID,
                    Username,
                    Password
                );

                if (loginResponse != null && loginResponse.status && loginResponse.result != null && loginResponse.result.Count > 0)
                {
                    var userData = loginResponse.result.First();

                    var userDetails = _userRepository.GetUserInfo(userData.ID);
                    if (userDetails.Count > 0)
                    {
                        var user = userDetails.FirstOrDefault();
                       var MenuList= _MenuSetup.GetAllRoleWiseMenu(user.USERTYPEID);

                        HttpContext.Session.SetString("MenuList",JsonSerializer.Serialize(MenuList));
                        // Store session data
                        HttpContext.Session.SetString("USERID", userData.ID ?? "");
                        HttpContext.Session.SetString("USERNAME", userData.NAME ?? "");
                        HttpContext.Session.SetString("EMPLOYEE_CODE", userData.CODE ?? "");
                        HttpContext.Session.SetString("EMPLOYEE_NAME", userData.NAMES ?? "");
                        HttpContext.Session.SetString("ADDRESS", userData.ADDRESS ?? "");
                        HttpContext.Session.SetString("CONTACT", userData.CONTACT ?? "");
                        HttpContext.Session.SetString("BUSINESS_UNIT", BusinessUnit);
                        HttpContext.Session.SetString("CONN_ID", businessUnitData.CONN_ID);
                        HttpContext.Session.SetString("SCHEMA_ID", businessUnitData.SCHEMA_ID);
                        HttpContext.Session.SetInt32("CompanyID", user.COMPANYID);
                        HttpContext.Session.SetInt32("UserTypeID", user.USERTYPEID);

                        // =========================
                        // CREATE AUTHENTICATION COOKIE
                        // =========================
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userData.NAME ?? ""),
                            new Claim(ClaimTypes.NameIdentifier, userData.ID ?? ""),
                            new Claim("USERID", userData.ID ?? ""),
                            new Claim("EMPLOYEE_CODE", userData.CODE ?? ""),
                            new Claim("EMPLOYEE_NAME", userData.NAMES ?? ""),
                            new Claim("BUSINESS_UNIT", BusinessUnit),
                            new Claim("CONN_ID", businessUnitData.CONN_ID),
                            new Claim("SCHEMA_ID", businessUnitData.SCHEMA_ID)
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        // Sign in user
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                            }
                        );

                        // Log to verify cookie is created
                        Console.WriteLine($"User {userData.NAME} authenticated successfully. Cookie created.");

                        return Json(new { success = true, redirect = Url.Action("Index", "Home"), model = MenuList });
                    }
                    else
                    {
                        return Json(new { success = false, message = "User not found" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = loginResponse?.message ?? "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Login error: {ex.Message}" });
            }
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        // =========================
        // ACCESS DENIED
        // =========================
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Denied()
        {
            return View();
        }

        // Newly added Code 21.7.26
        public async Task<IActionResult> SelectCompanyPage()
        {
            var loginUserDetails = _SessionHelper.GetUser();
            ViewBag.Name = loginUserDetails.USERID;
            List<CompanyModel> companyDetails = await _userRepository.GetCompany(loginUserDetails.USERID);
            return View("~/Views/Accounts/SelectCompanyPage.cshtml", companyDetails);
        }
        public IActionResult UserDashboardDirection(int companyID)
        {
            HttpContext.Session.SetInt32("CompanyID", companyID);
            return View("~/Views/Home/Index.cshtml");
        }
    }
}