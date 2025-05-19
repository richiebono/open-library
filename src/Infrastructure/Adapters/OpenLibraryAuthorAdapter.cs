using System.Text.Json;
using System.Net.Http.Json;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Infrastructure.Configuration;
using LaunchQ.TakeHomeProject.Application.Mappers;
using Microsoft.Extensions.Options;

namespace LaunchQ.TakeHomeProject.Infrastructure.Adapters
{
    /// <summary>
    /// Implementation of author port using OpenLibrary API adapter
    /// </summary>
    public class OpenLibraryAuthorAdapter : IAuthorPort
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IBookPort _bookPort;
        private readonly string _baseUrl;
        private readonly IMapper<AuthorResponseDto, Author> _authorMapper;

        public OpenLibraryAuthorAdapter(HttpClient httpClient, IBookPort bookPort, IOptions<ApiSettings> apiSettings, IMapper<AuthorResponseDto, Author> authorMapper)
        {
            _httpClient = httpClient;
            _bookPort = bookPort;
            _baseUrl = apiSettings.Value.OpenLibrary.BaseUrl;
            _authorMapper = authorMapper;
            
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<Author?> GetAuthorByKeyAsync(string key)
        {
            try
            {
                if (!key.StartsWith("/authors/"))
                {
                    key = $"/authors/{key}";
                }
                
                var response = await _httpClient.GetFromJsonAsync<AuthorResponseDto>($"{key}.json", _jsonOptions);
                
                if (response == null)
                    return null;

                var author = _authorMapper.Map(response);
                author.Books = await _bookPort.GetBooksByAuthorAsync(key);
                
                return author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting author: {ex.Message}");
                return null;
            }
        }
    }
}
