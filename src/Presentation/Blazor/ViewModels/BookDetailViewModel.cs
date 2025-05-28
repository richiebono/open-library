using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using System.Threading.Tasks;
using LaunchQ.TakeHomeProject.Presentation.ViewModels.Base;

namespace LaunchQ.TakeHomeProject.Presentation.ViewModels
{
    public class BookDetailViewModel
    {
        private readonly IBookService _bookService;
        private readonly ILocalStorageService _localStorageService;
        
        public BookDetailViewModel(IBookService bookService, ILocalStorageService localStorageService)
        {
            _bookService = bookService;
            _localStorageService = localStorageService;
        }
        
        public string BookKey { get; set; } = string.Empty;
        public Book? Book { get; set; }
        public bool Loading { get; set; } = true;
        public string? Error { get; set; }
        public bool IsFavorite { get; set; } = false;
        
        public async Task LoadBookAsync()
        {
            try
            {
                Loading = true;
                Error = null;
                
                Book = await _bookService.GetBookByKeyAsync(BookKey);
                
                if (Book == null)
                {
                    Error = "Failed to load book information.";
                    return;
                }
                
                // Check if the book is in favorites
                await CheckIfBookIsFavorite();
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
        
        private async Task CheckIfBookIsFavorite()
        {
            if (Book == null) return;
            
            var favoriteKeys = await _localStorageService.GetItemAsync<List<string>>(BaseBooksViewModel.FavoriteBooksStorageKey) 
                ?? new List<string>();
            
            string bookKey = Book.Key.Replace("/works/", "");
            IsFavorite = favoriteKeys.Contains(bookKey);
        }
        
        public async Task ToggleFavoriteStatusAsync()
        {
            if (Book == null) return;
            
            var favoriteKeys = await _localStorageService.GetItemAsync<List<string>>(BaseBooksViewModel.FavoriteBooksStorageKey) 
                ?? new List<string>();
            
            string bookKey = Book.Key.Replace("/works/", "");
            
            if (IsFavorite)
            {
                favoriteKeys.Remove(bookKey);
            }
            else
            {
                favoriteKeys.Add(bookKey);
            }
            
            await _localStorageService.SetItemAsync(BaseBooksViewModel.FavoriteBooksStorageKey, favoriteKeys);
            IsFavorite = !IsFavorite;
        }
    }
}
