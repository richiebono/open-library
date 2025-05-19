using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface for the author service
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Gets author by key
        /// </summary>
        /// <param name="key">Author key</param>
        /// <returns>Author or null if not found</returns>
        Task<Author?> GetAuthorByKeyAsync(string key);
    }
}
