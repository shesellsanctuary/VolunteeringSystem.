using Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    [TypeFilter(typeof(IsLoggedAttribute))]
    public class ProfessionalController : Controller
    {
        [HttpGet, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult List()
        {
            return View(new ProfessionalDao().GetAll());
        }

        [HttpGet, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult Add()
        {
            var professional = new Professional();
            return View(professional);
        }

        [HttpPost, CheckAccess(new string[] { "ADMIN" })]
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

        [HttpGet, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult Details(int professionalId)
        {
            return View(new ProfessionalDao().Get(professionalId));
        }
    }
}