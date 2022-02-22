using Microsoft.AspNetCore.Mvc;

namespace LibraryApplication.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}