using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Application.DTOs
{
    /// <summary>
    /// DTO for author response from OpenLibrary API
    /// </summary>
    public class AuthorResponseDto
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Personal_name { get; set; }
        public string? Birth_date { get; set; }
        public string? Death_date { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; } 
        public List<string>? Works { get; set; }
        public List<string>? Alternate_names { get; set; }
        public List<LinkDto>? Links { get; set; }
        public List<int>? Photos { get; set; }
        public Dictionary<string, string>? Remote_ids { get; set; }
        
        public Author ToAuthor()
        {
            var author = new Author
            {
                Key = Key,
                Name = Name,
                Personal_name = Personal_name,
                Birth_date = Birth_date,
                Death_date = Death_date,
                Bio = Bio
            };
            
            if (Photos != null && Photos.Any())
            {
                var photoMap = new Dictionary<string, string>();
                var sizes = new[] { "S", "M", "L" };
                
                // Obtém o primeiro ID de foto disponível
                int photoId = Photos.FirstOrDefault();
                if (photoId > 0)
                {
                    // Para cada tamanho de imagem, adiciona a URL correspondente ao dicionário
                    foreach (var size in sizes)
                    {
                        photoMap[size] = $"https://covers.openlibrary.org/a/id/{photoId}-{size}.jpg";
                    }
                }
                
                author.Photos = photoMap;
            }
            
            if (Links != null)
            {
                var wikipediaLink = Links.FirstOrDefault(l => l.Title.Contains("wikipedia", StringComparison.OrdinalIgnoreCase))?.Url;
                var officialSiteLink = Links.FirstOrDefault(l => 
                    l.Type?.Key == "/type/link/website" || 
                    l.Title.Contains("official", StringComparison.OrdinalIgnoreCase))?.Url;
                
                author.Wikipedia = wikipediaLink;
                author.OfficialSite = officialSiteLink;
            }
            
            return author;
        }
    }
}
