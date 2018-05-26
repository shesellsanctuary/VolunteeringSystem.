using System;
using System.IO;
using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Domain;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ILogger<VolunteerController> _logger;
        private readonly VolunteerDao _volunteerDao = new VolunteerDao();

        public VolunteerController(ILogger<VolunteerController> logger)
        {
            _logger = logger;
        }

        /* VOLUNTEER ACTIONS */
        [HttpGet]
        [TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Dashboard()
        {
            var volunteerId = Convert.ToInt32(HttpContext.Session.GetString("volunteerId"));
            var volunteer = _volunteerDao.Get(volunteerId);
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

            var pathPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets", photo.FileName);
            using (var stream = new FileStream(pathPhoto, FileMode.Create))
            {
                photo.CopyToAsync(stream);
            }

            var pathCriminal = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets",
                criminalRecord.FileName);
            using (var stream = new FileStream(pathCriminal, FileMode.Create))
            {
                criminalRecord.CopyToAsync(stream);
            }

            model.photo = photo.FileName;
            model.criminalRecord = criminalRecord.FileName;
            if (_volunteerDao.Add(model)) return Login(model.credentials.email, model.credentials.password);

            model.credentials.email = "";
            ViewBag.Error = "Usuário já existe, por favor insira um e-mail não cadastrado!";
            return View(model);
        }

        /* ADMIN ACTIONS */
        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult List(int status)
        {
            var volunteerList = _volunteerDao.GetByStatus(status);
            ViewBag.Status = ((VolunteerStatus) status).ToPortugueseString() + "s";
            return View(volunteerList);
        }

        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int volunteerId)
        {
            var volunteer = _volunteerDao.Get(volunteerId);
            return View(volunteer);
        }

        [HttpPost]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int volunteerId, int newStatus)
        {
            _volunteerDao.ChangeStatus(volunteerId, newStatus);
            return RedirectToAction("List", new {status = VolunteerStatus.Waiting});
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                SetSession(_volunteerDao.Login(email, password));
                return RedirectToAction("Dashboard");
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception.Message);
                ViewBag.Error = "Usuário ou senha incorretos!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            SetSession(null);
            HttpContext.Session.SetString("user", "");
            return RedirectToAction("Index", "Home");
        }
    }
}