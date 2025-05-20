using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Application.Services
{
    /// <summary>
    /// Implementation of book service
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookPort _bookPort;
        
        public BookService(IBookPort bookPort)
        {
            _bookPort = bookPort;
        }
        
        public async Task<Book?> GetBookByKeyAsync(string key)
        {
            return await _bookPort.GetBookByKeyAsync(key);
        }
        
    public async Task<List<BookSummary>> GetBooksByAuthorAsync(string authorKey)
    {
        if (string.IsNullOrWhiteSpace(authorKey))
            return new List<BookSummary>();
            
        var books = await _bookPort.GetBooksByAuthorAsync(authorKey);
        return books ?? new List<BookSummary>();
    }
        
        public async Task<(List<BookSummary> Books, int TotalCount)> GetPaginatedBooksByAuthorAsync(string authorKey, int page, int itemsPerPage, string? searchQuery = null)
        {
            if (string.IsNullOrWhiteSpace(authorKey))
                return (new List<BookSummary>(), 0);
            
            // Convert 1-based page to 0-based offset
            int offset = (page - 1) * itemsPerPage;
            
            return await _bookPort.GetPaginatedBooksByAuthorAsync(authorKey, itemsPerPage, offset, searchQuery);
        }
    }
}
