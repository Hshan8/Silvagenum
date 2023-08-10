using Microsoft.AspNetCore.Mvc;
using SilvagenumWebApp.Models;

namespace SilvagenumWebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IRepo _personRepo;

        public SearchController (IRepo personRepo)
        {
            _personRepo = personRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Person> result = _personRepo.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Person? result = _personRepo.Get(id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult SearchPeople([FromBody] string searchQuery)
        {
            IEnumerable<Person>? result = null;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                result = _personRepo.Get(searchQuery);
            }
            if (result is not null)
            {
                return new JsonResult(result);
            }
            return NotFound();
        }
    }
}
