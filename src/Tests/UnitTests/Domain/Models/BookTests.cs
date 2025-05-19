using LaunchQ.TakeHomeProject.Domain.Models;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.UnitTests.Domain.Models
{
    public class BookTests
    {
        [Fact]
        public void Book_WithFullProperties_ShouldBeInitializedCorrectly()
        {
            // Arrange & Act
            var book = new Book
            {
                Key = "OL1234567W",
                Title = "Test Book",
                Description = "A test book description",
                First_publish_date = "2020",
                Authors = new List<string> { "OL1A", "OL2A" },
                Subjects = new List<string> { "Fiction", "Science Fiction" },
                Excerpt = "Test excerpt from Chapter 1",
                Isbn = "1234567890",
                Covers = new Dictionary<string, string>
                {
                    { "S", "https://example.com/cover-small.jpg" },
                    { "M", "https://example.com/cover-medium.jpg" },
                    { "L", "https://example.com/cover-large.jpg" }
                }
            };

            // Assert
            book.Key.Should().Be("OL1234567W");
            book.Title.Should().Be("Test Book");
            book.Description.Should().Be("A test book description");
            book.First_publish_date.Should().Be("2020");
            book.Authors.Should().BeEquivalentTo(new List<string> { "OL1A", "OL2A" });
            book.Subjects.Should().BeEquivalentTo(new List<string> { "Fiction", "Science Fiction" });
            book.Excerpt.Should().Be("Test excerpt from Chapter 1");
            book.Isbn.Should().Be("1234567890");
            book.Covers.Should().HaveCount(3);
            book.Covers["S"].Should().Be("https://example.com/cover-small.jpg");
            book.Covers["M"].Should().Be("https://example.com/cover-medium.jpg");
            book.Covers["L"].Should().Be("https://example.com/cover-large.jpg");
        }

        [Fact]
        public void Book_DefaultConstructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var book = new Book();

            // Assert
            book.Key.Should().BeEmpty();
            book.Title.Should().BeEmpty();
            book.Authors.Should().NotBeNull();
            book.Authors.Should().BeEmpty();
            book.Subjects.Should().NotBeNull();
            book.Subjects.Should().BeEmpty();
            book.Description.Should().BeNull();
            book.Excerpt.Should().BeNull();
            book.First_publish_date.Should().BeNull();
            book.Isbn.Should().BeNull();
            book.Covers.Should().NotBeNull();
            book.Covers.Should().BeEmpty();
        }

        [Fact]
        public void Book_GetCoverImageUrl_ShouldReturnCorrectUrl()
        {
            // Arrange
            var book = new Book
            {
                Key = "/works/OL1234567W"
            };

            // Act
            var smallUrl = book.GetCoverImageUrl("S");
            var mediumUrl = book.GetCoverImageUrl(); // Default is "M"
            var largeUrl = book.GetCoverImageUrl("L");

            // Assert
            smallUrl.Should().Be("https://covers.openlibrary.org/b/id/OL1234567W-S.jpg");
            mediumUrl.Should().Be("https://covers.openlibrary.org/b/id/OL1234567W-M.jpg");
            largeUrl.Should().Be("https://covers.openlibrary.org/b/id/OL1234567W-L.jpg");
        }
    }
}
