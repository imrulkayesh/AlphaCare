using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using QCMS.Repositories;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Interface.CRMInterface;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models.CRMModels;
using RetailCare.Models.CRMViewModel;
using RetailCare.Models.ServiceModel;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RetailCare.Controllers
{
    public class ComplainGenerationController : Controller
    {
        private readonly IComplainRepository _complainRepository;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IStatusRepository _statusrepository;
        private readonly ICommonMethod _SessionHelper;
        private readonly IProblemRepository _ProblemRepository;
        private readonly IProductRepository _ProductDetails;
        private readonly ICommonServiceMethods _CommonServiceModel;
        private readonly IItemRepository _itemRepository;
        private readonly ITechnicianRepository _TechniciansData;
        private readonly IAssignmentManagementRepository _TaskAssing;
        private readonly IZoneRepository _ZoneRepository;
        ComplainGenerationViewModel Complain = new ComplainGenerationViewModel();
        public ComplainGenerationController(IComplainRepository ComplainRepository, ICompanyRepository CompanyRepository, IStatusRepository Status,
            ICommonMethod CommonMethod, IProblemRepository ProblemRepository, IProductRepository ProductDetails, ICommonServiceMethods CommonServiceModel,
           IItemRepository itemRepository, ITechnicianRepository TechniciansData, IAssignmentManagementRepository TaskAssing, IZoneRepository ZoneRepository)
        {
            _complainRepository = ComplainRepository;
            _CompanyRepository = CompanyRepository;
            _statusrepository = Status;
            _SessionHelper = CommonMethod;
            _ProblemRepository = ProblemRepository;
            _ProductDetails = ProductDetails;
            _CommonServiceModel = CommonServiceModel;
            _itemRepository = itemRepository;
            _TechniciansData = TechniciansData;
            _TaskAssing = TaskAssing;
            _ZoneRepository = ZoneRepository;
        }

        public IActionResult CreateToken()
        {
            var userdetails = _SessionHelper.GetUser();
            Complain = GetAllData();
            Complain.ComaplainModel.COMPLAINDATE = DateTime.Now;
            Complain.ComaplainModel.CUSTOMERNAME = userdetails.EMPLOYEE_NAME;
            Complain.ComaplainModel.CONTACTNO = userdetails.CONTACT;
            Complain.ComaplainModel.LOCATION = userdetails.ADDRESS;
            Complain.ComaplainModel.SHOWROOM = userdetails.EMPLOYEE_CODE;
            return View("~/Views/ComplainGeneration/CreateToken.cshtml", Complain);
        }
        public IActionResult SaveDataComplain([Bind(Prefix = "ComaplainModel")] CompalinModel ComplainData, List<ComplainProblemModel> ProblemListAdded)
        {
            if (ComplainData.TICKETID > 0)
            {
                var userdetails = _SessionHelper.GetUser();
                ComplainData.MODIFIEDDATE = DateTime.Now;
                ComplainData.MODIFIEDBY = _SessionHelper.GetUser().USERID;
                var InsertedData = _complainRepository.UpdateCompalin(ComplainData);
                if (InsertedData)
                {
                    var UpdateAssignTable = AssignTask(ComplainData, 1);
                    if (UpdateAssignTable)
                    {
                        var DeleteProblemList = _complainRepository.DeleteProblemList(ComplainData.TICKETCODE);
                        if (DeleteProblemList)
                        {
                            foreach (var item in ProblemListAdded)
                            {
                                item.TICKETCODE = ComplainData.TICKETCODE;
                                _complainRepository.InsertComplainDetails(item);
                            }
                            TempData["SuccessMSG"] = ComplainData.TICKETCODE + "have been Updated";
                            return RedirectToAction("CreateToken", "ComplainGeneration");
                        }
                        else
                        {
                            TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                            Complain = GetAllData();
                            Complain.ComaplainModel = ComplainData;
                            return View("~/Views/ComplainGeneration/CreateToken.cshtml", Complain);
                        }
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                        Complain = GetAllData();
                        Complain.ComaplainModel = ComplainData;
                        return View("~/Views/ComplainGeneration/CreateToken.cshtml", Complain);
                    }
                }
                else
                {
                    TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                    Complain = GetAllData();
                    Complain.ComaplainModel = ComplainData;
                    return View("~/Views/ComplainGeneration/CreateToken.cshtml", Complain);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (ProblemListAdded.Count() > 0)
                    {
                        // Fetching All the Data From database 
                        var userdetails = _SessionHelper.GetUser();
                        var NextTicketCode = _CommonServiceModel.GeneratingTickedCode();
                        var CompanyDetails = _CompanyRepository.GetSingleCompanyDetails(userdetails.COMPANYID);
                        // Generating Model & Complain Data for Insertion
                        ComplainData.TICKETID = NextTicketCode;
                        ComplainData.COMPANYID = userdetails.COMPANYID;
                        ComplainData.ISSENDFEEDBACK = 0;
                        ComplainData.TICKETCODE = CreatingSyntaxForTicket(CompanyDetails.TOKENSYSNTAX, NextTicketCode);
                        ComplainData.ENTRYDATE = DateTime.Now;
                        ComplainData.ENTRYBY = _SessionHelper.GetUser().USERID;
                        ComplainData.TICKETID = NextTicketCode;
                        ComplainData.ISACTIVE = 1;
                        //  ComplainData.ZONEID = 1;
                        var InsertedData = _complainRepository.AddNewComplain(ComplainData);
                        if (InsertedData)
                        {
                            foreach (var item in ProblemListAdded)
                            {
                                item.TICKETCODE = ComplainData.TICKETCODE;
                                _complainRepository.InsertComplainDetails(item);
                            }
                            var InsertAssingTable = AssignTask(ComplainData, 0);
                            if (InsertAssingTable)
                            {
                                TempData["SuccessMSG"] = "New Ticket:" + ComplainData.TICKETCODE;
                                return RedirectToAction("CreateToken", "ComplainGeneration");
                            }
                            else
                            {
                                TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                                Complain = GetAllData();
                                Complain.ComaplainModel = ComplainData;
                                return View("~/Views/ComplainGeneration/CreateToken.cshtml", Complain);
                            }
                        }
                    }
                }
                else
                {
                    TempData["ERRORMSG"] = "Enter all the required Fileds";
                    Complain = GetAllData();
                    Complain.TechnicianList = _TechniciansData.GetAllTechnicianData(ComplainData.COMPANYID, ComplainData.PROBLEMTYPEID);
                    Complain.ComaplainModel = ComplainData;
                    return View("~/Views/ComplainGeneration/CreateToken.cshtml", Complain);
                }
            }
            return View("~/Views/ComplainGeneration/CreateToken.cshtml", Complain);
        }
        private string CreatingSyntaxForTicket(string CompanyPrefix, int NextTicketCode)
        {
            DateTime CurrentDate = DateTime.Now;
            var Year = CurrentDate.Year.ToString().Substring(2, 2);
            var Month = CurrentDate.Month.ToString().PadLeft(2, '0');
            var Day = CurrentDate.Day.ToString().PadLeft(2, '0');
            var NextTicket = NextTicketCode.ToString().PadLeft(6, '0');
            string TicketCode = CompanyPrefix + Year + Month + Day + NextTicket;
            return TicketCode;
        }
        public bool AssignTask(CompalinModel ComplainModel, int Update)
        {
            bool IsAdded = true;
            if (ComplainModel != null)
            {
                if (Update > 0)
                {
                    try
                    {
                        var AssignTable = new AssignmentModel()
                        {
                            TICKETID = ComplainModel.TICKETCODE,
                            ASSIGNDATE = DateTime.Now,
                            STATUSID = ComplainModel.STATUSID,
                            TECHNICIANID = ComplainModel.TECHNICIANID,
                            COMPANYID = ComplainModel.COMPANYID,
                            CUSTOMERNAME = ComplainModel.CUSTOMERNAME,
                            CUSTOMERCONTACTNO = ComplainModel.CONTACTNO,
                            CUSTOMERADDRESS = ComplainModel.LOCATION,
                            ENTRYBY = ComplainModel.ENTRYBY,
                            ENTRYDATE = ComplainModel.ENTRYDATE,
                            SENDFEEDBACK = 0,
                            ISASSIGN = 1,
                            PROBLEMID = ComplainModel.PROBLEMTYPEID,
                        };
                        _TaskAssing.UpdateAssign(AssignTable);
                    }
                    catch (Exception ex)
                    {
                        IsAdded = false;
                    }
                }
                else
                {
                    try
                    {
                        var AssignTable = new AssignmentModel()
                        {
                            TICKETID = ComplainModel.TICKETCODE,
                            ASSIGNDATE = DateTime.Now,
                            STATUSID = ComplainModel.STATUSID,
                            TECHNICIANID = ComplainModel.TECHNICIANID,
                            COMPANYID = ComplainModel.COMPANYID,
                            CUSTOMERNAME = ComplainModel.CUSTOMERNAME,
                            CUSTOMERCONTACTNO = ComplainModel.CONTACTNO,
                            CUSTOMERADDRESS = ComplainModel.LOCATION,
                            ENTRYBY = ComplainModel.ENTRYBY,
                            ENTRYDATE = ComplainModel.ENTRYDATE,
                            SENDFEEDBACK = 0,
                            ISASSIGN = 1,
                            PROBLEMID = ComplainModel.PROBLEMTYPEID,
                        };
                        _TaskAssing.InsertAssignTechnician(AssignTable);
                    }
                    catch (Exception ex)
                    {
                        IsAdded = false;
                    }
                }

            }
            return IsAdded;
        }
        [HttpGet]
        public JsonResult GetProductWiseProductModel(int ProductID)
        {
            var userdetails = _SessionHelper.GetUser();
            var TechnicianData = _TechniciansData.GetAllTechnicianData(userdetails.COMPANYID, ProductID);
            var ProblemData = _ProblemRepository.GetAllProblemList(userdetails.COMPANYID).Where(x => x.PRODUCTID == ProductID).ToList();
            return Json(new
            {
                ProblemDataModel = ProblemData,
                TechniciansList = TechnicianData
            });
        }
        public IActionResult GetAllTicket()
        {
            var userdetails = _SessionHelper.GetUser();
            Complain = GetAllData();
            if (userdetails.USERTYPEID==4)
            {
                Complain.ComplainList = _complainRepository.GetALlComplainList(userdetails.COMPANYID).Where(x => x.SHOWROOM == userdetails.EMPLOYEE_CODE).ToList();
            }
            else 
            {
                Complain.ComplainList = _complainRepository.GetALlComplainList(userdetails.COMPANYID);
            }
            return View("~/Views/ComplainGeneration/GetAllTicket.cshtml", Complain);
        }
        [HttpGet]
        public JsonResult GetAllComplainProblemList(string ticketcode)
        {
            var userdetails = _SessionHelper.GetUser();
            var data = _complainRepository.GetAllPromlemDetails(ticketcode).ToList();
            return Json(new
            {
                ProblemList = data
            });
        }
        public IActionResult EditCompplain(int id)
        {
            ComplainGenerationViewModel complain = new ComplainGenerationViewModel();
            if (id > 0)
            {
                var userdetails = _SessionHelper.GetUser();
                complain.StatusList = _statusrepository.GetAllStatus(userdetails.COMPANYID).Where(x => x.ISDEFAULT == 1).ToList();
                complain.ComaplainModel = _complainRepository.GetComplainListUsingID(id);
                complain.ProblemListAdded = _complainRepository.GetAllPromlemDetails(complain.ComaplainModel.TICKETCODE).ToList();
                complain.ProblemList = _ProblemRepository.GetAllProblemList(userdetails.COMPANYID).Where(x => x.PRODUCTID == complain.ComaplainModel.PROBLEMTYPEID).ToList();
                complain.StatusList = _statusrepository.GetAllStatus(userdetails.COMPANYID).Where(x => x.STATUSID == 1).ToList();
                complain.TechnicianList = _TechniciansData.GetAllTechnicianData(userdetails.COMPANYID, complain.ComaplainModel.PROBLEMTYPEID);
                complain.ProductList = _ProductDetails.GetAllProcuctList(userdetails.COMPANYID);
            }
            return View("~/Views/ComplainGeneration/CreateToken.cshtml", complain);
        }
        private ComplainGenerationViewModel GetAllData()
        {
            var userdetails = _SessionHelper.GetUser();
            var ComplainGeneration = new ComplainGenerationViewModel()
            {
                ProductList = _ProductDetails.GetAllProcuctList(userdetails.COMPANYID),
                StatusList = _statusrepository.GetAllStatus(userdetails.COMPANYID).Where(x => x.STATUSID == 1).ToList()
            };
            return ComplainGeneration;
        }
    }
}
