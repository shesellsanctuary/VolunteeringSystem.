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

            // Cria uma lista de opções que serão usadas no <select> de instituição
            var institutes = new List<SelectListItem>();
            institutes.Add(new SelectListItem { Text = "Instituição #1", Value = "1" });
            institutes.Add(new SelectListItem { Text = "Instituição #2", Value = "2" });

            // ViewBag é um dicionário de dados, no qual podemos mandar dados para dentro da View
            ViewBag.instituteList = institutes;
            ViewBag.ageGroups = ageGroupDAO.ToSelectList(ageGroupDAO.GetAll());

            return View(Model);
        }

        [HttpPost]
        public IActionResult Index(Event Model)
        {
            EventDAO eventDAO = new EventDAO();
            var added = eventDAO.Add(Model);

            if (!added)
                return View(Model);

            return Redirect("Home");
        }
    }
}