using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Application.Mappers;
using LaunchQ.TakeHomeProject.Domain.Models;
using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;

namespace LaunchQ.TakeHomeProject.UnitTests.Application.Mappers
{
    public class BookMapperTests
    {
        private readonly BookMapper _bookMapper;

        public BookMapperTests()
        {
            _bookMapper = new BookMapper();
        }

        [Fact]
        public void Map_WithNullSource_ShouldReturnEmptyBook()
        {
            // Act
            var result = _bookMapper.Map((BookResponseDto)null);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().BeEmpty();
            result.Title.Should().BeEmpty();
        }

        [Fact]
        public void Map_WithValidSource_ShouldMapCorrectly()
        {
            // Arrange
            var description = new Description
            {
                Type = "/type/text",
                Value = "A test book description"
            };

            var dto = new BookResponseDto
            {
                Key = "OL1234567W",
                Title = "Test Book",
                Description = description,
                First_publish_date = "2022",
                Authors = new List<AuthorReferenceDto> 
                { 
                    new AuthorReferenceDto { Author = new AuthorInfoDto { Key = "OL123456A" } } 
                },
                Covers = new List<int> { 9876543 },
                Subjects = new List<string> { "Fiction", "Novel" },
                Excerpts = new List<ExcerptDto> 
                { 
                    new ExcerptDto { Text = "Test excerpt", Comment = "First chapter" } 
                }
            };

            // Act
            var result = _bookMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Key.Should().Be("OL1234567W");
            result.Title.Should().Be("Test Book");
            result.Description.Should().Be("A test book description");
            result.First_publish_date.Should().Be("2022");
            result.Authors.Should().HaveCount(1);
            result.Authors[0].Should().Be("OL123456A");
            result.Covers.Should().ContainKeys("S", "M", "L");
            result.Covers["S"].Should().Be("https://covers.openlibrary.org/b/id/9876543-S.jpg");
            result.Covers["M"].Should().Be("https://covers.openlibrary.org/b/id/9876543-M.jpg");
            result.Covers["L"].Should().Be("https://covers.openlibrary.org/b/id/9876543-L.jpg");
            result.Subjects.Should().BeEquivalentTo(new List<string> { "Fiction", "Novel" });
            result.Excerpt.Should().Be("Test excerpt");
        }

        [Fact]
        public void Map_WithEmptyCoversList_ShouldHaveEmptyCovers()
        {
            // Arrange
            var dto = new BookResponseDto
            {
                Key = "OL1234567W",
                Title = "Test Book",
                Covers = new List<int>()
            };

            // Act
            var result = _bookMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Covers.Should().BeEmpty();
        }
        
        [Fact]
        public void Map_WithNullDescription_ShouldMapToEmptyDescription()
        {
            // Arrange
            var dto = new BookResponseDto
            {
                Key = "OL1234567W",
                Title = "Test Book",
                Description = null
            };

            // Act
            var result = _bookMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().BeNull();
        }
        
        [Fact]
        public void Map_WithStringDescription_ShouldMapToDescriptionValue()
        {
            // Arrange
            var dto = new BookResponseDto
            {
                Key = "OL1234567W",
                Title = "Test Book",
                Description = new Description { Value = "Simple string description" }
            };

            // Act
            var result = _bookMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be("Simple string description");
        }

        [Fact]
        public void Map_WithNullLists_ShouldInitializeEmptyCollections()
        {
            // Arrange
            var dto = new BookResponseDto
            {
                Key = "OL1234567W",
                Title = "Test Book",
                Authors = null,
                Subjects = null,
                Excerpts = null,
                Covers = null
            };

            // Act
            var result = _bookMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Authors.Should().NotBeNull().And.BeEmpty();
            result.Subjects.Should().NotBeNull().And.BeEmpty();
            result.Excerpt.Should().BeNull();
            result.Covers.Should().NotBeNull().And.BeEmpty();
        }
    }
}
