using System;
using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public void SetSession(string name, string type)
        {
            if (name == null || type == null)
            {
                HttpContext.Session.SetString("user", "");
                HttpContext.Session.SetString("type", "");
                return;
            }

            HttpContext.Session.SetString("user", name);
            HttpContext.Session.SetString("type", type.ToUpper());
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user, string password)
        {
            var admin = new AdministratorDao().Login(user, password);
            if (admin != null)
            {
                SetSession(admin.name, "ADMIN");
                return RedirectToAction("Dashboard");
            }
            var professional = new ProfessionalDao().Login(user, password);
            if (professional != null)
            {
                SetSession(professional.name, "PROFESSIONAL");
                return RedirectToAction("Dashboard");
            }

            ViewBag.error = "Usuário ou senha estão incorretos!";
            return View();
        }

        [TypeFilter(typeof(IsLoggedAttribute)), CheckAccess(new string[] { "ADMIN", "PROFESSIONAL" })]
        public IActionResult Logout()
        {
            SetSession(null, null);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, TypeFilter(typeof(IsLoggedAttribute)), CheckAccess(new string[] { "ADMIN", "PROFESSIONAL" })]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult List()
        {
            return View(new AdministratorDao().GetAll());
        }

        [HttpGet, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult Add()
        {
            var admin = new Administrator();
            return View(admin);
        }

        [HttpPost, TypeFilter(typeof(IsLoggedAttribute)), CheckAccess(new string[] { "ADMIN" })]
        public IActionResult Add(Administrator Model, string type, string code)
        {
            if (ModelState.IsValid)
            {
                var _administratorDao = new AdministratorDao();
                _administratorDao.Add(Model);
                return RedirectToAction("List");
            }

            return View(Model);
        }

        [HttpGet, TypeFilter(typeof(IsLoggedAttribute)), CheckAccess(new string[] { "ADMIN" })]
        public IActionResult Details(int adminId)
        {
            return View(new AdministratorDao().Get(adminId));
        }
    }
}