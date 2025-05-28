using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LaunchQ.TakeHomeProject.Presentation.ViewModels.Base
{
    public class BaseBooksViewModel
    {
        protected const string DefaultAuthorKey = "OL23919A";
        public const string FavoriteBooksStorageKey = "favorite-books";
        
        public Author? Author { get; set; }
        public bool Loading { get; set; } = true;
        public string? Error { get; set; }
        
        public int ItemsPerPage { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int TotalBooks { get; set; }
        
        public List<BookSummary> PagedBooks { get; set; } = new List<BookSummary>();
        
        public int TotalPages => (int)Math.Ceiling((double)TotalBooks / ItemsPerPage);
            
        protected readonly IAuthorService _authorService = null!;
        protected readonly IBookService _bookService = null!;
        protected string _currentAuthorKey = string.Empty;
        
        public BaseBooksViewModel(IAuthorService authorService, IBookService bookService)
        {
            _authorService = authorService;
            _bookService = bookService;
        }
                
        // Property to store the search query
        public string SearchQuery { get; set; } = string.Empty;
        
        // List to store favorite books by key
        protected List<string> FavoriteBookKeys { get; set; } = new List<string>();
        
        // Method to check if a book is in the favorites list
        public bool IsBookFavorite(string bookKey)
        {
            return FavoriteBookKeys.Contains(bookKey);
        }
    }
}
