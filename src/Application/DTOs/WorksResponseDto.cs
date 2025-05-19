using LaunchQ.TakeHomeProject.Domain.Models;
using System.Text.Json.Serialization;

namespace LaunchQ.TakeHomeProject.Application.DTOs
{
    /// <summary>
    /// DTO for the OpenLibrary API response structure for an author's works
    /// </summary>
    public class WorksResponseDto
    {
        /// <summary>
        /// Total number of works from the author
        /// </summary>
        [JsonPropertyName("size")]
        public int? Size { get; set; }
        
        /// <summary>
        /// Links for pagination
        /// </summary>
        public PaginationLinksDto? Links { get; set; }
        
        /// <summary>
        /// The list of works entries
        /// </summary>
        [JsonPropertyName("entries")]
        public List<BookResponseDto> Entries { get; set; } = new List<BookResponseDto>();
    }

    /// <summary>
    /// DTO for each individual work returned by the OpenLibrary API
    /// </summary>
    public class WorkEntryDto
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<int>? Covers { get; set; }
        public Description? Description { get; set; }
    }
}
