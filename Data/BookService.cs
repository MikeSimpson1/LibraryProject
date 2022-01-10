namespace LibraryApplication.Data;

using Newtonsoft.Json;
using System.Threading.Tasks;

public class BookService
{
    HttpClient client = new HttpClient();
    public async Task<List<BookData>> GetBookDataAsync(string bookTitle, string bookAuthor)
    {
        List<BookData> books = new List<BookData>();
        bookTitle = bookTitle.Replace(" ", "+");
        bookAuthor = bookAuthor.Replace(" ", "+");
        string QueryString = "https://www.googleapis.com/books/v1/volumes?q=" + bookTitle + "+inauthor:" + bookAuthor;
        var stringTask = await client.GetStringAsync(QueryString);
        dynamic? bookQuery = JsonConvert.DeserializeObject(stringTask);
        if (bookQuery != null)
        {
            foreach (var book in bookQuery.items)
            {
                string title = book.volumeInfo.title;
                string author = book.volumeInfo.authors[0];
                string published = book.volumeInfo.publishedDate;
                string image = "https://www.ianrankin.net/wp-content/uploads/2018/07/missingbook.png";
                try
                {
                    image = book.volumeInfo.imageLinks.thumbnail;
                }
                catch(Exception e) { Console.Write(e.Message); }
                BookData b = new(title, author, published, image);
                books.Add(b);
            }
        }
        return books;
    }
}
