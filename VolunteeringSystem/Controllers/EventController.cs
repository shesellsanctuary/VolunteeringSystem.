using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class EventController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var Model = new Event();
            EventDAO eventDAO = new EventDAO();
            AgeGroupDAO ageGroupDAO = new AgeGroupDAO();

            // ViewBag é um dicionário de dados, no qual podemos mandar dados para dentro da View
            ViewBag.ageGroups = ageGroupDAO.ToSelectList(ageGroupDAO.GetAll());

            return View(Model);
        }

        [HttpPost]
        public IActionResult Index(Event Model)
        {
            EventDAO eventDAO = new EventDAO();
            var added = eventDAO.Add(Model);

            if (!added)
            {
                AgeGroupDAO ageGroupDAO = new AgeGroupDAO();

                ViewBag.ageGroups = ageGroupDAO.ToSelectList(ageGroupDAO.GetAll());
                return View(Model);
            }

            return RedirectToAction("Created");
        }

        [HttpGet]
        public IActionResult Created()
        {
            return View();
        }
    }
}