using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface for the local storage service
    /// </summary>
    public interface ILocalStorageService
    {
        /// <summary>
        /// Gets an item from storage
        /// </summary>
        /// <typeparam name="T">Type of the stored item</typeparam>
        /// <param name="key">Storage key</param>
        /// <returns>The stored item or default value if not found</returns>
        Task<T?> GetItemAsync<T>(string key);

        /// <summary>
        /// Sets an item in storage
        /// </summary>
        /// <typeparam name="T">Type of the item to store</typeparam>
        /// <param name="key">Storage key</param>
        /// <param name="value">Value to store</param>
        Task SetItemAsync<T>(string key, T value);

        /// <summary>
        /// Removes an item from storage
        /// </summary>
        /// <param name="key">Storage key</param>
        Task RemoveItemAsync(string key);

        /// <summary>
        /// Clears all items from storage
        /// </summary>
        Task ClearAsync();
    }
}
