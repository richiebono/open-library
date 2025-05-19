using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace LaunchQ.TakeHomeProject.Presentation.Blazor;

public static class AssetsExtensions
{
    public static void MapStaticAssets(this WebApplication app)
    {
        // Configure static files with default options
        // This will serve files from wwwroot by default
        app.UseStaticFiles();

        // Map assets directory for additional files
        app.MapWhen(
            ctx => ctx.Request.Path.StartsWithSegments("/_assets"),
            app => app.UseStaticFiles(new StaticFileOptions { RequestPath = string.Empty }));
    }
}

/// <summary>
/// Component that maps static assets from _content directory in a way that works with hot reload
/// </summary>
public class AssetsImportMap : ComponentBase
{
}

/// <summary>
/// Provides access to static assets from _content directory
/// </summary>
public class Assets : Dictionary<string, string>
{
    public new string this[string key] => key;
}
