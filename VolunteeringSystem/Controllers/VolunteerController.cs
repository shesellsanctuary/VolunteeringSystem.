using Admin.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class VolunteerController : Controller
    {
        private VolunteerDAO volunteerDAO = new VolunteerDAO();

        private readonly IHostingEnvironment _environment;

        // Constructor
        public VolunteerController(IHostingEnvironment IHostingEnvironment)
        {
            _environment = IHostingEnvironment;
        }

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

            // Command line argument must the the SMTP host.
            //SmtpClient client = new SmtpClient();
            //client.Port = 587;
            //client.Host = "smtp.gmail.com";
            //client.EnableSsl = true;
            //client.Timeout = 10000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential("delazeri1997@gmail.com", "password");

            //MailMessage mm = new MailMessage("delazeri1997@gmail.com", "guilherme_delazeri@hotmail.com", "test", "test");
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            //client.Send(mm);

            return RedirectToAction("List", new { status = VolunteerStatus.Waiting });
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
            var pathCriminal = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/criminalRecords", criminalRecord.FileName);
            using (var stream = new FileStream(pathCriminal, FileMode.Create))
            {
                criminalRecord.CopyToAsync(stream);
            }

            Model.photo = photo.FileName;
            Model.criminalRecord = criminalRecord.FileName;
            var added = volunteerDAO.Add(Model);
            if (!added)
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