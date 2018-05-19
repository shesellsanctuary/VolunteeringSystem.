using Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class VolunteerController : Controller
    {
        private VolunteerDAO volunteerDAO = new VolunteerDAO();

        [HttpGet, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult List(int status)
        {
            var volunteerList = volunteerDAO.GetByStatus(status);
            ViewBag.Status = status == 0 ? "em aprovação" : status == 1 ? "aprovados" : "bloqueados";

            return View(volunteerList);
        }

        [HttpGet, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int volunteerId)
        {
            var volunteer = volunteerDAO.Get(volunteerId);

            return View(volunteer);
        }

        [HttpPost, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int volunteerId, int newStatus)
        {
            volunteerDAO.ChangeStatus(volunteerId, newStatus);

            return RedirectToAction("List", new { status = VolunteerStatus.Waiting });
        }

        [HttpGet]
        public IActionResult Register()
        {
            var Model = new Volunteer();
            return View(Model);
        }


    }
}