using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class KidController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View(new Kid());
        }

        [HttpPost]
        public IActionResult Add(Kid model)
        {
            if (ModelState.IsValid) return RedirectToAction("Index", "Home");
            return View(model);
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(new KidDao().GetAll());
        }
    }
}