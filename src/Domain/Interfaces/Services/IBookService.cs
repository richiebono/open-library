using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface for the book service
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Gets book by key
        /// </summary>
        /// <param name="key">Book key</param>
        /// <returns>Book or null if not found</returns>
        Task<Book?> GetBookByKeyAsync(string key);
        
        /// <summary>
        /// Gets books by author key
        /// </summary>
        /// <param name="authorKey">Author key</param>
        /// <returns>List of book summaries</returns>
        Task<List<BookSummary>> GetBooksByAuthorAsync(string authorKey);
        
        /// <summary>
        /// Gets paginated books by author
        /// </summary>
        /// <param name="authorKey">Author key</param>
        /// <param name="page">Current page (1-based)</param>
        /// <param name="itemsPerPage">Items per page</param>
        /// <returns>Tuple with book list and total count</returns>
        Task<(List<BookSummary> Books, int TotalCount)> GetPaginatedBooksByAuthorAsync(string authorKey, int page, int itemsPerPage);
    }
}
