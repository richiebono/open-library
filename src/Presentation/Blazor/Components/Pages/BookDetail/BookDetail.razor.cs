using LaunchQ.TakeHomeProject.Presentation.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace LaunchQ.TakeHomeProject.Presentation.Blazor.Components.Pages.BookDetail
{
    public partial class BookDetail : ComponentBase
    {
        [Parameter]
        public string BookKey { get; set; } = string.Empty;
        
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;
        
        [Inject]
        private BookDetailViewModel ViewModel { get; set; } = default!;
        
        protected override void OnInitialized()
        {
            ViewModel.BookKey = BookKey;
        }
        
        protected override async Task OnInitializedAsync()
        {
            await LoadBookAsync();
        }
        
        protected override async Task OnParametersSetAsync()
        {
            ViewModel.BookKey = BookKey;
            await LoadBookAsync();
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if ((firstRender || ViewModel.Book != null) && !ViewModel.Loading)
            {
                await UpdatePageTitle();
            }
        }

        private async Task LoadBookAsync()
        {
            await ViewModel.LoadBookAsync();
        }

        private async Task UpdatePageTitle()
        {
            if (ViewModel?.Book != null)
            {
                string escapedTitle = ViewModel.Book.Title.Replace("'", "\\'");
                await JSRuntime.InvokeVoidAsync("eval", $"document.title = '{escapedTitle} - Book Details'");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("eval", "document.title = 'Book Details'");
            }
        }
    }
}
