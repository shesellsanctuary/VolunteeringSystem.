using System;
using System.Linq;
using Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Domain;
using VolunteeringSystem.Models;
using VolunteeringSystem.Helpers.Email;

namespace VolunteeringSystem.Controllers
{
    [TypeFilter(typeof(IsLoggedAttribute))]
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
        [HttpGet, CheckAccess(new string[] { "VOLUNTEER" })]
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

        [HttpPost, CheckAccess(new string[] { "VOLUNTEER" })]
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

        [HttpGet, CheckAccess(new string[] { "VOLUNTEER" })]
        public IActionResult Denied()
        {
            return View();
        }

        [HttpGet, CheckAccess(new string[] { "VOLUNTEER" })]
        public IActionResult Created()
        {
            return View();
        }

        [HttpGet, CheckAccess(new string[] { "VOLUNTEER" })]
        public IActionResult ListHistory(int id)
        {
            var ageGroups = _ageGroupDao.GetAll();
            var eventList = _eventDao.GetByVolunteer(id);
            foreach (var anEvent in eventList) anEvent.ageGroup = ageGroups.Single(a => a.id == anEvent.ageGroupId);
            ViewBag.Status = "";
            return View(eventList);
        }

        [HttpGet, CheckAccess(new string[] { "VOLUNTEER" })]
        public IActionResult Display(int eventId)
        {
            var anEvent = _eventDao.Get(eventId);
            anEvent.ageGroup = _ageGroupDao.Get(anEvent.ageGroupId);
            anEvent.volunteer = _volunteerDao.Get(anEvent.volunteerId);
            return View(anEvent);
        }

        /* ADMIN ACTIONS */
        [HttpGet, CheckAccess(new string[] { "ADMIN", "PROFESSIONAL" })]
        public IActionResult List(int status)
        {
            var ageGroups = _ageGroupDao.GetAll();
            var eventList = _eventDao.GetByStatus(status);
            foreach (var anEvent in eventList) anEvent.ageGroup = ageGroups.Single(a => a.id == anEvent.ageGroupId);
            ViewBag.Status = ((EventStatus) status).ToPortugueseString() + "s";
            return View(eventList);
        }

        [HttpGet, CheckAccess(new string[] { "ADMIN", "PROFESSIONAL" })]
        public IActionResult Homolog(int eventId)
        {
            var anEvent = _eventDao.Get(eventId);
            anEvent.ageGroup = _ageGroupDao.Get(anEvent.ageGroupId);
            anEvent.volunteer = _volunteerDao.Get(anEvent.volunteerId);
            return View(anEvent);
        }

        [HttpPost, CheckAccess(new string[] { "ADMIN", "PROFESSIONAL" })]
        public IActionResult Homolog(int eventId, int newStatus, string justification, string email, string comentary)
        {
            _eventDao.Homolog(eventId, newStatus, justification, comentary);
            if(newStatus == 1 || newStatus == 2)
            {
                SendMail.SendNewEventStatus(email, newStatus, justification);
            }
            return RedirectToAction("List", new {status = (int) EventStatus.Waiting});
        }
    }
}