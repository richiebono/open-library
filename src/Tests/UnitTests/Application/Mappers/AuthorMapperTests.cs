using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Application.Mappers;
using LaunchQ.TakeHomeProject.Domain.Models;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace LaunchQ.TakeHomeProject.UnitTests.Application.Mappers
{
    public class AuthorMapperTests
    {
        private readonly AuthorMapper _authorMapper;

        public AuthorMapperTests()
        {
            _authorMapper = new AuthorMapper();
        }

        [Fact]
        public void Map_WithNullSource_ShouldReturnEmptyAuthor()
        {
            // Act
            var result = _authorMapper.Map((AuthorResponseDto)null);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().BeEmpty();
            result.Name.Should().BeEmpty();
        }

        [Fact]
        public void Map_WithValidSource_ShouldMapCorrectly()
        {
            // Arrange
            var dto = new AuthorResponseDto
            {
                Key = "OL1234567A",
                Name = "John Doe",
                Personal_name = "John R. Doe",
                Birth_date = "1970-01-01",
                Death_date = "2023-01-01",
                Bio = "A test author biography",
                Works = new List<string> { "Work1", "Work2" },
                Alternate_names = new List<string> { "Johnny", "J. Doe" },
                Photos = new List<int> { 1234567 }
            };

            // Act
            var result = _authorMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().Be("OL1234567A");
            result.Name.Should().Be("John Doe");
            result.Personal_name.Should().Be("John R. Doe");
            result.Birth_date.Should().Be("1970-01-01");
            result.Death_date.Should().Be("2023-01-01");
            result.Bio.Should().Be("A test author biography");
            result.Works.Should().BeEquivalentTo(new List<string> { "Work1", "Work2" });
            result.Alternate_names.Should().BeEquivalentTo(new List<string> { "Johnny", "J. Doe" });
            result.Photos.Should().ContainKeys("S", "M", "L");
            result.Photos["S"].Should().Be("https://covers.openlibrary.org/a/id/1234567-S.jpg");
            result.Photos["M"].Should().Be("https://covers.openlibrary.org/a/id/1234567-M.jpg");
            result.Photos["L"].Should().Be("https://covers.openlibrary.org/a/id/1234567-L.jpg");
        }

        [Fact]
        public void Map_WithExistingDestination_ShouldUpdateDestination()
        {
            // Arrange
            var dto = new AuthorResponseDto
            {
                Key = "OL1234567A",
                Name = "John Doe",
                Photos = new List<int> { 1234567 }
            };

            var existingAuthor = new Author
            {
                Key = "OldKey",
                Name = "Old Name",
                Birth_date = "Old Birth Date"
            };

            // Act
            var result = _authorMapper.Map(dto, existingAuthor);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(existingAuthor);
            result.Key.Should().Be("OL1234567A");
            result.Name.Should().Be("John Doe");
            result.Birth_date.Should().BeNull();
            result.Photos.Should().ContainKeys("S", "M", "L");
        }

        [Fact]
        public void Map_WithEmptyPhotosList_ShouldHaveEmptyPhotos()
        {
            // Arrange
            var dto = new AuthorResponseDto
            {
                Key = "OL1234567A",
                Name = "John Doe",
                Photos = new List<int>()
            };

            // Act
            var result = _authorMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Photos.Should().BeEmpty();
        }
        
        [Fact]
        public void Map_WithNullCollections_ShouldInitializeEmptyCollections()
        {
            // Arrange
            var dto = new AuthorResponseDto
            {
                Key = "OL1234567A",
                Name = "John Doe",
                Works = null,
                Alternate_names = null,
                Photos = null
            };

            // Act
            var result = _authorMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Works.Should().NotBeNull().And.BeEmpty();
            result.Alternate_names.Should().NotBeNull().And.BeEmpty();
            result.Photos.Should().NotBeNull().And.BeEmpty();
        }
        
        [Fact]
        public void Map_WithAllNullProperties_ShouldInitializeBasicProperties()
        {
            // Arrange
            var dto = new AuthorResponseDto
            {
                Key = "OL1234567A",
                Name = null,
                Personal_name = null,
                Birth_date = null,
                Death_date = null,
                Bio = null
            };

            // Act
            var result = _authorMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().Be("OL1234567A");
            result.Name.Should().BeNull();
            result.Personal_name.Should().BeNull();
            result.Birth_date.Should().BeNull();
            result.Death_date.Should().BeNull();
            result.Bio.Should().BeNull();
        }
    }
}
