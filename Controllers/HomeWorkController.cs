using Microsoft.AspNetCore.Mvc;

namespace Ajax0122.Controllers
{
    public class HomeWorkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckAccount()
        {
            return View();
        }

        public IActionResult Register() 
        {
            return View();
        }

        public IActionResult Spots() 
        {
            return View();
        }
    }
}
