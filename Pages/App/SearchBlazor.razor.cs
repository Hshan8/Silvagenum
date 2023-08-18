using SilvagenumWebApp.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.Design;

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
            SelectionType ??= "";
            RelatedId ??= 0;
        }

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
