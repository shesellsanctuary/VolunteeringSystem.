using System;
using System.Linq;
using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Domain;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly AgeGroupDao _ageGroupDao = new AgeGroupDao();
        private readonly EventDao _eventDao = new EventDao();
        private readonly ILogger<EventController> _logger;
        private readonly VolunteerDao _volunteerDao = new VolunteerDao();

        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        /* VOLUNTEER ACTIONS */
        [HttpGet]
        [TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Index()
        {
            var volunteerId = Convert.ToInt32(HttpContext.Session.GetString("volunteerId"));
            var volunteer = _volunteerDao.Get(volunteerId);
            if (volunteer.status == VolunteerStatus.Waiting || volunteer.status == VolunteerStatus.Blocked)
                return RedirectToAction("Denied");
            var model = new Event();
            ViewBag.ageGroups = _ageGroupDao.ToSelectList(_ageGroupDao.GetAll());
            return View(model);
        }

        [HttpPost]
        [TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Index(Event Model)
        {
            Model.institute = "";
            Model.volunteerId = Convert.ToInt32(HttpContext.Session.GetString("volunteerId"));
            var ageGroup = _ageGroupDao.Get(int.Parse(Model.ageGroup.label));
            Model.ageGroup = ageGroup;
            _logger.LogInformation("Creating an Event for the age group " + Model.ageGroup + ".");
            _logger.LogInformation("Creating an Event for the volunteer " + Model.volunteerId + ".");
            var added = _eventDao.Add(Model);
            if (added) return RedirectToAction("Created");
            ViewBag.ageGroups = _ageGroupDao.ToSelectList(_ageGroupDao.GetAll());
            return View(Model);
        }

        [HttpGet]
        [TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Denied()
        {
            return View();
        }

        [HttpGet]
        [TypeFilter(typeof(IsLoggedVolunteerAttribute))]
        public IActionResult Created()
        {
            return View();
        }

        /* ADMIN ACTIONS */
        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult List(int status)
        {
            var ageGroups = _ageGroupDao.GetAll();
            var eventList = _eventDao.GetByStatus(status);
            foreach (var anEvent in eventList) anEvent.ageGroup = ageGroups.Single(a => a.id == anEvent.ageGroupId);
            ViewBag.Status = ((EventStatus) status).ToPortugueseString() + "s";
            return View(eventList);
        }

        [HttpGet]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int eventId)
        {
            var anEvent = _eventDao.Get(eventId);
            anEvent.ageGroup = _ageGroupDao.Get(anEvent.ageGroupId);
            anEvent.volunteer = _volunteerDao.Get(anEvent.volunteerId);
            return View(anEvent);
        }

        [HttpPost]
        [TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult Homolog(int eventId, int newStatus, string justification)
        {
            _eventDao.Homolog(eventId, newStatus, justification);
            return RedirectToAction("List", new {status = (int) EventStatus.Waiting});
        }
    }
}