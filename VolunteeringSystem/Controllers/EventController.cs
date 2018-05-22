using Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace VolunteeringSystem.Controllers
{
    public class EventController : Controller
    {
        private EventDAO eventDAO = new EventDAO();
        private AgeGroupDAO ageGroupDAO = new AgeGroupDAO();
        private VolunteerDAO volunteerDAO = new VolunteerDAO();
        
        /* VOLUNTEER ACTIONS */
        [HttpGet, TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Index()
        {
            var volunteerId = Convert.ToInt32(HttpContext.Session.GetString("volunteerId"));

            Volunteer volunteer = volunteerDAO.Get(volunteerId);

            if(volunteer.status == VolunteerStatus.Waiting || volunteer.status == VolunteerStatus.Blocked)
            {
                // error
                return RedirectToAction("Denied"); //change this later
            }

            
            var Model = new Event();
        
            ViewBag.ageGroups = ageGroupDAO.ToSelectList(ageGroupDAO.GetAll());

            return View(Model);
        }

        [HttpPost, TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Index(Event Model)
        {
            
            Model.institute = "";
            Model.volunteerId = Convert.ToInt32(HttpContext.Session.GetString("volunteerId"));

            var added = eventDAO.Add(Model);

            if (!added)
            {
                ViewBag.ageGroups = ageGroupDAO.ToSelectList(ageGroupDAO.GetAll());
                return View(Model);
            }

            return RedirectToAction("Created");
        }

        [HttpGet, TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Denied() {
            return View();
        }

        [HttpGet, TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Created()
        {
            return View();
        }

        /* ADMIN ACTIONS */
        [HttpGet, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult List(int status)
        {
            var eventList = eventDAO.GetByStatus(status);
            var ageGroups = ageGroupDAO.GetAll();

            foreach (var item in eventList)
            {
                item.ageGroup = ageGroups.Single(a => a.id == item.ageGroupId);
            }

            ViewBag.Status = status == 0 ? "em aprovação" : status == 1 ? "aprovados" : "negados";

            return View(eventList);
        }

        [HttpGet, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int eventId)
        {
            var eventSelected = eventDAO.Get(eventId);
            eventSelected.ageGroup = ageGroupDAO.Get(eventSelected.ageGroupId);
            eventSelected.volunteer = volunteerDAO.Get(eventSelected.volunteerId);


            return View(eventSelected);
        }
        
        [HttpPost, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int eventId, int newStatus, string justification)
        {
            eventDAO.Homolog(eventId, newStatus, justification);

            return RedirectToAction("List", new { status = (int) EventStatus.Waiting });
        }
    }
}
