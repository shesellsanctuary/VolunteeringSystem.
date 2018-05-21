using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class VolunteerController : Controller
    {
        private VolunteerDAO volunteerDAO = new VolunteerDAO();

        [HttpGet, TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Dashboard()
        {
            var volunteerId = Convert.ToInt32(HttpContext.Session.GetString("volunteerId"));
            var volunteer = volunteerDAO.Get(volunteerId);

            return View(volunteer);
        }

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

        [HttpPost]
        public IActionResult Register(Volunteer Model)
        {
            Model.photo = "";
            var added = volunteerDAO.Add(Model);

            if(!added)
            {
                Model.credentials.email = "";
                ViewBag.Error = "Usuário já existe, por favor insira um e-mail não cadastrado !";
                return View(Model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet, TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Created()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            int volunteerId = volunteerDAO.Login(email, password);

            if (volunteerId > 0)
            {
                var volunteer = volunteerDAO.Get(volunteerId);
                
                HttpContext.Session.SetString("volunteerId", volunteerId.ToString());
                HttpContext.Session.SetString("volunteerName", volunteer.name);
                HttpContext.Session.SetString("type", "VOLUNTEER");
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Usuário ou senha incorretos!";
            return View();
        }
    }
}