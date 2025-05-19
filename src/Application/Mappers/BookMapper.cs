using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Application.Mappers
{
    /// <summary>
    /// Implementação do mapeamento entre BookResponseDto e Book
    /// </summary>
    public class BookMapper : IMapper<BookResponseDto, Book>
    {
        public Book Map(BookResponseDto source)
        {
            if (source == null)
                return new Book();
                
            var book = new Book
            {
                Key = source.Key,
                Title = source.Title,
                First_publish_date = source.First_publish_date,
                Description = source.Description?.Value,
                Authors = source.Authors?.Select(a => a.Author?.Key ?? string.Empty).ToList() ?? new(),
                Subjects = source.Subjects ?? new(),
                Covers = source.Covers != null ? ConvertCoversToDictionary(source.Covers) : new(),
                Excerpt = source.Excerpts?.FirstOrDefault()?.Text
            };
            
            return book;
        }
        
        public Book Map(BookResponseDto source, Book destination)
        {
            if (source == null)
                return destination;
                
            destination.Key = source.Key;
            destination.Title = source.Title;
            destination.First_publish_date = source.First_publish_date;
            destination.Description = source.Description?.Value;
            destination.Authors = source.Authors?.Select(a => a.Author?.Key ?? string.Empty).ToList() ?? new();
            destination.Subjects = source.Subjects ?? new();
            destination.Covers = source.Covers != null ? ConvertCoversToDictionary(source.Covers) : new();
            destination.Excerpt = source.Excerpts?.FirstOrDefault()?.Text;
            
            return destination;
        }
    
        /// <summary>
        /// Converte uma lista de IDs de capas para um dicionário com URLs completas das imagens
        /// </summary>
        private Dictionary<string, string> ConvertCoversToDictionary(List<int> covers)
        {
            var result = new Dictionary<string, string>();
            if (covers != null && covers.Any())
            {
                var sizes = new[] { "S", "M", "L" };
                
                // Obtém o primeiro ID de capa disponível
                int coverId = covers.FirstOrDefault();
                if (coverId > 0)
                {
                    // Para cada tamanho de imagem, adiciona a URL correspondente ao dicionário
                    foreach (var size in sizes)
                    {
                        result[size] = $"https://covers.openlibrary.org/b/id/{coverId}-{size}.jpg";
                    }
                }
            }
            return result;
        }
    }
}
