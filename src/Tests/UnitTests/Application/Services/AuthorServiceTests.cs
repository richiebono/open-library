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
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorPort> _authorPortMock;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _authorPortMock = new Mock<IAuthorPort>();
            _authorService = new AuthorService(_authorPortMock.Object);
        }

        [Fact]
        public async Task GetAuthorByKeyAsync_ShouldReturnFromPort()
        {
            // Arrange
            var authorKey = "OL1234567A";
            var expectedAuthor = new Author
            {
                Key = authorKey,
                Name = "Test Author"
            };

            _authorPortMock
                .Setup(x => x.GetAuthorByKeyAsync(authorKey))
                .ReturnsAsync(expectedAuthor);

            // Act
            var result = await _authorService.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedAuthor);
            _authorPortMock.Verify(x => x.GetAuthorByKeyAsync(authorKey), Times.Once);
        }
        
        [Fact]
        public async Task GetAuthorByKeyAsync_WithNullKey_ShouldReturnNull()
        {
            // Arrange
            string authorKey = null;

            // Act
            var result = await _authorService.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().BeNull();
            _authorPortMock.Verify(x => x.GetAuthorByKeyAsync(It.IsAny<string>()), Times.Never);
        }
        
        [Fact]
        public async Task GetAuthorByKeyAsync_WhenPortReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var authorKey = "OL1234567A";

            _authorPortMock
                .Setup(x => x.GetAuthorByKeyAsync(authorKey))
                .ReturnsAsync((Author)null);

            // Act
            var result = await _authorService.GetAuthorByKeyAsync(authorKey);

            // Assert
            result.Should().BeNull();
            _authorPortMock.Verify(x => x.GetAuthorByKeyAsync(authorKey), Times.Once);
        }
        
        // Removed tests for GetPagedBooks and GetTotalPages as they were removed from AuthorService
    }
}
