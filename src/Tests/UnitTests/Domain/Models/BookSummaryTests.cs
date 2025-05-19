using LaunchQ.TakeHomeProject.Domain.Models;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.UnitTests.Domain.Models
{
    public class BookSummaryTests
    {
        [Fact]
        public void BookSummary_WithProperties_ShouldBeInitializedCorrectly()
        {
            // Arrange & Act
            var bookSummary = new BookSummary
            {
                Key = "OL1234567W",
                Title = "Test Book",
                HasCovers = true,
                HasDescription = true
            };

            // Assert
            bookSummary.Key.Should().Be("OL1234567W");
            bookSummary.Title.Should().Be("Test Book");
            bookSummary.HasCovers.Should().BeTrue();
            bookSummary.HasDescription.Should().BeTrue();
        }

        [Fact]
        public void BookSummary_DefaultConstructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var bookSummary = new BookSummary();

            // Assert
            bookSummary.Key.Should().BeEmpty();
            bookSummary.Title.Should().BeEmpty();
            bookSummary.HasCovers.Should().BeFalse();
            bookSummary.HasDescription.Should().BeFalse();
        }
    }
}
