using SilvagenumWebApp.Models;
using Microsoft.AspNetCore.Components;

namespace SilvagenumWebApp.Pages.App
{
    public partial class PersonCard
    {
        [Parameter]
        public Person? Person { get; set; }
    }
}
