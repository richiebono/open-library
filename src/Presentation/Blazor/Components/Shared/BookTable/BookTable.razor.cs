using LaunchQ.TakeHomeProject.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LaunchQ.TakeHomeProject.Presentation.Blazor.Components.Shared.BookTable
{
    public partial class BookTable : ComponentBase
    {
        [Parameter]
        public List<BookSummary>? Books { get; set; }

        [Parameter]
        public EventCallback<string> OnBookSelected { get; set; }

        protected override void OnInitialized()
        {
            if (Books == null)
            {
                Books = new List<BookSummary>();
            }
        }
    }
}
