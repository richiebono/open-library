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
    /// Implementation of book port using OpenLibrary API adapter
    /// </summary>
    public class OpenLibraryBookAdapter : IBookPort
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _baseUrl;
        private readonly IMapper<BookResponseDto, Book> _bookMapper;
        private readonly IMapper<WorksResponseDto, List<BookSummary>> _worksMapper;

        public OpenLibraryBookAdapter(
            HttpClient httpClient, 
            IOptions<ApiSettings> apiSettings,
            IMapper<BookResponseDto, Book> bookMapper,
            IMapper<WorksResponseDto, List<BookSummary>> worksMapper)
        {
            _httpClient = httpClient;
            _baseUrl = apiSettings.Value.OpenLibrary.BaseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _bookMapper = bookMapper;
            _worksMapper = worksMapper;
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new DescriptionJsonConverter() }
            };
        }

        /// <inheritdoc />
        public async Task<Book?> GetBookByKeyAsync(string key)
        {
            try
            {
                if (!key.StartsWith("/works/"))
                {
                    key = $"/works/{key}";
                }
                
                var response = await _httpClient.GetStringAsync($"{key}.json");
                Console.WriteLine($"JSON recebido para o livro: {key}");
                var bookResponse = JsonSerializer.Deserialize<BookResponseDto>(response, _jsonOptions);
                
                if (bookResponse == null)
                    return null;

                return _bookMapper.Map(bookResponse);
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Erro ao deserializar JSON do livro: {jsonEx.Message}");
                if (jsonEx.InnerException != null)
                {
                    Console.WriteLine($"Erro interno: {jsonEx.InnerException.Message}");
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter o livro: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Erro interno: {ex.InnerException.Message}");
                }
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<List<BookSummary>> GetBooksByAuthorAsync(string authorKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authorKey))
                    return new List<BookSummary>();
                    
                if (!authorKey.StartsWith("/authors/"))
                {
                    authorKey = $"/authors/{authorKey}";
                }
                
                var response = await _httpClient.GetFromJsonAsync<WorksResponseDto>($"{authorKey}/works.json", _jsonOptions);
                
                if (response == null)
                    return new List<BookSummary>();
                    
                return _worksMapper.Map(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting books by author: {ex.Message}");
                return new List<BookSummary>();
            }
        }
        
        /// <inheritdoc />
        public async Task<(List<BookSummary> Books, int TotalCount)> GetPaginatedBooksByAuthorAsync(string authorKey, int limit, int offset)
        {
            try
            {
                if (!authorKey.StartsWith("/authors/"))
                {
                    authorKey = $"/authors/{authorKey}";
                }
                
                var response = await _httpClient.GetFromJsonAsync<WorksResponseDto>($"{authorKey}/works.json?limit={limit}&offset={offset}", _jsonOptions);
                
                if (response == null)
                    return (new List<BookSummary>(), 0);
                    
                var books = _worksMapper.Map(response);
                int totalCount = response.Size ?? 0;
                
                return (books, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting paginated books by author: {ex.Message}");
                return (new List<BookSummary>(), 0);
            }
        }
    }
}
