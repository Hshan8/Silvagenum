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

        private readonly int pageSize = 6;            //temporary

        public IActionResult Index(int? pageNumber)
        {
            pageNumber ??= 1;
            PagedListViewModel<Person> list = new(
                _personRepo.GetAll(pageNumber, pageSize), 
                _personRepo.GetCount(), 
                pageNumber.Value, 
                pageSize,
                "List of all the people in the database");
            return View(list);
        }

        public IActionResult Details(int id)
        {
            Person? person = _personRepo.Get(id);
            if (person == null)
            {
                return NotFound();
            }
            person.Father = _personRepo.Get(person.FatherId ?? 0);
            person.Mother = _personRepo.Get(person.MotherId ?? 0);
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

        #region Edit & relations
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

        public IActionResult EditRelation(int childId, int parentId, int type)
        {
            Person? child = _personRepo.Get(childId);
            Person? parent = _personRepo.Get(parentId);
            Gender gender = Gender.male;
            int backId = 0;

            try
            {
                if (child is null)
                    throw new ArgumentNullException(nameof(childId));

                switch (type)
                {
                    case 0:                     //link/unlink mother
                        gender = Gender.female;
                        backId = childId;
                        break;
                    case 1:                     //link/unlink father
                        backId = childId;
                        break;
                    case 2:                     //link child
                        if (parent is null)
                            throw new ArgumentNullException(nameof(parentId));
                        gender = parent.Gender;
                        backId = parentId;
                        break;
                    case 3:                     //unlink child
                        if (parent is null)
                            throw new ArgumentNullException(nameof(parentId));
                        gender = parent.Gender;
                        backId = parentId;
                        parent = null;
                        break;
                    default:
                        break;
                }

                _personRepo.SetRelation(child, parent, gender);
            }
            catch (ArgumentNullException ex)
            {
                TempData["Error"] = $"Updating the person failed, please try again. The person of the given {ex.ParamName} could not be found.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Updating the person failed, please try again. Unexpected error occurred: {ex.Message}";
            }

            if (backId == 0)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Details), new { id = backId });
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
