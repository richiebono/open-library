using LaunchQ.TakeHomeProject.Domain.Models;
using System.Text.Json.Serialization;

namespace LaunchQ.TakeHomeProject.Application.DTOs
{
    /// <summary>
    /// DTO for book response from OpenLibrary API
    /// </summary>
    public class BookResponseDto
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<AuthorReferenceDto>? Authors { get; set; }
        
        public Description? Description { get; set; }
        
        public string? First_publish_date { get; set; }
        public List<ExcerptDto>? Excerpts { get; set; }
        public List<int>? Covers { get; set; }
        public List<string>? Subjects { get; set; }
        public Dictionary<string, List<string>>? Identifiers { get; set; }

        public Book ToBook()
        {
            var book = new Book
            {
                Key = Key,
                Title = Title,
                First_publish_date = First_publish_date,
                Description = Description?.Value,
                Authors = Authors?.Select(a => a.Author?.Key ?? string.Empty).ToList(),
                Subjects = Subjects
            };
            
            if (Covers != null && Covers.Any())
            {
                book.Covers = new Dictionary<string, string>();
                // Obtém o primeiro ID de capa disponível
                int coverId = Covers.FirstOrDefault();
                if (coverId > 0)
                {
                    // Para cada tamanho de imagem, adiciona a URL correspondente ao dicionário
                    book.Covers.Add("S", $"https://covers.openlibrary.org/b/id/{coverId}-S.jpg");
                    book.Covers.Add("M", $"https://covers.openlibrary.org/b/id/{coverId}-M.jpg");
                    book.Covers.Add("L", $"https://covers.openlibrary.org/b/id/{coverId}-L.jpg");
                }
            }
            
            if (Identifiers != null && Identifiers.TryGetValue("isbn_13", out var isbn13))
            {
                book.Isbn = isbn13.FirstOrDefault();
            }
            else if (Identifiers != null && Identifiers.TryGetValue("isbn_10", out var isbn10))
            {
                book.Isbn = isbn10.FirstOrDefault();
            }
            
            if (Excerpts != null && Excerpts.Any())
            {
                book.Excerpt = Excerpts.FirstOrDefault()?.Text;
            }
            
            return book;
        }
    }
}
