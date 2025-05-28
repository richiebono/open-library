using LaunchQ.TakeHomeProject.Presentation.Blazor.Components;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Ports;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using LaunchQ.TakeHomeProject.Infrastructure.Adapters;
using LaunchQ.TakeHomeProject.Application.Services;
using LaunchQ.TakeHomeProject.Presentation.ViewModels;
using LaunchQ.TakeHomeProject.Infrastructure.Configuration;
using LaunchQ.TakeHomeProject.Application.Mappers;
using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Application.DTOs;
using LaunchQ.TakeHomeProject.Presentation.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure strongly typed settings objects
var apiSettingsSection = builder.Configuration.GetSection("ApiSettings");
builder.Services.Configure<ApiSettings>(apiSettingsSection);

// Register HttpClient
builder.Services.AddHttpClient();

// Register Mappers
builder.Services.AddSingleton<IMapper<AuthorResponseDto, Author>, AuthorMapper>();
builder.Services.AddSingleton<IMapper<BookResponseDto, Book>, BookMapper>();
builder.Services.AddSingleton<IMapper<WorksResponseDto, List<BookSummary>>, WorksMapper>();

// Register Ports with Adapter implementations
builder.Services.AddScoped<IBookPort, OpenLibraryBookAdapter>();
builder.Services.AddScoped<IAuthorPort, OpenLibraryAuthorAdapter>();
builder.Services.AddScoped<ILocalStoragePort, BrowserLocalStorageAdapter>();

// Register Services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

// Register ViewModels as transient services
builder.Services.AddTransient<BooksViewModel>();
builder.Services.AddTransient<BookDetailViewModel>();
builder.Services.AddTransient<FavoriteBooksViewModel>();

// Register Assets class for dependency injection with required dependencies
builder.Services.AddScoped<Assets>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
    bool useHttps = !builder.Configuration.GetValue<bool>("UseHttps", false);
    if (useHttps)
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }
}
else 
{
    app.UseHttpsRedirection();
}

app.MapStaticAssets();

app.UseAntiforgery();
app.MapRazorComponents<LaunchQ.TakeHomeProject.Presentation.Blazor.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();