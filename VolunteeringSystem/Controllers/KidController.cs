using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class KidController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            var kid = new Kid();

            return View(kid);
        }

        [HttpPost]
        public IActionResult Add(Kid model)
        {
            if (ModelState.IsValid) return RedirectToAction("Index", "Home");

            return View(model);
        }
    }
}