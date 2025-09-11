using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
