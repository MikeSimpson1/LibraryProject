namespace LibraryApplication.Data;

using Newtonsoft.Json;
using System.Threading.Tasks;

public class BookService
{
    HttpClient client = new HttpClient();
    public async Task<List<BookData>> GetBookDataAsync()
    {
        List<BookData> books = new List<BookData>();
        var stringTask = await client.GetStringAsync("https://www.googleapis.com/books/v1/volumes?q=the+institute+inauthor:stephen+king");
        dynamic? bookQuery = JsonConvert.DeserializeObject(stringTask);
        if (bookQuery != null)
        {
            foreach (var book in bookQuery.items)
            {
                string title = book.volumeInfo.title;
                string author = book.volumeInfo.authors[0];
                string published = book.volumeInfo.publishedDate;
                string image = book.volumeInfo.imageLinks.thumbnail;
                BookData b = new(title, author, published, image);
                books.Add(b);
            }
        }
        return books;
    }
}
