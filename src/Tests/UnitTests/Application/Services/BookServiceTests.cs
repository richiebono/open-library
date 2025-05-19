using LaunchQ.TakeHomeProject.Application.Services;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.UnitTests.Application.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookPort> _bookPortMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookPortMock = new Mock<IBookPort>();
            _bookService = new BookService(_bookPortMock.Object);
        }

        [Fact]
        public async Task GetBookByKeyAsync_ShouldReturnBookFromPort()
        {
            // Arrange
            var bookKey = "OL1234567W";
            var expectedBook = new Book
            {
                Key = bookKey,
                Title = "Test Book"
            };

            _bookPortMock
                .Setup(x => x.GetBookByKeyAsync(bookKey))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _bookService.GetBookByKeyAsync(bookKey);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedBook);
            _bookPortMock.Verify(x => x.GetBookByKeyAsync(bookKey), Times.Once);
        }

        [Fact]
        public async Task GetBookByKeyAsync_WhenPortReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var bookKey = "OL1234567W";

            _bookPortMock
                .Setup(x => x.GetBookByKeyAsync(bookKey))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.GetBookByKeyAsync(bookKey);

            // Assert
            result.Should().BeNull();
            _bookPortMock.Verify(x => x.GetBookByKeyAsync(bookKey), Times.Once);
        }
        
        [Fact]
        public async Task GetBooksByAuthorAsync_ShouldReturnBooksFromPort()
        {
            // Arrange
            var authorKey = "OL1234567A";
            var expectedBooks = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Book 1" },
                new BookSummary { Key = "OL2W", Title = "Book 2" }
            };

            _bookPortMock
                .Setup(x => x.GetBooksByAuthorAsync(authorKey))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookService.GetBooksByAuthorAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedBooks);
            _bookPortMock.Verify(x => x.GetBooksByAuthorAsync(authorKey), Times.Once);
        }
        
        [Fact]
        public async Task GetBooksByAuthorAsync_WhenPortReturnsNull_ShouldReturnEmptyList()
        {
            // Arrange
            var authorKey = "OL1234567A";

            _bookPortMock
                .Setup(x => x.GetBooksByAuthorAsync(authorKey))
                .ReturnsAsync((List<BookSummary>)null);

            // Act
            var result = await _bookService.GetBooksByAuthorAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _bookPortMock.Verify(x => x.GetBooksByAuthorAsync(authorKey), Times.Once);
        }
        
        [Fact]
        public async Task GetBooksByAuthorAsync_WhenPortReturnsEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            var authorKey = "OL1234567A";

            _bookPortMock
                .Setup(x => x.GetBooksByAuthorAsync(authorKey))
                .ReturnsAsync(new List<BookSummary>());

            // Act
            var result = await _bookService.GetBooksByAuthorAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _bookPortMock.Verify(x => x.GetBooksByAuthorAsync(authorKey), Times.Once);
        }
        
        [Fact]
        public async Task GetBooksByAuthorAsync_WithInvalidKey_ShouldHandleGracefully()
        {
            // Arrange
            string authorKey = null;

            // Act
            var result = await _bookService.GetBooksByAuthorAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _bookPortMock.Verify(x => x.GetBooksByAuthorAsync(It.IsAny<string>()), Times.Never);
        }
        
        [Fact]
        public async Task GetPaginatedBooksByAuthorAsync_ShouldReturnBooksAndTotalCount()
        {
            // Arrange
            var authorKey = "OL1234567A";
            var page = 2;
            var itemsPerPage = 10;
            var offset = (page - 1) * itemsPerPage; // = 10
            
            var expectedBooks = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Book 1" },
                new BookSummary { Key = "OL2W", Title = "Book 2" }
            };
            int expectedTotalCount = 25;

            _bookPortMock
                .Setup(x => x.GetPaginatedBooksByAuthorAsync(authorKey, itemsPerPage, offset))
                .ReturnsAsync((expectedBooks, expectedTotalCount));

            // Act
            var result = await _bookService.GetPaginatedBooksByAuthorAsync(authorKey, page, itemsPerPage);

            // Assert
            result.Books.Should().NotBeNull();
            result.Books.Should().BeEquivalentTo(expectedBooks);
            result.TotalCount.Should().Be(expectedTotalCount);
            _bookPortMock.Verify(x => x.GetPaginatedBooksByAuthorAsync(
                authorKey, itemsPerPage, offset), Times.Once);
        }
        
        [Fact]
        public async Task GetPaginatedBooksByAuthorAsync_WithInvalidKey_ShouldReturnEmpty()
        {
            // Arrange
            string authorKey = null;
            var page = 1;
            var itemsPerPage = 10;

            // Act
            var result = await _bookService.GetPaginatedBooksByAuthorAsync(authorKey, page, itemsPerPage);

            // Assert
            result.Books.Should().NotBeNull();
            result.Books.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            _bookPortMock.Verify(x => x.GetPaginatedBooksByAuthorAsync(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
