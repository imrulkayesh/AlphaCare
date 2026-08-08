using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using QCMS.Repositories;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Interface.CRMInterface;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models;
using RetailCare.Models.ServiceModel;
using RetailCare.Models.ServiceViewModel;
using RetailCare.Repositories;
using RetailCare.Repositories.CRMRepository;
using RetailCare.Repositories.ServiceRepository;

namespace RetailCare.Controllers
{
    public class FeedBackController : Controller
    {
        private readonly IAssignmentManagementRepository _TaskAssing;
        private readonly ICommonMethod _SessionHelper;
        private readonly IStatusRepository _Statusrepository;
        private readonly IProductRepository _ProductRepository;
        private readonly IItemRepository _ItemRepository;
        private readonly IProblemRepository _ProblemRepository;
        private readonly ITechnicianRepository _TechniciansRepository;
        private readonly IComplainRepository _ComplainRepository;
        private readonly IBrandRepository _BrandRepository;
        private readonly IFeedbackRepository _FeedBackRepository;
        private readonly IWebHostEnvironment _environment;
        public FeedBackController(IAssignmentManagementRepository TaskAssing, ICommonMethod sessionHelper, IStatusRepository Statusrepository,
         IProductRepository ProductRepository, IItemRepository ItemRepository, IProblemRepository ProblemRepository, ITechnicianRepository TechniciansRepository,
         IComplainRepository ComplainRepository, IBrandRepository BrandRepository, IFeedbackRepository FeedBackRepository, IWebHostEnvironment environment)
        {
            _TaskAssing = TaskAssing;
            _SessionHelper = sessionHelper;
            _Statusrepository = Statusrepository;
            _ProductRepository = ProductRepository;
            _ItemRepository = ItemRepository;
            _ProblemRepository = ProblemRepository;
            _TechniciansRepository = TechniciansRepository;
            _ComplainRepository = ComplainRepository;
            _BrandRepository = BrandRepository;
            _FeedBackRepository = FeedBackRepository;
            _environment = environment;
        }
        public IActionResult GetAllTokenListForFeedback()
        {
            var userdetails = _SessionHelper.GetUser();
            FeedBackViewModel feedBackViewModel = new FeedBackViewModel();
            var techniciansDetails = _TechniciansRepository.GetSingleTechnicianUsingStaffID(userdetails.STAFFID);
            if (techniciansDetails != null)
            {
                feedBackViewModel.AssignmentListForFeedback = _TaskAssing.GetAllAssignmentListForFeedbackTechnicianIDWise(techniciansDetails.TECHNICIANID);
            }
            else
            {
                feedBackViewModel.AssignmentListForFeedback = _TaskAssing.GetAllAssignmentListForFeedback(userdetails.COMPANYID);
            }  
            return View("~/Views/FeedBack/GetAllTokenListForFeedback.cshtml", feedBackViewModel);
        }
        public IActionResult CreateNewFeedback(string id)
        {
            FeedBackViewModel feedBackViewModel = new FeedBackViewModel();
            var userdetails = _SessionHelper.GetUser();
            var assignment = _TaskAssing.GetSingleTaskUsingTickedID(id);
            if (assignment != null)
            {
                feedBackViewModel.FeedBackDetails = new FeedabackModel
                {
                    TICKETID = assignment.TICKETID,
                    CUSTOMERNAME = assignment.CUSTOMERNAME,
                    CUSTOMERCONTACTNO = assignment.CUSTOMERCONTACTNO,
                    CUSTOMERADDRESS = assignment.CUSTOMERADDRESS,
                    PRODUCTID = assignment.PRODUCTID,
                    ITEMID=assignment.ITEMID,
                    ACTUALPROBLEMID = assignment.PROBLEMID,
                    STATUSID=assignment.STATUSID,
                    SOLVEDBY=assignment.TECHNICIANID.ToString()
                };
            }
          //  feedBackViewModel.CompalinModel = _ComplainRepository.GetComplainUsingTickedID(id);
            feedBackViewModel.StatusList = _Statusrepository.GetAllStatus(userdetails.COMPANYID).Where(x=>x.STATUSID!=1).ToList();
            feedBackViewModel.ProductList = _ProductRepository.GetAllProcuctList(userdetails.COMPANYID);
            feedBackViewModel.ProductModelsList = _ProductRepository.GetALlProductModelUsingProductID(assignment.PRODUCTID);
            feedBackViewModel.ItemList = _ItemRepository.GetAllItemList(userdetails.COMPANYID);
            feedBackViewModel.ProblemList = _ProblemRepository.GetAllProblemList(userdetails.COMPANYID);
            feedBackViewModel.SolvedBy = _TechniciansRepository.GetAllTechniciansList(userdetails.COMPANYID);
            feedBackViewModel.AssistBy1 = _TechniciansRepository.GetAllTechnicianData(userdetails.COMPANYID, assignment.ITEMID);
            feedBackViewModel.AssistBy2 = _TechniciansRepository.GetAllTechnicianData(userdetails.COMPANYID, assignment.ITEMID);
            feedBackViewModel.BrandList = _BrandRepository.GetAllBrandList(userdetails.COMPANYID);
            feedBackViewModel.SubproblemList = _ProblemRepository.GetAllSubproblemProblemWise(assignment.PROBLEMID);
            return View("~/Views/FeedBack/CreateNewFeedback.cshtml", feedBackViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SaveFeedBackDataAsync([Bind(Prefix = "FeedBackDetails")] FeedabackModel FeedbackData, List<string> SelectedSubProblemList,
    IFormFile? formFile)
        {
            FeedabackModel feedBackViewModel = new FeedabackModel();
            if (ModelState.IsValid)
            {
                if(FeedbackData.STATUSID==2 || FeedbackData.STATUSID==3)
                {
                    FeedbackData.WORKINGDATE=DateTime.Now;
                }
                FeedbackData.SUBPROBLEM = string.Join(", ",SelectedSubProblemList);
                FeedbackData.SRMODE = "N";
                FeedbackData.PSMODE = "N";
                FeedbackData.ISAPPROVED = 0;
                FeedbackData.SENDCOST =0;
                if (formFile != null && formFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_environment.WebRootPath, "FeedbackImages");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    // THIS IS MISSING
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    FeedbackData.FILEPATH = "~/FeedbackImages/" + fileName;
                }
                var userdetails = _SessionHelper.GetUser();
                FeedbackData.ENTRYBY = userdetails.USERID;
                FeedbackData.ENTRYDATE = DateTime.Now;
                var insertedData = _FeedBackRepository.AddNewFeedback(FeedbackData);
                if (insertedData)
                {
                    var assign = _TaskAssing.GetSingleTaskUsingTickedID(FeedbackData.TICKETID);
                    if(assign !=null)
                    {
                        assign.STATUSID =(int) FeedbackData.STATUSID;
                        assign.SENDFEEDBACK = 1;
                        var Updateassing = _TaskAssing.UpdateAssign(assign);
                        if(Updateassing)
                        {
                            TempData["SuccessMSG"] = "The data have been added Sucessfully";
                            return RedirectToAction("GetAllTokenListForFeedback", "FeedBack");
                        }
                    }
                }
                else
                {
                    TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                    return View("~/Views/FeedBack/CreateNewFeedback.cshtml", FeedbackData);
                }
            }
            else
            {
                TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                return View("~/Views/FeedBack/CreateNewFeedback.cshtml", FeedbackData);
            }
            return RedirectToAction("GetAllTokenListForFeedback", "FeedBack");
        }
        // Ajax code feedbacks
        //public JsonResult GetAllClassProducts(int BrandID)
        //{
        //   List<ItemModel> ItemModel  = new List<ItemModel>();
        //    if(BrandID>0)
        //    {
        //        ItemModel= _ItemRepository.GetAllItemListBrandWise(BrandID);
        //    }
        //    return Json(ItemModel);
        //}
    }
}
