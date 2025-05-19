using LaunchQ.TakeHomeProject.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LaunchQ.TakeHomeProject.Presentation.Blazor.Components.Shared.AuthorCard
{
    public partial class AuthorCard : ComponentBase
    {
        [Parameter]
        public Author? Author { get; set; }
    }
}
