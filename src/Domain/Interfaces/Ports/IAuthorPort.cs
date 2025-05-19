using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Domain.Interfaces.Ports
{
    /// <summary>
    /// Port for author operations to external systems
    /// </summary>
    public interface IAuthorPort
    {
        /// <summary>
        /// Gets an author by key including their books
        /// </summary>
        /// <param name="key">The author key</param>
        /// <returns>The author or null if not found</returns>
        Task<Author?> GetAuthorByKeyAsync(string key);
    }
}
