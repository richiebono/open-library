using LaunchQ.TakeHomeProject.Domain.Models;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.UnitTests.Domain.Models
{
    public class AuthorTests
    {
        [Fact]
        public void Author_WithFullProperties_ShouldBeInitializedCorrectly()
        {
            // Arrange & Act
            var author = new Author
            {
                Key = "OL1234567A",
                Name = "John Doe",
                Personal_name = "John R. Doe",
                Birth_date = "1970-01-01",
                Death_date = "2023-01-01",
                Bio = "A test author biography",
                Works = new List<string> { "Work1", "Work2" },
                Alternate_names = new List<string> { "Johnny", "J. Doe" },
                Photos = new Dictionary<string, string>
                {
                    { "S", "https://example.com/photo-small.jpg" },
                    { "M", "https://example.com/photo-medium.jpg" },
                    { "L", "https://example.com/photo-large.jpg" }
                }
            };

            // Assert
            author.Key.Should().Be("OL1234567A");
            author.Name.Should().Be("John Doe");
            author.Personal_name.Should().Be("John R. Doe");
            author.Birth_date.Should().Be("1970-01-01");
            author.Death_date.Should().Be("2023-01-01");
            author.Bio.Should().Be("A test author biography");
            author.Works.Should().BeEquivalentTo(new List<string> { "Work1", "Work2" });
            author.Alternate_names.Should().BeEquivalentTo(new List<string> { "Johnny", "J. Doe" });
            author.Photos.Should().HaveCount(3);
            author.Photos["S"].Should().Be("https://example.com/photo-small.jpg");
            author.Photos["M"].Should().Be("https://example.com/photo-medium.jpg");
            author.Photos["L"].Should().Be("https://example.com/photo-large.jpg");
        }

        [Fact]
        public void Author_DefaultConstructor_ShouldInitializeCollections()
        {
            // Arrange & Act
            var author = new Author();

            // Assert
            author.Works.Should().NotBeNull();
            author.Works.Should().BeEmpty();
            author.Alternate_names.Should().NotBeNull();
            author.Alternate_names.Should().BeEmpty();
            author.Photos.Should().NotBeNull();
            author.Photos.Should().BeEmpty();
        }

        [Fact]
        public void Author_WithNullCollections_ShouldHandleGracefully()
        {
            // Arrange & Act
            var author = new Author
            {
                Key = "OL1234567A",
                Name = "John Doe",
                Works = null!,
                Alternate_names = null!,
                Photos = null!,
                Books = null!
            };

            // Assert
            author.Key.Should().Be("OL1234567A");
            author.Name.Should().Be("John Doe");
            
            (author.Works == null).Should().BeTrue();
            (author.Alternate_names == null).Should().BeTrue();
            (author.Photos == null).Should().BeTrue();
            (author.Books == null).Should().BeTrue();
        }
        
        [Fact]
        public void Author_WithBooks_ShouldHaveBooksCollection()
        {
            // Arrange & Act
            var author = new Author
            {
                Key = "OL1234567A",
                Name = "John Doe",
                Books = new List<BookSummary>
                {
                    new BookSummary { Key = "OL1W", Title = "Book 1", HasCovers = true, HasDescription = true },
                    new BookSummary { Key = "OL2W", Title = "Book 2", HasCovers = false, HasDescription = true }
                }
            };

            // Assert
            author.Books.Should().NotBeNull();
            author.Books.Should().HaveCount(2);
            author.Books[0].Key.Should().Be("OL1W");
            author.Books[0].Title.Should().Be("Book 1");
            author.Books[0].HasCovers.Should().BeTrue();
            author.Books[0].HasDescription.Should().BeTrue();
            author.Books[1].Key.Should().Be("OL2W");
            author.Books[1].Title.Should().Be("Book 2");
            author.Books[1].HasCovers.Should().BeFalse();
            author.Books[1].HasDescription.Should().BeTrue();
        }
    }
}
