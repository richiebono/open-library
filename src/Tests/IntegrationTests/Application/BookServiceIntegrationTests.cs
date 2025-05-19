using LaunchQ.TakeHomeProject.Infrastructure.Configuration;
using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Application.Mappers;
using LaunchQ.TakeHomeProject.Application.Services;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Infrastructure.Adapters;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace LaunchQ.TakeHomeProject.IntegrationTests.Application
{
    [Trait("Category", "Integration")]
    public class BookServiceIntegrationTests : IAsyncLifetime
    {
        private ServiceProvider _serviceProvider = null!;
        private BookService _bookService = null!;

        public async Task InitializeAsync()
        {
            var services = new ServiceCollection();
            
            // Configure services
            services.AddHttpClient();
            
            // Configure settings
            services.Configure<ApiSettings>(options => 
            {
                options.OpenLibrary = new OpenLibrarySettings
                {
                    BaseUrl = "https://openlibrary.org"
                };
            });
            
            // Register mappers
            services.AddSingleton<IMapper<AuthorResponseDto, Author>, AuthorMapper>();
            services.AddSingleton<IMapper<BookResponseDto, Book>, BookMapper>();
            services.AddSingleton<IMapper<WorksResponseDto, List<BookSummary>>, WorksMapper>();
            
            // Register adapters
            services.AddScoped<IBookPort, OpenLibraryBookAdapter>();
            services.AddScoped<IAuthorPort, OpenLibraryAuthorAdapter>();
            
            // Register services
            services.AddScoped<BookService>();
            
            _serviceProvider = services.BuildServiceProvider();
            _bookService = _serviceProvider.GetRequiredService<BookService>();
            
            await Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _serviceProvider.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetBookByKeyAsync_WithValidKey_ShouldReturnBook()
        {
            // Arrange
            // Livro: Breakthroughs in Science
            var bookKey = "OL46219W"; 

            // Act
            var result = await _bookService.GetBookByKeyAsync(bookKey);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().EndWith(bookKey);
            result.Title.Should().NotBeEmpty();
            result.Title.Should().Be("Breakthroughs in Science");
        }
        
        [Fact]
        public async Task GetBooksByAuthorAsync_WithValidKey_ShouldReturnBooks()
        {
            // Arrange
            // Isaac Asimov's key
            var authorKey = "OL34221A";

            // Act
            var result = await _bookService.GetBooksByAuthorAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetBookByKeyAsync_WithInvalidKey_ShouldReturnNull()
        {
            // Arrange
            // Non-existent key
            var bookKey = "OL999999999W"; 

            // Act
            var result = await _bookService.GetBookByKeyAsync(bookKey);

            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async Task GetPaginatedBooksByAuthorAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            // Isaac Asimov's key
            var authorKey = "OL34221A";
            var page = 1;
            var itemsPerPage = 5;

            // Act
            var result = await _bookService.GetPaginatedBooksByAuthorAsync(authorKey, page, itemsPerPage);

            // Assert
            result.Books.Should().NotBeNull();
            result.Books.Count.Should().BeLessThanOrEqualTo(itemsPerPage);
            result.TotalCount.Should().BeGreaterThan(0);
        }
        
        [Fact]
        public async Task GetPaginatedBooksByAuthorAsync_WithPaging_ShouldReturnDifferentPages()
        {
            // Arrange
            // Isaac Asimov's key (has many books)
            var authorKey = "OL34221A";
            var itemsPerPage = 3;

            // Act
            var firstPageResult = await _bookService.GetPaginatedBooksByAuthorAsync(authorKey, 1, itemsPerPage);
            var secondPageResult = await _bookService.GetPaginatedBooksByAuthorAsync(authorKey, 2, itemsPerPage);

            // Assert
            firstPageResult.Books.Should().NotBeNull();
            secondPageResult.Books.Should().NotBeNull();
            
            // Both pages should have items
            firstPageResult.Books.Should().NotBeEmpty();
            secondPageResult.Books.Should().NotBeEmpty();
            
            // Books on different pages should be different
            firstPageResult.Books.Should().NotContain(book => 
                secondPageResult.Books.Any(b => b.Key == book.Key));
            
            // Total count should be the same for both calls
            firstPageResult.TotalCount.Should().Be(secondPageResult.TotalCount);
        }
    }
}
