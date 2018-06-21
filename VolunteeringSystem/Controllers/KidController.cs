using Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using VolunteeringSystem.DAO;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Controllers
{
    [TypeFilter(typeof(IsLoggedAttribute))]
    public class KidController : Controller
    {
        [HttpGet, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult Add()
        {
            return View(new Kid());
        }

        [HttpPost, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult Add(Kid Model)
        {
            if (ModelState.IsValid)
            {
                var _kidDao = new KidDao();
                _kidDao.Add(Model);
                return RedirectToAction("List");
            }

            return View(Model);
        }

        [HttpGet, CheckAccess(new string[] { "ADMIN" })]
        public IActionResult List()
        {
            return View(new KidDao().GetAll());
        }
    }
}