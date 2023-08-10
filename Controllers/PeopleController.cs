using Microsoft.AspNetCore.Mvc;
using SilvagenumWebApp.Models;
using SilvagenumWebApp.ViewModels;

namespace SilvagenumWebApp.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IRepo _personRepo;

        public PeopleController(IRepo personRepo)
        {
            _personRepo = personRepo;
        }

        public IActionResult Index() => View(new PersonListViewModel(_personRepo.GetAll(), "List of all people in the repo"));

        public IActionResult Details(int id)
        {
            Person? person = _personRepo.Get(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        public IActionResult Search() => View();

        #region Create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                _personRepo.Add(person);
                TempData["Success"] = "You have successfully added a new person to the database.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Message = "A valid person object could not be created.";
            return View(person);
        }
        #endregion

        #region Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person? selectedPerson = _personRepo.Get(id.Value);
            return View(selectedPerson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _personRepo.Update(person);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the person failed, please try again. Error: {ex.Message}");
            }
            return View(person);
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Person person)
        {
            try
            {
                _personRepo.Delete(person);
                TempData["Success"] = "Person deleted succesfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Deleting the person failed, please try again. Error: {ex.Message}";
            }
            
            return RedirectToAction(nameof(Index));         //catch concurrency exception; if none return to the person view
        }
        #endregion
    }
}
