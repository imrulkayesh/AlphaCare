using AlphaCare.Interface.DashboardInterface;
using AlphaCare.Models.Dashboard_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCMS.Models;
using RetailCare.Common;
using RetailCare.Models.CRMModels;
using System.Diagnostics;

namespace QCMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonMethod _SessionHelper;
        private readonly IUserDashboardRepository _UserDashboard;
        public HomeController(ILogger<HomeController> logger, ICommonMethod SessionHelper, IUserDashboardRepository UserDashboard)
        {
            _logger = logger;
            _SessionHelper = SessionHelper;
            _UserDashboard = UserDashboard;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("USER_NAME");
            ViewBag.DistName = HttpContext.Session.GetString("DIST_NAME");
            return View();
        }

        // Top Card Design 
        [HttpGet]
        public JsonResult GetProductWiseProductModel()
        {
            var userdetails = _SessionHelper.GetUser();
            var CardDetails1= _UserDashboard.GetComplaintDashboard(userdetails.COMPANYID, userdetails.EMPLOYEE_CODE);
            var CardDetails2 = _UserDashboard.GetDueComplaintDashboard(userdetails.COMPANYID, userdetails.EMPLOYEE_CODE);
            return Json(new
            {
                Singlecard= CardDetails1,
                VsCardDetasil= CardDetails2
            });
        }
        [HttpGet]
        public JsonResult GetDetailsData(int typeID)
        {
            var userdetails = _SessionHelper.GetUser();
            List<CompalinModel> CardDetails1 = new List<CompalinModel>();
            if (typeID==1)
            {
                CardDetails1 = _UserDashboard.GetPendingComplain(userdetails.COMPANYID, userdetails.EMPLOYEE_CODE);
            }
            if (typeID == 2)
            {
                CardDetails1 = _UserDashboard.GetDueComplain(userdetails.COMPANYID, userdetails.EMPLOYEE_CODE);
            }
            return Json(new
            {
                PendingComplainList = CardDetails1
            });
        }
    }
}
