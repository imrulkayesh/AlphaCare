using QCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace QCMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("USER_NAME");
            ViewBag.DistName = HttpContext.Session.GetString("DIST_NAME");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login1()
        {
            return View();
        }
        public IActionResult datatable()
        {
            return View();
        }
        public IActionResult TestAlert()
        {
            return View();
        }
        public IActionResult CustomerInfo()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        
    }
}
