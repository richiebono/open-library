using System.ComponentModel;
using LaunchQ.TakeHomeProject.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LaunchQ.TakeHomeProject.Presentation.Blazor.Components.Shared.Pagination
{
    public partial class Pagination : ComponentBase
    {
        [Parameter]
        public List<BookSummary>? Books { get; set; }
        
        [Parameter]
        public int TotalBooks { get; set; }
        
        [Parameter]
        public int CurrentPage { get; set; } = 1;
        
        [Parameter]
        public int ItemsPerPage { get; set; } = 10;
        
        [Parameter]
        public EventCallback<int> OnPageChangedCallback { get; set; }
        
        [Parameter]
        public EventCallback<int> OnItemsPerPageChangedCallback { get; set; }
        
        public int[] ItemsPerPageOptions { get; set; } = { 5, 10, 25, 50 };
        
        public int MaxPagesToShow { get; set; } = 5;
        
        public int TotalPages => (int)Math.Ceiling((double)TotalBooks / ItemsPerPage);
        
        private async Task OnPageChanged(int page)
        {
            if (page < 1 || page > TotalPages || page == CurrentPage)
                return;
                
            CurrentPage = page;
            await OnPageChangedCallback.InvokeAsync(page);
        }
        
        private async Task OnItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var value))
            {
                ItemsPerPage = value;
                await OnItemsPerPageChangedCallback.InvokeAsync(value);
            }
        }
    }
}
