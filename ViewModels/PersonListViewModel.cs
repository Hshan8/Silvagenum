using SilvagenumWebApp.Models;

namespace SilvagenumWebApp.ViewModels
{
    public class PersonListViewModel
    {
        public IEnumerable<Person> People { get; }
        public string? CurrentTree { get; }

        public PersonListViewModel(IEnumerable<Person> people, string? currentTree)
        {
            People = people;
            CurrentTree = currentTree;
        }
    }
}
