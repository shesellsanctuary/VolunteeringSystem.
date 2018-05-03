using Microsoft.AspNetCore.Mvc;

namespace VolunteeringSystem.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (email == "teste" && password == "teste")
            {
                //logInUser();
                return Redirect("Home");
            }
            else
            {
                email = "";
                password = "";
                ViewBag.error = "Erro!";
                return View();
            }
        }
    }
}