using Microsoft.AspNetCore.Components;
using SilvagenumWebApp.Models;

namespace SilvagenumWebApp.Pages.App
{
    public partial class SearchBlazor
    {
        [Parameter]
        public string? SelectionType { get; set; }
        [Parameter]
        public int? RelatedId { get; set; }

        public string SearchText = "";
        public List<Person> FilteredPeople { get; set; } = new List<Person>();

        [Inject]
        public IRepo? Repo { get; set; }

        protected override void OnParametersSet()
        {
            SelectionType = null;
            RelatedId = null;
        }

        private void Search()
        {
            FilteredPeople.Clear();
            if (Repo is not null)
            {
                if (SearchText.Length >= 3)
                {
                    FilteredPeople = Repo.Get(SearchText)?.ToList() ?? FilteredPeople;
                }
                /*if (SelectionType == "mother")
                {
                    FilteredPeople = FilteredPeople.Where(p => p.Gender == Gender.female).ToList();
                }
                if (SelectionType == "father")
                {
                    FilteredPeople = FilteredPeople.Where(p => p.Gender == Gender.male).ToList();
                }*/
            }
        }
    }
}
