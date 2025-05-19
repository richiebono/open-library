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
    public class AuthorServiceIntegrationTests : IAsyncLifetime
    {
        private ServiceProvider _serviceProvider = null!;
        private AuthorService _authorService = null!;

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
            services.AddScoped<AuthorService>();
            
            _serviceProvider = services.BuildServiceProvider();
            _authorService = _serviceProvider.GetRequiredService<AuthorService>();
            
            await Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _serviceProvider.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetAuthorByKeyAsync_WithValidKey_ShouldReturnAuthor()
        {
            // Arrange
            // Isaac Asimov's key
            var authorKey = "OL34221A"; 

            // Act
            var result = await _authorService.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().EndWith(authorKey);
            result.Name.Should().NotBeEmpty();
            result.Books.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task GetAuthorByKeyAsync_WithInvalidKey_ShouldReturnNull()
        {
            // Arrange
            // Non-existent key
            var authorKey = "OL999999999A"; 

            // Act
            var result = await _authorService.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().BeNull();
        }
    }
}
