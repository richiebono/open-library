using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Application.Services
{
    /// <summary>
    /// Implementation of author service
    /// </summary>
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorPort _authorPort;
        
        public AuthorService(IAuthorPort authorPort)
        {
            _authorPort = authorPort;
        }
        
        public async Task<Author?> GetAuthorByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
                
            return await _authorPort.GetAuthorByKeyAsync(key);
        }
    }
}
