using Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class ProfessionalController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(new ProfessionalDao().GetAll());
        }

        [HttpGet]
        public IActionResult Add()
        {
            var professional = new Professional();
            return View(professional);
        }

        [HttpPost]
        public IActionResult Add(Professional Model)
        {
            if (ModelState.IsValid)
            {
                var _professionalDao = new ProfessionalDao();
                _professionalDao.Add(Model);
                return RedirectToAction("List");
            }

            return View(Model);
        }

        [HttpGet]
        public IActionResult Details(int professionalId)
        {
            return View(new ProfessionalDao().Get(professionalId));
        }
    }
}