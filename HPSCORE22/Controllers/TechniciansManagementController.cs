using Microsoft.AspNetCore.Mvc;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Interface.CRMInterface;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models.CRMViewModel;
using RetailCare.Models.ServiceModel;
using RetailCare.Models.ServiceViewModel;
using RetailCare.Repositories;
using RetailCare.Repositories.ServiceRepository;

namespace RetailCare.Controllers
{
    public class TechniciansManagementController : Controller
    {
        private readonly IItemRepository  _itemRepository;
        private readonly ICommonMethod _SessionHelper;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IZoneRepository _ZoneRepository;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IProductRepository _ProductDetails;
        public TechniciansManagementController(IItemRepository itemrepo, ICommonMethod SessionHelper,ITechnicianRepository Technicians,
         IZoneRepository ZoneRepository, ICompanyRepository companyRepository, IProductRepository productDetails)
        {
            _itemRepository = itemrepo;
            _SessionHelper = SessionHelper;
            _technicianRepository = Technicians;
            _ZoneRepository = ZoneRepository;
            _CompanyRepository = companyRepository;
            _ProductDetails = productDetails;
        }
        public IActionResult CreateTechnicians()
        {
            TechnicianViewModel Technicians = new TechnicianViewModel();
            Technicians = GetAllData();
            return View("~/Views/TechniciansManagement/CreateTechnicians.cshtml", Technicians);
        }
        [HttpPost]
        public IActionResult SaveTechnicianData(TechnicianViewModel TechniciansDetails)
        {
            var userdetails = _SessionHelper.GetUser();
            TechnicianViewModel Technicians = new TechnicianViewModel();
            if (TechniciansDetails.TechnicianDetails.TECHNICIANID>0)
            {
                if(TechniciansDetails.CheckingClassNameList.Count() > 0)
                {
                    if (TechniciansDetails.TechnicianDetails.CheckedBox == true)
                    {
                        TechniciansDetails.TechnicianDetails.ACTIVE = 1;
                    }
                    else
                    {
                        TechniciansDetails.TechnicianDetails.ACTIVE = 0;
                    }
                    TechniciansDetails.TechnicianDetails.MODIFIEDDATE = DateTime.Now;
                    TechniciansDetails.TechnicianDetails.MODIFIEDBY = userdetails.USERID;
                    var UpdateTechnician = _technicianRepository.UpdateTechnician(TechniciansDetails.TechnicianDetails);
                    if(UpdateTechnician)
                    {
                        var DeletePreviousPermission = _technicianRepository.DeletePreviousPermission(TechniciansDetails.TechnicianDetails.TECHNICIANID);
                        if(DeletePreviousPermission)
                        {
                            foreach (var ClassName in TechniciansDetails.CheckingClassNameList)
                            {
                                var TechniciansClass = new TECHNICIANSASSIGNPRODUCT()
                                {
                                    TECHNICIANID = TechniciansDetails.TechnicianDetails.TECHNICIANID,
                                    PRODUCTID = ClassName,
                                    ACTIVE=1
                                };
                                _technicianRepository.AddAssignClass(TechniciansClass);
                            }
                            TempData["SuccessMSG"] = "Technicins have been added";
                            return RedirectToAction("CreateTechnicians", "TechniciansManagement");
                        }
                        else
                        {
                            TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                            Technicians = GetAllData();
                            Technicians.TechnicianDetails = TechniciansDetails.TechnicianDetails;
                            Technicians.CheckingClassNameList= TechniciansDetails.CheckingClassNameList;
                            return View("~/Views/TechniciansManagement/CreateTechnicians.cshtml", Technicians);
                        }
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                        Technicians = GetAllData();
                        Technicians.TechnicianDetails = TechniciansDetails.TechnicianDetails;
                        Technicians.CheckingClassNameList = TechniciansDetails.CheckingClassNameList;
                        return View("~/Views/TechniciansManagement/CreateTechnicians.cshtml", Technicians);
                    }
                }
            }
            else
            {
                if(ModelState.IsValid)
                {
                    if(TechniciansDetails.CheckingClassNameList.Count()>0)
                    {
                        if (TechniciansDetails.TechnicianDetails.CheckedBox==true)
                        {
                            TechniciansDetails.TechnicianDetails.ACTIVE = 1;
                        }
                        else
                        {
                            TechniciansDetails.TechnicianDetails.ACTIVE = 0;
                        }
                        TechniciansDetails.TechnicianDetails.ENTRYDATE = DateTime.Now;
                        TechniciansDetails.TechnicianDetails.ENTRYBY = userdetails.USERID;
                        var TechniciansID = _technicianRepository.AddTechnician(TechniciansDetails.TechnicianDetails);
                        if (TechniciansID > 0)
                        {
                            foreach (var ClassName in TechniciansDetails.CheckingClassNameList)
                            {
                                var TechniciansClass = new TECHNICIANSASSIGNPRODUCT()
                                {
                                    TECHNICIANID = TechniciansID,
                                    PRODUCTID = ClassName,
                                    ACTIVE=1
                                };
                                _technicianRepository.AddAssignClass(TechniciansClass);
                            }
                            TempData["SuccessMSG"] = "Technicins have been added";
                            return RedirectToAction("CreateTechnicians", "TechniciansManagement");
                        }
                        else
                        {
                            TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                            Technicians = GetAllData();
                            Technicians.TechnicianDetails = TechniciansDetails.TechnicianDetails;
                            Technicians.CheckingClassNameList = TechniciansDetails.CheckingClassNameList;
                            return View("~/Views/TechniciansManagement/CreateTechnicians.cshtml", Technicians);
                        }
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Please Select at Least 1 Class";
                        Technicians = GetAllData();
                        Technicians.TechnicianDetails = TechniciansDetails.TechnicianDetails;
                        Technicians.CheckingClassNameList = TechniciansDetails.CheckingClassNameList;
                        return View("~/Views/TechniciansManagement/CreateTechnicians.cshtml", Technicians);
                    }
                }
                else
                {
                    TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                    Technicians = GetAllData();
                    Technicians.TechnicianDetails = TechniciansDetails.TechnicianDetails;
                    Technicians.CheckingClassNameList = TechniciansDetails.CheckingClassNameList;
                    return View("~/Views/TechniciansManagement/CreateTechnicians.cshtml", Technicians);
                }
            }
            return RedirectToAction("CreateTechnicians", "TechniciansManagement");
        }
        public IActionResult GetAllTechniciansList()
        {
            var userdetails = _SessionHelper.GetUser();
            TechnicianViewModel Technicians = new TechnicianViewModel();
            Technicians.TechnicianList = _technicianRepository.GetAllTechniciansList(userdetails.COMPANYID);
            return View("~/Views/TechniciansManagement/GetAllTechniciansList.cshtml", Technicians);
        }
        public IActionResult EditTechnician(int id)
        {
            TechnicianViewModel Technicians = new TechnicianViewModel();
            if (id > 0)
            {
                var userdetails = _SessionHelper.GetUser();
                Technicians.ZoneList = _ZoneRepository.GetAllZoneDetails();
                //Technicians.CompanyList = _CompanyRepository.GetALLCompany(userdetails.COMPANYID);
                Technicians.TechnicianDetails = _technicianRepository.GetSingleTechnicians(userdetails.COMPANYID,id);
                // All classes
                Technicians.ProductList = _ProductDetails.GetAllProcuctList(userdetails.COMPANYID);
                // Assigned class IDs
                Technicians.CheckingClassNameList = _technicianRepository
                                                .GetTechniciansAssignClassList(id)
                                                .Select(x => x.PRODUCTID)
                                                .ToList();

            }
            return View("~/Views/TechniciansManagement/CreateTechnicians.cshtml", Technicians);
        }

        private TechnicianViewModel GetAllData()
        {
            var userdetails = _SessionHelper.GetUser();
            var technician = new TechnicianViewModel()
            {
                 ZoneList = _ZoneRepository.GetAllZoneDetails(),
                 ProductList = _ProductDetails.GetAllProcuctList(userdetails.COMPANYID),
            };
            return technician;
        }
    }
}
