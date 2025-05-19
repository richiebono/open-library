namespace LaunchQ.TakeHomeProject.Infrastructure.Configuration
{
    public class ApiSettings
    {
        public OpenLibrarySettings OpenLibrary { get; set; } = new OpenLibrarySettings();
    }

    public class OpenLibrarySettings
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}
