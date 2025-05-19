using System.Text.Json.Serialization;

namespace LaunchQ.TakeHomeProject.Domain.Models
{
    /// <summary>
    /// Domain entity representing a Book Summary (simplified version of a Book)
    /// </summary>
    public class BookSummary
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        
        // Properties to store the flags for UI display
        public bool HasCovers { get; set; } = false;
        public bool HasDescription { get; set; } = false;
        
        // Properties from the API response
        [JsonPropertyName("covers")]
        public List<int>? Covers { get; set; }
        
        [JsonPropertyName("description")]
        public Description? Description { get; set; }

        public string GetFormattedKey()
        {
            return Key.Replace("/works/", "");
        }
        
        /// <summary>
        /// Checks if the book has complete information for details display
        /// </summary>
        public bool HasCompleteDetails()
        {
            // A book has complete details if it has a non-empty title and at least
            // one of the two pieces of information: description or cover
            return !string.IsNullOrEmpty(Title) && (HasDescription || HasCovers);
        }
    }
}
