using Microsoft.AspNetCore.Mvc;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Interface.CRMInterface;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models;
using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceViewModel;
using RetailCare.Repositories.CRMRepository;

namespace RetailCare.Controllers
{
    public class ReportGenerationController : Controller
    {
        private readonly IReportGenerationRepository _ReportGeneration;
        private readonly ICommonMethod _SessionHelper;
        public readonly IReportingMethods _ReportingMethods;
        private readonly IStatusRepository _statusrepository;
        private readonly IComplainRepository _complainRepository;
        public readonly ITechnicianRepository _technicianRepository;
        public ReportGenerationController(IReportGenerationRepository ReportGeneration, ICommonMethod SessionHelper, IReportingMethods reportingMethods,
          IStatusRepository statusrepository, IComplainRepository complainRepository, ITechnicianRepository technicianRepository)
        {
            _ReportGeneration = ReportGeneration;
            _SessionHelper = SessionHelper;
            _ReportingMethods = reportingMethods;
            _statusrepository = statusrepository;
            _complainRepository = complainRepository;
            _technicianRepository = technicianRepository;
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
                if (UserDetails.USERTYPEID==4)
                {
                    if (AllComplainList.Count > 0)
                    {
                        Report.ComplainList = AllComplainList.Where(x => x.SHOWROOM == UserDetails.EMPLOYEE_CODE).ToList();
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Data Can not been Found";
                    }
                }
                else
                {
                    if (AllComplainList.Count > 0)
                    {
                        Report.ComplainList = AllComplainList;
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Data Can not been Found";
                    }
                }
                Report.StatusList = _statusrepository.GetAllStatus(UserDetails.COMPANYID).ToList();
                return View("~/Views/ReportGeneration/ComplainReportGeneration.cshtml", Report);
            }
            else
            {
                Report.StatusList = _statusrepository.GetAllStatus(UserDetails.COMPANYID).ToList();
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
        [HttpGet]
        public JsonResult GetAllComplainProblemList(string ticketcode)
        {
            var data = _complainRepository.GetAllPromlemDetails(ticketcode).ToList();
            var FeedackImage= _ReportGeneration.GetAllFeedbackImage(ticketcode).ToList();
            return Json(new
            {
                ProblemList = data,
                ImageDetails= FeedackImage
            });
        }

        // Technician Report
        public IActionResult TechnicianReportGeneration()
        {
            ReportGenerationViewModel ReportData = new ReportGenerationViewModel();
            var userdetails = _SessionHelper.GetUser();
            ReportData.TechnicianList= _technicianRepository.GetAllTechniciansList(userdetails.COMPANYID);
            return View("~/Views/ReportGeneration/TechnicianReportGeneration.cshtml", ReportData);
        }
        public IActionResult TechnicianReportGenerationReport(ReportGenerationViewModel Report)
        {
            var UserDetails = _SessionHelper.GetUser();
            if (ModelState.IsValid)
            {
                var filtereddata = _ReportGeneration.GetAllTechTotalSolveData(Report.FilteringOption, UserDetails.COMPANYID).ToList();
                if (filtereddata.Count > 0)
                {
                    Report.TechniciansListReport = filtereddata;
                }
                else
                {
                    TempData["ERRORMSG"] = "Data Can not been Found";
                }
                var userdetails = _SessionHelper.GetUser();
                Report.TechnicianList = _technicianRepository.GetAllTechniciansList(userdetails.COMPANYID);
            }
            else
            {
                return View("~/Views/ReportGeneration/TechnicianReportGeneration.cshtml", Report);
            }
            return View("~/Views/ReportGeneration/TechnicianReportGeneration.cshtml", Report);
        }
        [HttpGet]
        public JsonResult GetTechniansdetails(int technician,int statusID,string startdate,string enddate)
        {
            var userdetails = _SessionHelper.GetUser();
            List<CompalinModel> Complaintlist= new List<CompalinModel>();
            DateTime startdatetime=Convert.ToDateTime(startdate);
            DateTime enddatetime=Convert.ToDateTime(enddate);
            if (statusID==0)
            {
                Complaintlist = _ReportGeneration.GetAllSolveDataTechWise(technician, userdetails.COMPANYID, startdatetime, enddatetime).ToList();
            }
            else
            {
                Complaintlist = _ReportGeneration.GetAllSolveDataTechWise(technician, userdetails.COMPANYID, startdatetime, enddatetime).Where(x=>x.STATUSID== statusID).ToList();
            }
           return Json(new
           {
             ProblemList = Complaintlist,
           });
        }
    }
}
