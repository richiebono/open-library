using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace LaunchQ.TakeHomeProject.Presentation.Blazor.Components.Shared.SearchBox
{
    public partial class SearchBox : ComponentBase
    {
        private string searchText = string.Empty;

        [Parameter]
        public string Placeholder { get; set; } = "Search...";

        [Parameter]
        public string SearchButtonText { get; set; } = "Search";

        [Parameter]
        public string ClearButtonText { get; set; } = "Clear";

        [Parameter]
        public bool ShowClearButton { get; set; } = true;

        [Parameter]
        public string InitialSearchText { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<string> OnSearch { get; set; }

        protected override void OnParametersSet()
        {
            // Inicializa o texto de busca com o valor inicial fornecido
            if (!string.IsNullOrEmpty(InitialSearchText) && string.IsNullOrEmpty(searchText))
            {
                searchText = InitialSearchText;
            }
        }

        private async Task HandleSearch()
        {
            await OnSearch.InvokeAsync(searchText);
        }

        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await HandleSearch();
            }
        }

        private async Task HandleClear()
        {
            searchText = string.Empty;
            await OnSearch.InvokeAsync(string.Empty);
        }
    }
}
