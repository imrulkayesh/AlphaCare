using AlphaCare.Interface.DashboardInterface;
using AlphaCare.Models.Dashboard_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCMS.Models;
using RetailCare.Common;
using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceViewModel;
using System.Diagnostics;

namespace QCMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonMethod _SessionHelper;
        private readonly IUserDashboardRepository _UserDashboard;

        public HomeController(
            ILogger<HomeController> logger,
            ICommonMethod SessionHelper,
            IUserDashboardRepository UserDashboard)
        {
            _logger = logger;
            _SessionHelper = SessionHelper;
            _UserDashboard = UserDashboard;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserName =
                HttpContext.Session.GetString("USER_NAME");

            ViewBag.DistName =
                HttpContext.Session.GetString("DIST_NAME");

            UserDashboardViewModel UserDashboard =
                new UserDashboardViewModel();

            UserDashboard.Top10TechnicianList =
                await GetTop10TechnicianSolve();
            UserDashboard.Lowest10TechnicianList =
                await GetLow10TechnicianSolve();
            UserDashboard.Top5Showroom =
                     await GetTop5Showroom();
            return View(
                "~/Views/Home/Index.cshtml",
                UserDashboard
            );
        }
        // Top Card Design
        [HttpGet]
        public JsonResult GetProductWiseProductModel()
        {
            var userdetails = _SessionHelper.GetUser();

            var CardDetails1 =
                _UserDashboard.GetComplaintDashboard(
                    userdetails.COMPANYID,
                    userdetails.EMPLOYEE_CODE
                );

            var CardDetails2 =
                _UserDashboard.GetDueComplaintDashboard(
                    userdetails.COMPANYID,
                    userdetails.EMPLOYEE_CODE
                );

            return Json(new
            {
                Singlecard = CardDetails1,
                VsCardDetasil = CardDetails2
            });
        }
        [HttpGet]
        public JsonResult GetDetailsData(int typeID)
        {
            var userdetails = _SessionHelper.GetUser();

            List<CompalinModel> CardDetails1 =
                new List<CompalinModel>();

            if (typeID == 1)
            {
                CardDetails1 =
                    _UserDashboard.GetPendingComplain(
                        userdetails.COMPANYID,
                        userdetails.EMPLOYEE_CODE
                    );
            }

            if (typeID == 2)
            {
                CardDetails1 =
                    _UserDashboard.GetDueComplain(
                        userdetails.COMPANYID,
                        userdetails.EMPLOYEE_CODE
                    );
            }

            return Json(new
            {
                PendingComplainList = CardDetails1
            });
        }
        // Top 5 Technician Solve & Top 5 Problem Type 
        [HttpGet]
        public async Task<JsonResult> GetTop5ProblemType()
        {
            var userdetails = _SessionHelper.GetUser();

            var problemTypeData = await _UserDashboard.GetTop5ProblemType(
                userdetails.COMPANYID,
                userdetails.EMPLOYEE_CODE
            );

            return Json(new
            {
                ProblemTypeData = problemTypeData
            });
        }
        private async Task<List<Top10TechnicianModel>> GetTop10TechnicianSolve()
        {
            var userdetails = _SessionHelper.GetUser();

            return await _UserDashboard.GetTop10TechnicianSolvedATA(
                userdetails.COMPANYID,
                userdetails.EMPLOYEE_CODE
            );
        }
        private async Task<List<Top10TechnicianModel>> GetLow10TechnicianSolve()
        {
            var userdetails = _SessionHelper.GetUser();

            return await _UserDashboard.GetLowest10TechnicianSolvedATA(
                userdetails.COMPANYID,
                userdetails.EMPLOYEE_CODE
            );
        }
        private async Task<List<Top10TechnicianModel>> GetTop5Showroom()
        {
            var userdetails = _SessionHelper.GetUser();

            return await _UserDashboard.GetTop5Showroom(
                userdetails.COMPANYID
            );
        }
        [HttpGet]
        public async Task<JsonResult> GetCurrentTimeTrand()
        {
            var userdetails = _SessionHelper.GetUser();

            var timeTrendData = await _UserDashboard.GetCurrentMonthTimeTrand(
                userdetails.COMPANYID
            );

            return Json(new
            {
                TimeTrendData = timeTrendData
            });
        }
        [HttpGet]
        public JsonResult GetCurrentDayStatus()
        {
            var userdetails = _SessionHelper.GetUser();

            var CardDetails1 = _UserDashboard.GetCurrentDayTicketStatus(
                    userdetails.COMPANYID,
                    userdetails.EMPLOYEE_CODE
                );
            return Json(new
            {
                Singlecard = CardDetails1,
            });
        }
    }
}