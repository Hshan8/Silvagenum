using SilvagenumWebApp.Models;
using Microsoft.AspNetCore.Components;

namespace SilvagenumWebApp.Pages.App
{
    public partial class SearchBlazor
    {
        public string SearchText = "";
        public List<Person> FilteredPeople { get; set; } = new List<Person>();

        [Inject]
        public IRepo? Repo { get; set; }

        private void Search()
        {
            FilteredPeople.Clear();
            if (Repo is not null)
            {
                if (SearchText.Length >= 3)
                    FilteredPeople = Repo.Get(SearchText).ToList();
            }
        }
    }
}
