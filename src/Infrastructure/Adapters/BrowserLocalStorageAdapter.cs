using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using Microsoft.JSInterop;
using System.Text.Json;

namespace LaunchQ.TakeHomeProject.Infrastructure.Adapters
{
    /// <summary>
    /// Implementation of ILocalStoragePort that uses browser's localStorage
    /// </summary>
    public class BrowserLocalStorageAdapter : ILocalStoragePort
    {
        private readonly IJSRuntime _jsRuntime;

        public BrowserLocalStorageAdapter(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Gets an item from localStorage
        /// </summary>
        public async Task<T?> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            
            if (string.IsNullOrEmpty(json))
                return default;
                
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Sets an item in localStorage
        /// </summary>
        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        /// <summary>
        /// Removes an item from localStorage
        /// </summary>
        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        /// <summary>
        /// Clears all items from localStorage
        /// </summary>
        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.clear");
        }
    }
}
