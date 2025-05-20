using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LaunchQ.TakeHomeProject.Presentation.ViewModels
{
    public class BooksViewModel
    {
        private const string DefaultAuthorKey = "OL23919A";
        
        public Author? Author { get; set; }
        public bool Loading { get; set; } = true;
        public string? Error { get; set; }
        
        public int ItemsPerPage { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int TotalBooks { get; set; }
        
        public List<BookSummary> PagedBooks { get; private set; } = new List<BookSummary>();
        
        public int TotalPages => (int)Math.Ceiling((double)TotalBooks / ItemsPerPage);
            
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private string _currentAuthorKey = string.Empty;
        
        public BooksViewModel(IAuthorService authorService, IBookService bookService)
        {
            _authorService = authorService;
            _bookService = bookService;
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
        
        // Propriedade para armazenar a consulta de busca
        public string SearchQuery { get; set; } = string.Empty;
        
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
