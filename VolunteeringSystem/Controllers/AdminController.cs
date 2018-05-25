using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;

namespace VolunteeringSystem.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user, string password)
        {
            if (user == "admin" && password == "admin")
            {
                HttpContext.Session.SetString("user", user);
                HttpContext.Session.SetString("type", "ADMIN");

                return RedirectToAction("Dashboard");
            }

            user = "";
            password = "";
            ViewBag.error = "Usuário ou senha estão incorretos!";

            return View();
        }

        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(new AdministratorDao().GetAll());
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}