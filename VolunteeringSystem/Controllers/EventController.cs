using Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    public class EventController : Controller
    {
        private EventDAO eventDAO = new EventDAO();
        private AgeGroupDAO ageGroupDAO = new AgeGroupDAO();
        private VolunteerDAO volunteerDAO = new VolunteerDAO();


        [HttpGet]
        public IActionResult Index()
        {
            var Model = new Event();
        
            ViewBag.ageGroups = ageGroupDAO.ToSelectList(ageGroupDAO.GetAll());

            return View(Model);
        }

        [HttpPost]
        public IActionResult Index(Event Model)
        {
            Model.institute = "";
            Model.volunteerId = 2018;

            var added = eventDAO.Add(Model);

            if (!added)
            {
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

        [HttpGet, TypeFilter(typeof(IsLoggedAdminAttribute))]
        public IActionResult List(int status)
        {
            var eventList = eventDAO.GetByStatus(status);
            var ageGroups = ageGroupDAO.GetAll();

            foreach (var item in eventList)
            {
                item.ageGroup = ageGroups.Single(a => a.id == item.ageGroupId);
            }
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
