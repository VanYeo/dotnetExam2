using Microsoft.AspNetCore.Mvc;

namespace dotnetExam2.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
