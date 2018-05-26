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

        public void SetSession(Administrator administrator)
        {
            if (administrator == null)
            {
                HttpContext.Session.SetString("user", "");
                HttpContext.Session.SetString("type", "");
                return;
            }

            HttpContext.Session.SetString("user", administrator.name);
            HttpContext.Session.SetString("type", "ADMIN");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user, string password)
        {
            try
            {
                SetSession(new AdministratorDao().Login(user, password));
                return RedirectToAction("Dashboard");
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception.Message);
                ViewBag.error = "Usuário ou senha estão incorretos!";
                return View();
            }
        }

        public IActionResult Logout()
        {
            SetSession(null);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(new AdministratorDao().GetAll());
        }
    }
}