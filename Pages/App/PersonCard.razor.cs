using SilvagenumWebApp.Models;
using Microsoft.AspNetCore.Components;

namespace SilvagenumWebApp.Pages.App
{
    public partial class PersonCard
    {
        [Parameter]
        public Person? Person { get; set; }
        [Parameter]
        public string? SelectionType { get; set; }
        [Parameter]
        public int? RelatedId { get; set; }

        protected override void OnParametersSet()
        {
            SelectionType = null;
            RelatedId = null;
        }
    }
}
