using LaunchQ.TakeHomeProject.Application.DTOs;

namespace LaunchQ.TakeHomeProject.Application.DTOs
{
    /// <summary>
    /// DTO for author reference from OpenLibrary API
    /// </summary>
    public class AuthorReferenceDto
    {
        public AuthorInfoDto? Author { get; set; }
    }
}
