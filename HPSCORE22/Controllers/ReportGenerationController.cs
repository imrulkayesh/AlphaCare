using Microsoft.AspNetCore.Mvc;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Interface.CRMInterface;
using RetailCare.Models;

namespace RetailCare.Controllers
{
    public class ReportGenerationController : Controller
    {
        private readonly IReportGenerationRepository _ReportGeneration;
        private readonly ICommonMethod _SessionHelper;
        public readonly IReportingMethods _ReportingMethods;
        private readonly IStatusRepository _statusrepository;
        public ReportGenerationController(IReportGenerationRepository ReportGeneration, ICommonMethod SessionHelper, IReportingMethods reportingMethods,
          IStatusRepository statusrepository)
        {
            _ReportGeneration = ReportGeneration;
            _SessionHelper = SessionHelper;
            _ReportingMethods = reportingMethods;
            _statusrepository = statusrepository;
        }
        // Complain Report
        public IActionResult ComplainReportGeneration()
        {
            var UserDetails = _SessionHelper.GetUser();
            ReportGenerationViewModel ReportData=new ReportGenerationViewModel();
            ReportData.StatusList= _statusrepository.GetAllStatus(UserDetails.COMPANYID).ToList();
            return View("~/Views/ReportGeneration/ComplainReportGeneration.cshtml", ReportData);
        }
        public IActionResult ComplainReport(ReportGenerationViewModel Report)
        {
            var UserDetails = _SessionHelper.GetUser();
            if (ModelState.IsValid)
            {
                var AllComplainList = _ReportGeneration.GetComplainReport(Report.FilteringOption, UserDetails.COMPANYID).ToList();
                if (UserDetails.USERTYPEID==1)
                {
                    if(AllComplainList.Count > 0)
                    {
                        Report.ComplainList = AllComplainList;
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Data Can not been Found";
                    }
                }
                else
                {
                    if(AllComplainList.Count > 0)
                    {
                        Report.ComplainList = AllComplainList.Where(x=>x.SHOWROOM== UserDetails.EMPLOYEE_CODE).ToList();
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Data Can not been Found";
                    }
                }
            }
            else
            {
                return View("~/Views/ReportGeneration/ComplainReportGeneration.cshtml", Report);
            }
           return View("~/Views/ReportGeneration/ComplainReportGeneration.cshtml", Report);
        }

        // Feedback Report
        public IActionResult FeedbackReportGeneration()
        {
            ReportGenerationViewModel ReportData = new ReportGenerationViewModel();
            return View("~/Views/ReportGeneration/FeedbackReportGeneration.cshtml", ReportData);
        }
        public IActionResult FeeBackReport(ReportGenerationViewModel Report)
        {
            var UserDetails = _SessionHelper.GetUser();
            if (ModelState.IsValid)
            {
                var filtereddata = _ReportGeneration.GetFeedBackReport(Report.FilteringOption, UserDetails.COMPANYID).ToList();
                if (filtereddata.Count > 0)
                {
                    Report.FeedbackReport = filtereddata;
                }
                else
                {
                    TempData["ERRORMSG"] = "Data Can not been Found";
                }
            }
            else
            {
                return View("~/Views/ReportGeneration/FeedbackReportGeneration.cshtml", Report);
            }
            return View("~/Views/ReportGeneration/FeedbackReportGeneration.cshtml", Report);
        }
    }
}
