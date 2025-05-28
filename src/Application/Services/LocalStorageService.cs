using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;

namespace LaunchQ.TakeHomeProject.Application.Services
{
    /// <summary>
    /// Implementation of the local storage service that uses ILocalStoragePort
    /// </summary>
    public class LocalStorageService : ILocalStorageService
    {
        private readonly ILocalStoragePort _localStoragePort;

        public LocalStorageService(ILocalStoragePort localStoragePort)
        {
            _localStoragePort = localStoragePort;
        }

        /// <summary>
        /// Gets an item from storage
        /// </summary>
        public async Task<T?> GetItemAsync<T>(string key)
        {
            return await _localStoragePort.GetItemAsync<T>(key);
        }

        /// <summary>
        /// Sets an item in storage
        /// </summary>
        public async Task SetItemAsync<T>(string key, T value)
        {
            await _localStoragePort.SetItemAsync(key, value);
        }

        /// <summary>
        /// Removes an item from storage
        /// </summary>
        public async Task RemoveItemAsync(string key)
        {
            await _localStoragePort.RemoveItemAsync(key);
        }

        /// <summary>
        /// Clears all items from storage
        /// </summary>
        public async Task ClearAsync()
        {
            await _localStoragePort.ClearAsync();
        }
    }
}
