using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using LaunchQ.TakeHomeProject.Presentation.ViewModels.Base;
using System.Text.Json;

namespace LaunchQ.TakeHomeProject.Presentation.ViewModels
{
    public class FavoriteBooksViewModel : BaseBooksViewModel
    {
        private readonly ILocalStorageService _localStorageService;
        
        public FavoriteBooksViewModel(
            IAuthorService authorService, 
            IBookService bookService,
            ILocalStorageService localStorageService) : base(authorService, bookService)
        {
            _localStorageService = localStorageService;
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
                await LoadFavoriteBooksAsync();
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

        // Load books from local storage
        private async Task LoadFavoriteBooksAsync()
        {
            try
            {
                // Get the list of favorite book keys from local storage
                FavoriteBookKeys = await _localStorageService.GetItemAsync<List<string>>(FavoriteBooksStorageKey) ?? new List<string>();
                
                if (!FavoriteBookKeys.Any())
                {
                    PagedBooks = new List<BookSummary>();
                    TotalBooks = 0;
                    return;
                }
                
                // Load all books by the author
                var allBooks = await _bookService.GetBooksByAuthorAsync(_currentAuthorKey);
                
                // Filter to only include favorites
                var favoriteBooks = allBooks.Where(b => FavoriteBookKeys.Contains(b.Key.Replace("/works/", ""))).ToList();
                
                // Apply pagination and search filtering
                var filteredBooks = !string.IsNullOrWhiteSpace(SearchQuery) 
                    ? favoriteBooks.Where(b => b.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList() 
                    : favoriteBooks;
                
                TotalBooks = filteredBooks.Count;
                
                // Apply pagination
                int startIndex = (CurrentPage - 1) * ItemsPerPage;
                PagedBooks = filteredBooks
                    .Skip(startIndex)
                    .Take(ItemsPerPage)
                    .ToList();
            }
            catch (Exception ex)
            {
                Error = $"Failed to load favorite books: {ex.Message}";
                PagedBooks = new List<BookSummary>();
                TotalBooks = 0;
            }
        }
        
        public async Task AddToFavorites(BookSummary book)
        {
            string bookKey = book.GetFormattedKey();
            
            if (!FavoriteBookKeys.Contains(bookKey))
            {
                FavoriteBookKeys.Add(bookKey);
                await SaveFavoritesToLocalStorage();
                await LoadFavoriteBooksAsync();
            }
        }
        
        public async Task RemoveFromFavorites(string bookKey)
        {
            bookKey = bookKey.Replace("/works/", "");
            
            if (FavoriteBookKeys.Contains(bookKey))
            {
                FavoriteBookKeys.Remove(bookKey);
                await SaveFavoritesToLocalStorage();
                await LoadFavoriteBooksAsync();
            }
        }
        
        private async Task SaveFavoritesToLocalStorage()
        {
            await _localStorageService.SetItemAsync(FavoriteBooksStorageKey, FavoriteBookKeys);
        }
       
        public async Task OnPageChanged(int page)
        {
            CurrentPage = page;
            await LoadFavoriteBooksAsync();
        }
        
        public async Task OnItemsPerPageChanged(int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
            CurrentPage = 1;
            await LoadFavoriteBooksAsync();
        }
        
        public async Task OnSearchQueryChanged(string searchQuery)
        {
            SearchQuery = searchQuery;
            CurrentPage = 1;
            await LoadFavoriteBooksAsync();
        }
    }
}
