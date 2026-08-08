using Microsoft.AspNetCore.Mvc;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;

namespace RetailCare.Controllers
{
    public class ReportGenerationController : Controller
    {
        private readonly IReportGenerationRepository _ReportGeneration;
        private readonly ICommonMethod _SessionHelper;
        public readonly IReportingMethods _ReportingMethods;
        public ReportGenerationController(IReportGenerationRepository ReportGeneration, ICommonMethod SessionHelper, IReportingMethods reportingMethods)
        {
            _ReportGeneration = ReportGeneration;
            _SessionHelper = SessionHelper;
            _ReportingMethods = reportingMethods;
        }
        // Complain Report
        public IActionResult ComplainReportGeneration()
        {
            ReportGenerationViewModel ReportData=new ReportGenerationViewModel();
            return View("~/Views/ReportGeneration/ComplainReportGeneration.cshtml", ReportData);
        }
        public IActionResult ComplainReport(ReportGenerationViewModel Report)
        {
            var UserDetails = _SessionHelper.GetUser();
            if (ModelState.IsValid)
            {
                var filtereddata= _ReportGeneration.GetComplainReport(Report.FilteringOption, UserDetails.COMPANYID).ToList();
                if(filtereddata.Count>0)
                {
                    Report.ComplainList= filtereddata;
                }
                else
                {
                    TempData["ERRORMSG"] = "Data Can not been Found";
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
