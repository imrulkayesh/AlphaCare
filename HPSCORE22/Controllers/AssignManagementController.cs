using Microsoft.AspNetCore.Mvc;

namespace RetailCare.Controllers
{
    public class AssignManagementController : Controller
    {
        // For assignment Management
        public AssignManagementController()
        {
        
        }
        public IActionResult GetAssignList()
        {

            return View();
        }
    }
}
