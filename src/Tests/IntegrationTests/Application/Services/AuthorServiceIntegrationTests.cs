using LaunchQ.TakeHomeProject.Infrastructure.Configuration;
using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Application.Mappers;
using LaunchQ.TakeHomeProject.Application.Services;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Infrastructure.Adapters;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.IntegrationTests.Application.Services
{
    // Observação: Testes de integração requerem acesso à API externa OpenLibrary
    // Isso significa que eles podem falhar se a API estiver indisponível ou se houver problemas de rede
    // Pode ser desejável pular estes testes em ambientes CI usando [Trait("Category", "Integration")]
    [Trait("Category", "Integration")]
    public class AuthorServiceIntegrationTests
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly AuthorMapper _authorMapper;
        private readonly WorksMapper _worksMapper;
        private readonly OpenLibraryAuthorAdapter _authorAdapter;
        private readonly AuthorService _authorService;

        public AuthorServiceIntegrationTests()
        {
            _httpClient = new HttpClient();
            _apiSettings = new ApiSettings
            {
                OpenLibrary = new OpenLibrarySettings
                {
                    BaseUrl = "https://openlibrary.org"
                }
            };
            
            var options = Options.Create(_apiSettings);
            _authorMapper = new AuthorMapper();
            _worksMapper = new WorksMapper();
            // Create a mock BookPort for the OpenLibraryAuthorAdapter dependency
            var bookAdapter = new OpenLibraryBookAdapter(_httpClient, options, new BookMapper(), _worksMapper);
            _authorAdapter = new OpenLibraryAuthorAdapter(_httpClient, bookAdapter, options, _authorMapper);
            _authorService = new AuthorService(_authorAdapter);
        }

        [Fact]
        public async Task GetAuthorByKeyAsync_WithValidAuthorKey_ShouldReturnAuthor()
        {
            // Arrange
            // Isaac Asimov's key
            var authorKey = "OL34221A";

            // Act
            var result = await _authorService.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().EndWith(authorKey); // API retorna a chave com prefixo '/authors/'
            result.Name.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAuthorByKeyAsync_WithValidAuthorKey_ShouldReturnAuthorWithBooks()
        {
            // Arrange
            // Isaac Asimov's key
            var authorKey = "OL34221A";

            // Act
            var result = await _authorService.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Books.Should().NotBeNull();
            result.Books.Count.Should().BeGreaterThan(0);
        }
    }
}
