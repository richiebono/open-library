using LaunchQ.TakeHomeProject.Infrastructure.Configuration;
using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Application.Mappers;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Infrastructure.Adapters;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.UnitTests.Infrastructure.Adapters
{
    public class OpenLibraryAuthorAdapterTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _apiSettings;
        private readonly Mock<IBookPort> _bookPortMock;
        private readonly Mock<IMapper<AuthorResponseDto, Author>> _authorMapperMock;
        private readonly OpenLibraryAuthorAdapter _adapter;

        public OpenLibraryAuthorAdapterTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object);
            _apiSettings = Options.Create(new ApiSettings
            {
                OpenLibrary = new OpenLibrarySettings
                {
                    BaseUrl = "https://openlibrary.org"
                }
            });
            _bookPortMock = new Mock<IBookPort>();
            _authorMapperMock = new Mock<IMapper<AuthorResponseDto, Author>>();

            _adapter = new OpenLibraryAuthorAdapter(_httpClient, _bookPortMock.Object, _apiSettings, _authorMapperMock.Object);
        }

        [Fact]
        public async Task GetAuthorByKeyAsync_ReturnsAuthor()
        {
            // Arrange
            var authorKey = "OL1234567A";
            var authorResponseDto = new AuthorResponseDto
            {
                Key = authorKey,
                Name = "Test Author",
                Birth_date = "1970-01-01"
            };
            
            var mappedAuthor = new Author
            {
                Key = authorKey,
                Name = "Test Author",
                Birth_date = "1970-01-01"
            };
            
            var bookSummaries = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Book 1" }
            };
            
            var responseJson = JsonSerializer.Serialize(authorResponseDto);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && 
                                                        req.RequestUri.ToString().EndsWith($"/authors/{authorKey}.json")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
                
            _authorMapperMock
                .Setup(x => x.Map(It.IsAny<AuthorResponseDto>()))
                .Returns(mappedAuthor);
                
            _bookPortMock
                .Setup(x => x.GetBooksByAuthorAsync(It.IsAny<string>()))
                .ReturnsAsync(bookSummaries);

            // Act
            var result = await _adapter.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().Be(authorKey);
            result.Name.Should().Be("Test Author");
            result.Birth_date.Should().Be("1970-01-01");
            result.Books.Should().BeEquivalentTo(bookSummaries);
            
            _authorMapperMock.Verify(x => x.Map(It.IsAny<AuthorResponseDto>()), Times.Once);
            _bookPortMock.Verify(x => x.GetBooksByAuthorAsync(It.IsAny<string>()), Times.Once);
        }

        // Test for author works retrieval is now handled by BookPort tests, 
        // as OpenLibraryAuthorAdapter delegates to BookPort for book fetching

        [Fact]
        public async Task GetAuthorByKeyAsync_HttpRequestException_ReturnsNull()
        {
            // Arrange
            var authorKey = "OL1234567A";

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Test exception"));

            // Act
            var result = await _adapter.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().BeNull();
        }
        
        // Test for GetPagedBooks removed as the method has been removed from OpenLibraryAuthorAdapter
        
        // GetPagedBooks method has been removed from the OpenLibraryAuthorAdapter
        
        // Test for GetTotalPages removed as the method has been removed from OpenLibraryAuthorAdapter
        
        // Test for GetTotalPages with null books removed as the method has been removed from OpenLibraryAuthorAdapter
    }
}
