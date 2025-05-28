namespace LaunchQ.TakeHomeProject.Domain.Interfaces.Ports
{
    /// <summary>
    /// Port for interacting with localStorage functionality
    /// </summary>
    public interface ILocalStoragePort
    {
        /// <summary>
        /// Gets an item from storage
        /// </summary>
        Task<T?> GetItemAsync<T>(string key);

        /// <summary>
        /// Sets an item in storage
        /// </summary>
        Task SetItemAsync<T>(string key, T value);

        /// <summary>
        /// Removes an item from storage
        /// </summary>
        Task RemoveItemAsync(string key);

        /// <summary>
        /// Clears all items from storage
        /// </summary>
        Task ClearAsync();
    }
}
