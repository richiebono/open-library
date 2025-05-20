using LaunchQ.TakeHomeProject.Infrastructure.Configuration;
using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Application.Mappers;
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
    public class OpenLibraryBookAdapterTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _apiSettings;
        private readonly Mock<IMapper<BookResponseDto, Book>> _bookMapperMock;
        private readonly Mock<IMapper<WorksResponseDto, List<BookSummary>>> _worksMapperMock;
        private readonly OpenLibraryBookAdapter _adapter;

        public OpenLibraryBookAdapterTests()
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
            _bookMapperMock = new Mock<IMapper<BookResponseDto, Book>>();
            _worksMapperMock = new Mock<IMapper<WorksResponseDto, List<BookSummary>>>();

            _adapter = new OpenLibraryBookAdapter(_httpClient, _apiSettings, _bookMapperMock.Object, _worksMapperMock.Object);
        }

        [Fact]
        public async Task GetBookByKeyAsync_ReturnsBook()
        {
            // Arrange
            var bookKey = "OL1234567W";
            var description = new Description
            {
                Type = "/type/text",
                Value = "A test book description"
            };
            
            var bookResponseDto = new BookResponseDto
            {
                Key = bookKey,
                Title = "Test Book",
                Description = description
            };
            
            var expectedBook = new Book
            {
                Key = bookKey,
                Title = "Test Book",
                Description = "A test book description"
            };
            
            var responseJson = JsonSerializer.Serialize(bookResponseDto);
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
                                                        req.RequestUri!.ToString().EndsWith($"/works/{bookKey}.json")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
                
            _bookMapperMock
                .Setup(x => x.Map(It.IsAny<BookResponseDto>()))
                .Returns(expectedBook);

            // Act
            var result = await _adapter.GetBookByKeyAsync(bookKey);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().Be(bookKey);
            result.Title.Should().Be("Test Book");
            result.Description.Should().Be("A test book description");
            _bookMapperMock.Verify(x => x.Map(It.IsAny<BookResponseDto>()), Times.Once);
        }

        [Fact]
        public async Task GetBookByKeyAsync_HttpRequestException_ReturnsNull()
        {
            // Arrange
            var bookKey = "OL1234567W";

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Test exception"));

            // Act
            var result = await _adapter.GetBookByKeyAsync(bookKey);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetBookByKeyAsync_NotFound_ReturnsNull()
        {
            // Arrange
            var bookKey = "OL1234567W";
            
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _adapter.GetBookByKeyAsync(bookKey);

            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async Task GetBooksByAuthorAsync_ReturnsBookList()
        {
            // Arrange
            var authorKey = "OL1234567A";
            
            var worksResponseDto = new WorksResponseDto
            {
                Size = 2,
                Entries = new List<BookResponseDto>
                {
                    new BookResponseDto { Key = "OL1W", Title = "Book 1" },
                    new BookResponseDto { Key = "OL2W", Title = "Book 2" }
                }
            };
            
            var responseJson = JsonSerializer.Serialize(worksResponseDto);
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
                                                       req.RequestUri!.ToString().Contains($"/authors/{authorKey}/works.json")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
                
            var expectedBooks = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Book 1" },
                new BookSummary { Key = "OL2W", Title = "Book 2" }
            };
            
            _worksMapperMock
                .Setup(x => x.Map(It.IsAny<WorksResponseDto>()))
                .Returns(expectedBooks);
                
            // Act
            var result = await _adapter.GetBooksByAuthorAsync(authorKey);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedBooks);
        }
        
        [Fact]
        public async Task GetPaginatedBooksByAuthorAsync_ReturnsBooksAndTotalCount()
        {
            // Arrange
            var authorKey = "OL1234567A";
            var limit = 10;
            var offset = 20;
            
            var worksResponseDto = new WorksResponseDto
            {
                Size = 45,
                Links = new PaginationLinksDto 
                { 
                    Next = "/authors/OL1234567A/works.json?limit=10&offset=30",
                    Previous = "/authors/OL1234567A/works.json?limit=10&offset=10"
                },
                Entries = new List<BookResponseDto>
                {
                    new BookResponseDto { Key = "OL21W", Title = "Book 21" },
                    new BookResponseDto { Key = "OL22W", Title = "Book 22" }
                }
            };
            
            var responseJson = JsonSerializer.Serialize(worksResponseDto);
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
                                                      req.RequestUri!.ToString().Contains($"/authors/{authorKey}/works.json?limit={limit}&offset={offset}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
                
            var expectedBooks = new List<BookSummary>
            {
                new BookSummary { Key = "OL21W", Title = "Book 21" },
                new BookSummary { Key = "OL22W", Title = "Book 22" }
            };
            
            _worksMapperMock
                .Setup(x => x.Map(It.IsAny<WorksResponseDto>()))
                .Returns(expectedBooks);
                
            // Act
            var result = await _adapter.GetPaginatedBooksByAuthorAsync(authorKey, limit, offset);
            
            // Assert
            result.Books.Should().NotBeNull();
            result.Books.Should().BeEquivalentTo(expectedBooks);
            result.TotalCount.Should().Be(45);
            _worksMapperMock.Verify(x => x.Map(It.IsAny<WorksResponseDto>()), Times.Once);
        }
        
        [Fact]
        public async Task GetPaginatedBooksByAuthorAsync_HttpRequestException_ReturnsEmptyListAndZeroCount()
        {
            // Arrange
            var authorKey = "OL1234567A";
            var limit = 10;
            var offset = 0;

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Test exception"));

            // Act
            var result = await _adapter.GetPaginatedBooksByAuthorAsync(authorKey, limit, offset);

            // Assert
            result.Books.Should().NotBeNull();
            result.Books.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }
        
        [Fact]
        public async Task GetPaginatedBooksByAuthorAsync_WithSearchQuery_FiltersBooksByTitle()
        {
            // Arrange
            var authorKey = "OL1234567A";
            var limit = 10;
            var offset = 0;
            var searchQuery = "Fantasy";
            
            var worksResponseDto = new WorksResponseDto
            {
                Size = 4,
                Entries = new List<BookResponseDto>
                {
                    new BookResponseDto { Key = "OL1W", Title = "Fantasy Adventure" },
                    new BookResponseDto { Key = "OL2W", Title = "Science Fiction" },
                    new BookResponseDto { Key = "OL3W", Title = "Fantasy World" },
                    new BookResponseDto { Key = "OL4W", Title = "Mystery Novel" }
                }
            };
            
            var responseJson = JsonSerializer.Serialize(worksResponseDto);
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
                                                      req.RequestUri!.ToString().Contains($"/authors/{authorKey}/works.json?limit={limit}&offset={offset}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
                
            var bookSummaries = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Fantasy Adventure" },
                new BookSummary { Key = "OL2W", Title = "Science Fiction" },
                new BookSummary { Key = "OL3W", Title = "Fantasy World" },
                new BookSummary { Key = "OL4W", Title = "Mystery Novel" }
            };
            
            _worksMapperMock
                .Setup(x => x.Map(It.IsAny<WorksResponseDto>()))
                .Returns(bookSummaries);

            // Act
            var result = await _adapter.GetPaginatedBooksByAuthorAsync(authorKey, limit, offset, searchQuery);

            // Assert
            result.Books.Should().NotBeNull();
            result.Books.Count.Should().Be(2);
            result.TotalCount.Should().Be(2);
            result.Books.Should().OnlyContain(b => b.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            
            _worksMapperMock.Verify(x => x.Map(It.IsAny<WorksResponseDto>()), Times.Once);
        }
    }
}
