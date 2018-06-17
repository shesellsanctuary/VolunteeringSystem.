﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Denied()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}