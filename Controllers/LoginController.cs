using Microsoft.AspNetCore.Mvc;

namespace mvc_todolist.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
