using IronPython.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApplication.Data
{
    public class ImageService
    {
        BookService bookService;
        public ImageService(BookService bookService)
        {
            this.bookService = bookService;
        }
        public void testAsync()
        {
            var engine = Python.CreateEngine();
            var source = engine.CreateScriptSourceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "calc.py"));
            var scopes = engine.CreateScope();

            source.Execute(scopes);
            var script = scopes.GetVariable("calculator");
            var scriptInstance = engine.Operations.CreateInstance(script);
            List<BookData> books = bookService.GetBookDataFromDbAsync("Stephen", "0");
            foreach (BookData book in books)
            {
                Console.WriteLine("Script: " + scriptInstance.image(book.ImageLink));
            }
        }
    }
}
