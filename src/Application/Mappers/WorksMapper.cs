using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Domain.Models;
using System.Text.RegularExpressions;

namespace LaunchQ.TakeHomeProject.Application.Mappers
{
    /// <summary>
    /// Implementation of mapping between WorksResponseDto and BookSummary
    /// </summary>
    public class WorksMapper : IMapper<WorksResponseDto, List<BookSummary>>
    {
        public List<BookSummary> Map(WorksResponseDto source)
        {
            if (source == null || source.Entries == null)
                return new List<BookSummary>();
                
            return source.Entries.Select(entry => new BookSummary
            {
                Key = ExtractKeyFromPath(entry.Key),
                Title = entry.Title ?? string.Empty,
                Covers = entry.Covers,
                HasCovers = entry.Covers != null && entry.Covers.Count > 0,
                HasDescription = entry.Description != null
            }).ToList();
        }
        
        public List<BookSummary> Map(WorksResponseDto source, List<BookSummary> destination)
        {
            if (source == null || source.Entries == null)
                return destination;
            
            var result = source.Entries.Select(entry => new BookSummary
            {
                Key = ExtractKeyFromPath(entry.Key),
                Title = entry.Title ?? string.Empty,
                Covers = entry.Covers,
                HasCovers = entry.Covers != null && entry.Covers.Count > 0,
                HasDescription = entry.Description != null
            }).ToList();
            
            destination.Clear();
            destination.AddRange(result);
            
            return destination;

        }
        
            
        private string ExtractKeyFromPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            
            var match = Regex.Match(path, @"/works/([A-Z0-9]+)");
            return match.Success ? match.Groups[1].Value : path;
        }
        
        private Dictionary<string, string> GenerateCoverUrls(int? coverId)
        {
            if (coverId == null) return new Dictionary<string, string>();
            
            return new Dictionary<string, string>
            {
                { "S", $"https://covers.openlibrary.org/b/id/{coverId}-S.jpg" },
                { "M", $"https://covers.openlibrary.org/b/id/{coverId}-M.jpg" },
                { "L", $"https://covers.openlibrary.org/b/id/{coverId}-L.jpg" }
            };
        }
    }
}
