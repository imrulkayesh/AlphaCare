using Microsoft.AspNetCore.Mvc;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models;
using RetailCare.Models.ServiceModel;
using RetailCare.Models.ServiceViewModel;

namespace RetailCare.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IBrandRepository _BrandRepository;
        private readonly ICommonMethod _SessionHelper;
        public ProductManagementController(IItemRepository itemRepository, IBrandRepository BrandRepository, ICommonMethod SessionHelper)
        {
            _itemRepository = itemRepository;
            _BrandRepository = BrandRepository;
            _SessionHelper = SessionHelper;
        }
        public IActionResult CreateNewBrand()
        {
            var userDetails = _SessionHelper.GetUser();
            ProductManagementViewModel brand = new ProductManagementViewModel();
            brand.BrandsList = _BrandRepository.GetAllBrandList(userDetails.COMPANYID);
            return View("~/Views/ProductManagement/CreateNewBrand.cshtml", brand);
        }
        public IActionResult SaveBrandData([Bind(Prefix = "Brand")] BrandModel Brandetails)
        {
            var userDetails = _SessionHelper.GetUser();
            ProductManagementViewModel brand = new ProductManagementViewModel();
            if(Brandetails.GROUPID>0)
            {
                Brandetails.MODIFIEDBY = userDetails.USERID;
                Brandetails.MODIFIEDDATE = DateTime.Now;
                var UpdatedValues = _BrandRepository.UpdateGroup(Brandetails);
                if (UpdatedValues)
                {
                    TempData["SuccessMSG"] = "Sucessfully Updated";
                    return RedirectToAction("CreateNewBrand", "ProductManagement");
                }
                else
                {
                    TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                    var userD = _SessionHelper.GetUser();
                    ProductManagementViewModel brandDetails = new ProductManagementViewModel();
                    brand.BrandsList = _BrandRepository.GetAllBrandList(userD.COMPANYID);
                    return View("~/Views/ProductManagement/CreateNewBrand.cshtml", brandDetails);
                }
            }
            else
            {
                if(ModelState.IsValid)
                {
                    Brandetails.ENTRYBY= userDetails.USERID;
                    Brandetails.ENTRYDATE= DateTime.Now;
                    var ID=  _BrandRepository.InsertGroup(Brandetails);
                    if(ID>0)
                    {
                        TempData["SuccessMSG"] = "Sucessfully added";
                        return RedirectToAction("CreateNewBrand", "ProductManagement");
                    }
                    else
                    {
                        TempData["ERRORMSG"] = "Server Error. Please try again after some time";
                        var userD= _SessionHelper.GetUser();
                        ProductManagementViewModel brandDetails = new ProductManagementViewModel();
                        brand.BrandsList = _BrandRepository.GetAllBrandList(userD.COMPANYID);
                        return View("~/Views/ProductManagement/CreateNewBrand.cshtml", brandDetails);
                    }
                }
            }
            return View("~/Views/ProductManagement/CreateNewBrand.cshtml", brand);
        }
        public IActionResult EditBrand(int id)
        {
            var userDetails = _SessionHelper.GetUser();
            ProductManagementViewModel brand = new ProductManagementViewModel();
            brand.Brand = _BrandRepository.GetBrandUsngBrandID(id);
            return View("~/Views/ProductManagement/CreateNewBrand.cshtml", brand);
        }
        // Class Setup
        public IActionResult CreateNewClass()
        {
            var userDetails = _SessionHelper.GetUser();
            ProductManagementViewModel brand = new ProductManagementViewModel();
            brand.Items = _itemRepository.GetAllItemList(userDetails.COMPANYID);
            return View("~/Views/ProductManagement/CreateNewClass.cshtml", brand);
        }
        public IActionResult SaveClassData([Bind(Prefix = "ItemDetails")] ItemModel ClassDetails)
        {
            ProductManagementViewModel brand = new ProductManagementViewModel();
            if (ClassDetails.ITEMID > 0)
            {
                var UpdatedValues = _itemRepository.UpdateClass(ClassDetails);
                if(UpdatedValues)
                {
                    TempData["SuccessMSG"] = "Sucessfully Updated";
                    return RedirectToAction("CreateNewBrand", "ProductManagement");
                }
                else
                {
                    var userDetails = _SessionHelper.GetUser();
                    brand.Items = _itemRepository.GetAllItemList(userDetails.COMPANYID);
                    return View("~/Views/ProductManagement/CreateNewClass.cshtml", brand);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    TempData["SuccessMSG"] = "Sucessfully added";
                    return RedirectToAction("CreateNewClass", "ProductManagement");
                }
                else
                {
                    var userDetails = _SessionHelper.GetUser();
                    brand.Items = _itemRepository.GetAllItemList(userDetails.COMPANYID);
                    return View("~/Views/ProductManagement/CreateNewClass.cshtml", brand);
                }
            }
            return View();
        }
        public IActionResult EditClass(int id)
        {
            var userDetails = _SessionHelper.GetUser();
            ProductManagementViewModel brand = new ProductManagementViewModel();
            brand.BrandsList = _BrandRepository.GetAllBrandList(userDetails.COMPANYID);
            return View("~/Views/ProductManagement/CreateNewClass.cshtml", brand);
        }
    }
}
