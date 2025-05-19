using System.Text.Json.Serialization;

namespace LaunchQ.TakeHomeProject.Domain.Models
{
    /// <summary>
    /// Domain entity representing a book description
    /// </summary>
    public class Description
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
