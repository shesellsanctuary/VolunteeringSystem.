using System;
using System.IO;
using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly VolunteerDAO volunteerDAO = new VolunteerDAO();

        /* VOLUNTEER ACTIONS */
        [HttpGet]
        [TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Dashboard()
        {
            var volunteerId = Convert.ToInt32(HttpContext.Session.GetString("volunteerId"));
            var volunteer = volunteerDAO.Get(volunteerId);

            return View(volunteer);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var Model = new Volunteer();
            return View(Model);
        }

        [HttpPost]
        public IActionResult Register(Volunteer Model, IFormFile photo, IFormFile criminalRecord)
        {
            if (photo == null || criminalRecord == null)
            {
                ViewBag.Error = "É obrigatório inserir uma foto e o registro de antecedentes criminais";
                return View(Model);
            }

            // Save photo
            var pathPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/volunteers", photo.FileName);
            using (var stream = new FileStream(pathPhoto, FileMode.Create))
            {
                photo.CopyToAsync(stream);
            }

            // Save criminal record
            var pathCriminal = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/criminalRecords",
                criminalRecord.FileName);
            using (var stream = new FileStream(pathCriminal, FileMode.Create))
            {
                criminalRecord.CopyToAsync(stream);
            }

            Model.photo = photo.FileName;
            Model.criminalRecord = criminalRecord.FileName;
            var added = volunteerDAO.Add(Model);
            if (added)
            {
                var volunteerId = volunteerDAO.Login(Model.credentials.email, Model.credentials.password);
                Model.id = volunteerId;
                SetSession(Model);
                return RedirectToAction("Dashboard");
            }

            Model.credentials.email = "";
            ViewBag.Error = "Usuário já existe, por favor insira um e-mail não cadastrado !";
            return View(Model);
        }

        /* ADMIN ACTIONS */
        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult List(int status)
        {
            var volunteerList = volunteerDAO.GetByStatus(status);
            ViewBag.Status = status == 0 ? "em aprovação" : status == 1 ? "aprovados" : "bloqueados";

            return View(volunteerList);
        }

        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int volunteerId)
        {
            var volunteer = volunteerDAO.Get(volunteerId);

            return View(volunteer);
        }

        [HttpPost]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int volunteerId, int newStatus)
        {
            volunteerDAO.ChangeStatus(volunteerId, newStatus);

            return RedirectToAction("List", new {status = VolunteerStatus.Waiting});
        }

        /* FREE ACTIONS */
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var volunteerId = volunteerDAO.Login(email, password);

            if (volunteerId > 0)
            {
                var volunteer = volunteerDAO.Get(volunteerId);
                SetSession(volunteer);

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Usuário ou senha incorretos!";
            return View();
        }

        public void SetSession(Volunteer volunteer)
        {
            HttpContext.Session.SetString("volunteerId", volunteer.id.ToString());
            HttpContext.Session.SetString("volunteerName", volunteer.name);
            HttpContext.Session.SetString("type", "VOLUNTEER");
        }
    }
}