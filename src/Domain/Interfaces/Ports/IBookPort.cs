using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Domain.Interfaces.Ports
{
    /// <summary>
    /// Port for book operations to external systems
    /// </summary>
    public interface IBookPort
    {
        /// <summary>
        /// Gets books by author without pagination
        /// </summary>
        Task<List<BookSummary>> GetBooksByAuthorAsync(string authorKey);
        
        /// <summary>
        /// Gets paginated books by author
        /// </summary>
        /// <param name="authorKey">The author key</param>
        /// <param name="limit">The number of items per page</param>
        /// <param name="offset">The offset for pagination</param>
        /// <returns>Paginated books and total count</returns>
        Task<(List<BookSummary> Books, int TotalCount)> GetPaginatedBooksByAuthorAsync(string authorKey, int limit, int offset);
        
        /// <summary>
        /// Gets a book by its key
        /// </summary>
        Task<Book?> GetBookByKeyAsync(string key);
    }
}
