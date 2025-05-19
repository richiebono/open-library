using LaunchQ.TakeHomeProject.Domain.Models;
using LaunchQ.TakeHomeProject.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace LaunchQ.TakeHomeProject.Presentation.ViewModels
{
    public class BookDetailViewModel
    {
        private readonly IBookService _bookService;
        
        public BookDetailViewModel(IBookService bookService)
        {
            _bookService = bookService;
        }
        
        public string BookKey { get; set; } = string.Empty;
        public Book? Book { get; set; }
        public bool Loading { get; set; } = true;
        public string? Error { get; set; }
        
        public async Task LoadBookAsync()
        {
            try
            {
                Loading = true;
                Error = null;
                
                Book = await _bookService.GetBookByKeyAsync(BookKey);
                
                if (Book == null)
                {
                    Error = "Failed to load book information.";
                }
            }
            catch (Exception ex)
            {
                Error = $"An error occurred: {ex.Message}";
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
