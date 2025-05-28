using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using LaunchQ.TakeHomeProject.Presentation.ViewModels.Base;

namespace LaunchQ.TakeHomeProject.Presentation.ViewModels
{
    public class FavoreiteBooksViewModel : BaseBooksViewModel
    {
        
        public FavoreiteBooksViewModel(IAuthorService authorService, IBookService bookService): base(authorService, bookService)
        {        
        }
        
       public async Task LoadAuthorAsync()
        {
            try
            {
                Loading = true;
                Error = null;

                Author = await _authorService.GetAuthorByKeyAsync(DefaultAuthorKey);

                if (Author == null)
                {
                    Error = "Failed to load author information.";
                    return;
                }

                _currentAuthorKey = DefaultAuthorKey;
                await LoadPaginatedBooksAsync();
            }
            catch (Exception ex)
            {
                Error = $"An error occurred: {ex.Message}";
            }
            finally
            {
                Loading = false;
            }
        }


        private async Task LoadPaginatedBooksAsync()
        {
            if (string.IsNullOrEmpty(_currentAuthorKey))
                return;

            var result = await _bookService.GetPaginatedBooksByAuthorAsync(
                _currentAuthorKey,
                CurrentPage,
                ItemsPerPage,
                !string.IsNullOrWhiteSpace(SearchQuery) ? SearchQuery : null);

            PagedBooks = result.Books;
            TotalBooks = result.TotalCount;
        }
       
        public async Task OnPageChanged(int page)
        {
            CurrentPage = page;
            await LoadPaginatedBooksAsync();
        }
        
        public async Task OnItemsPerPageChanged(int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
            CurrentPage = 1;
            await LoadPaginatedBooksAsync();
        }
        
        public async Task OnSearchQueryChanged(string searchQuery)
        {
            SearchQuery = searchQuery;
            CurrentPage = 1; // Resetar para a primeira página ao buscar
            await LoadPaginatedBooksAsync();
        }
    }
}
