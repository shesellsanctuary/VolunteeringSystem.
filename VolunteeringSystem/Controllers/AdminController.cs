﻿using System;
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

        [HttpGet, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Add()
        {
            var admin = new Administrator();
            return View(admin);
        }

        [HttpPost, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Add(Administrator Model, string type, string code)
        {
            if (ModelState.IsValid)
            {
                var _administratorDao = new AdministratorDao();
                switch (type)
                {
                    case "ADMIN":
                        _administratorDao.Add(Model);
                        break;
                    case "MEDIC":
                        var medic = new Medic
                        {
                            name = Model.name,
                            birthDate = Model.birthDate,
                            CPF = Model.CPF,
                            sex = Model.sex,
                            credentials = Model.credentials
                        };
                        medic.CRM = code;
                        _administratorDao.Add(medic);
                        break;
                    case "PSYCHOLOGY":
                        var psychology = new Psychology
                        {
                            name = Model.name,
                            birthDate = Model.birthDate,
                            CPF = Model.CPF,
                            sex = Model.sex,
                            credentials = Model.credentials
                        };
                        psychology.CFP = code;
                        _administratorDao.Add(psychology);
                        break;
                    case "EDUCATOR":
                        var educator = new Educator
                        {
                            name = Model.name,
                            birthDate = Model.birthDate,
                            CPF = Model.CPF,
                            sex = Model.sex,
                            credentials = Model.credentials
                        };
                        educator.CFEP = code;
                        _administratorDao.Add(educator);
                        break;
                    default:
                        ViewBag.Error = "Informe o tipo de administrador";
                        return View(Model);
                }

                return RedirectToAction("List");
            }

            return View(Model);
        }
    }
}