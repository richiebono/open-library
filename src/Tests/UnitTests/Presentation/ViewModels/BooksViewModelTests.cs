using LaunchQ.TakeHomeProject.Presentation.ViewModels;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using LaunchQ.TakeHomeProject.Domain.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.UnitTests.Presentation.ViewModels
{
    public class BooksViewModelTests
    {
        private readonly Mock<IAuthorService> _authorServiceMock;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BooksViewModel _viewModel;

        public BooksViewModelTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _bookServiceMock = new Mock<IBookService>();
            _viewModel = new BooksViewModel(_authorServiceMock.Object, _bookServiceMock.Object);
        }

        [Fact]
        public async Task LoadAuthorAsync_ShouldLoadAuthorAndBooks()
        {
            // Arrange
            var author = new Author 
            { 
                Key = "OL23919A", 
                Name = "Test Author" 
            };
            
            var books = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Book 1" },
                new BookSummary { Key = "OL2W", Title = "Book 2" }
            };
            
            _authorServiceMock
                .Setup(x => x.GetAuthorByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(author);
                
            _bookServiceMock
                .Setup(x => x.GetPaginatedBooksByAuthorAsync(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<string>()))
                .ReturnsAsync((books, 2));

            // Act
            await _viewModel.LoadAuthorAsync();

            // Assert
            _viewModel.Author.Should().Be(author);
            _viewModel.PagedBooks.Should().BeEquivalentTo(books);
            _viewModel.TotalBooks.Should().Be(2);
            _viewModel.Loading.Should().BeFalse();
            _viewModel.Error.Should().BeNull();
        }

        [Fact]
        public async Task OnSearchQueryChanged_ShouldUpdateSearchQueryAndReloadBooks()
        {
            // Arrange
            string searchQuery = "Fantasy";
            var filteredBooks = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Fantasy Book" }
            };
            
            _bookServiceMock
                .Setup(x => x.GetPaginatedBooksByAuthorAsync(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    searchQuery))
                .ReturnsAsync((filteredBooks, 1));

            // First load the author to initialize _currentAuthorKey field
            var author = new Author { Key = "OL23919A", Name = "Test Author" };
            _authorServiceMock
                .Setup(x => x.GetAuthorByKeyAsync("OL23919A"))
                .ReturnsAsync(author);
            await _viewModel.LoadAuthorAsync();

            // Act
            await _viewModel.OnSearchQueryChanged(searchQuery);

            // Assert
            _viewModel.SearchQuery.Should().Be(searchQuery);
            _viewModel.CurrentPage.Should().Be(1); // Deve resetar para a primeira página
            _viewModel.PagedBooks.Should().BeEquivalentTo(filteredBooks);
            _viewModel.TotalBooks.Should().Be(1);
            
            _bookServiceMock.Verify(x => x.GetPaginatedBooksByAuthorAsync(
                It.IsAny<string>(), 
                1, // Primeira página
                It.IsAny<int>(), 
                searchQuery), 
                Times.Once);
        }
        
        [Fact]
        public async Task OnSearchQueryChanged_WithEmptyQuery_ShouldResetSearch()
        {
            // Arrange
            string searchQuery = string.Empty;
            var allBooks = new List<BookSummary>
            {
                new BookSummary { Key = "OL1W", Title = "Book 1" },
                new BookSummary { Key = "OL2W", Title = "Book 2" }
            };
            
            _bookServiceMock
                .Setup(x => x.GetPaginatedBooksByAuthorAsync(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    null))
                .ReturnsAsync((allBooks, 2));

            // First load the author to initialize _currentAuthorKey field
            var author = new Author { Key = "OL23919A", Name = "Test Author" };
            _authorServiceMock
                .Setup(x => x.GetAuthorByKeyAsync("OL23919A"))
                .ReturnsAsync(author);
            await _viewModel.LoadAuthorAsync();
            
            // Set a previous search query
            _viewModel.SearchQuery = "Previous Search";

            // Act
            await _viewModel.OnSearchQueryChanged(searchQuery);

            // Assert
            _viewModel.SearchQuery.Should().BeEmpty();
            _viewModel.CurrentPage.Should().Be(1); // Deve resetar para a primeira página
            _viewModel.PagedBooks.Should().BeEquivalentTo(allBooks);
            _viewModel.TotalBooks.Should().Be(2);
            
            _bookServiceMock.Verify(x => x.GetPaginatedBooksByAuthorAsync(
                It.IsAny<string>(), 
                1, // Primeira página
                It.IsAny<int>(), 
                null), 
                Times.Exactly(2)); // Called once during LoadAuthorAsync and once during OnSearchQueryChanged
        }
    }
}
