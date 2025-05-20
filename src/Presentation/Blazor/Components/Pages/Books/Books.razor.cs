using LaunchQ.TakeHomeProject.Presentation.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace LaunchQ.TakeHomeProject.Presentation.Blazor.Components.Pages.Books
{
    public partial class Books : ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;
        
        [Inject]
        private BooksViewModel ViewModel { get; set; } = default!;
        
        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadAuthorAsync();
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender || ViewModel.Author != null)
            {
                await UpdatePageTitle();
            }
        }
        
        private async Task UpdatePageTitle()
        {
            await JSRuntime.InvokeVoidAsync("eval", "document.title = 'Books'");
        }
        
        private async Task OnPageChanged(int page)
        {
            await ViewModel.OnPageChanged(page);
            StateHasChanged();
        }
        
        private async Task OnItemsPerPageChanged(int itemsPerPage)
        {
            await ViewModel.OnItemsPerPageChanged(itemsPerPage);
            StateHasChanged();
        }
        
        private async Task HandleSearch(string searchText)
        {
            await ViewModel.OnSearchQueryChanged(searchText);
            StateHasChanged();
        }
    }
}
