using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Application.Mappers;
using LaunchQ.TakeHomeProject.Domain.Models;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace LaunchQ.TakeHomeProject.UnitTests.Application.Mappers
{
    public class WorksMapperTests
    {
        private readonly WorksMapper _worksMapper;

        public WorksMapperTests()
        {
            _worksMapper = new WorksMapper();
        }

        [Fact]
        public void Map_WithNullSource_ShouldReturnEmptyList()
        {
            // Act
            #pragma warning disable CS8604 // Possible null reference argument
            var result = _worksMapper.Map(null!);
            #pragma warning restore CS8604 // Possible null reference argument

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void Map_WithValidSource_ShouldMapCorrectly()
        {
            // Arrange
            var author = new AuthorInfoDto { Key = "/authors/OL1234A" };
            
            var dto = new WorksResponseDto
            {
                Size = 3,
                Links = new PaginationLinksDto { Next = "/authors/OL1234A/works.json?offset=3" },
                Entries = new List<BookResponseDto>
                {
                    new BookResponseDto { Key = "/works/OL1W", Title = "Book 1", Authors = new List<AuthorReferenceDto> { new AuthorReferenceDto { Author = author } } },
                    new BookResponseDto { Key = "/works/OL2W", Title = "Book 2", Authors = new List<AuthorReferenceDto> { new AuthorReferenceDto { Author = author } } },
                    new BookResponseDto { Key = "/works/OL3W", Title = "Book 3", Authors = new List<AuthorReferenceDto> { new AuthorReferenceDto { Author = author } } }
                }
            };

            // Act
            var result = _worksMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            
            // Test first book
            result[0].Key.Should().Be("OL1W");
            result[0].Title.Should().Be("Book 1");
            
            // Test second book
            result[1].Key.Should().Be("OL2W");
            result[1].Title.Should().Be("Book 2");
            
            // Test third book
            result[2].Key.Should().Be("OL3W");
            result[2].Title.Should().Be("Book 3");
        }
        
        [Fact]
        public void Map_WithEmptyEntries_ShouldReturnEmptyList()
        {
            // Arrange
            var dto = new WorksResponseDto
            {
                Size = 0,
                Entries = new List<BookResponseDto>()
            };
            
            // Act
            var result = _worksMapper.Map(dto);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        
        [Fact]
        public void Map_WithNullEntries_ShouldReturnEmptyList()
        {
            // Arrange
            var dto = new WorksResponseDto
            {
                Size = 0,
                Entries = null!
            };
            
            // Act
            var result = _worksMapper.Map(dto);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        
        [Fact]
        public void Map_WithMissingTitleInEntry_ShouldMapWithEmptyTitle()
        {
            // Arrange
            var dto = new WorksResponseDto
            {
                Size = 1,
                Entries = new List<BookResponseDto>
                {
                    new BookResponseDto { Key = "/works/OL1W", Title = null! }
                }
            };
            
            // Act
            var result = _worksMapper.Map(dto);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Key.Should().Be("OL1W");
            result[0].Title.Should().BeEmpty();
        }
    }
}
