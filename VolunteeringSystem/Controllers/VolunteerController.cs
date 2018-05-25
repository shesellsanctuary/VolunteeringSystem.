using System;
using System.IO;
using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Domain;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly VolunteerDao volunteerDAO = new VolunteerDao();

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
            return View(new Volunteer());
        }

        [HttpPost]
        public IActionResult Register(Volunteer model, IFormFile photo, IFormFile criminalRecord)
        {
            if (photo == null || criminalRecord == null)
            {
                ViewBag.Error = "É obrigatório inserir uma foto e o registro de antecedentes criminais!";
                return View(model);
            }

            var pathPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/volunteers", photo.FileName);
            using (var stream = new FileStream(pathPhoto, FileMode.Create))
            {
                photo.CopyToAsync(stream);
            }

            var pathCriminal = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/criminalRecords",
                criminalRecord.FileName);
            using (var stream = new FileStream(pathCriminal, FileMode.Create))
            {
                criminalRecord.CopyToAsync(stream);
            }

            model.photo = photo.FileName;
            model.criminalRecord = criminalRecord.FileName;
            if (volunteerDAO.Add(model))
            {
                var volunteerId = volunteerDAO.Login(model.credentials.email, model.credentials.password);
                model.id = volunteerId;
                SetSession(model);
                return RedirectToAction("Dashboard");
            }

            model.credentials.email = "";
            ViewBag.Error = "Usuário já existe, por favor insira um e-mail não cadastrado!";
            return View(model);
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

        [HttpGet]
        public IActionResult Logout()
        {
            SetSession(null);
            HttpContext.Session.SetString("user", "");
            HttpContext.Session.SetString("type", "");
            return RedirectToAction("Index", "Home");
        }

        public void SetSession(Volunteer volunteer)
        {
            if (volunteer == null)
            {
                HttpContext.Session.SetString("volunteerId", "");
                HttpContext.Session.SetString("volunteerName", "");
                HttpContext.Session.SetString("type", "");
                return;
            }

            HttpContext.Session.SetString("volunteerId", volunteer.id.ToString());
            HttpContext.Session.SetString("volunteerName", volunteer.name);
            HttpContext.Session.SetString("type", "VOLUNTEER");
        }
    }
}