namespace LibraryApplication.Data;

using Newtonsoft.Json;
using System.Threading.Tasks;

public class BookService
{
    HttpClient client = new HttpClient();
    private readonly IServiceScopeFactory scopeFactory;
    public BookService(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }
    public async Task<List<BookData>> GetBookDataAsync(string bookTitle, string bookAuthor)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BookDataDbContext>();


            List<BookData> books = new List<BookData>();
            bookTitle = bookTitle.Replace(" ", "+");
            bookAuthor = bookAuthor.Replace(" ", "+");

            /*  
             *  TODO:
             *  This needs to work. First query the database, and then query google.
             * 
             */
            List<BookData> fromDb = GetBookDataFromDbAsync(bookTitle, bookAuthor);
            if (fromDb.Count >= 10)
            {
                return fromDb;
            }
            //

            string QueryString = "https://www.googleapis.com/books/v1/volumes?q=" + bookTitle + "+inauthor:" + bookAuthor;
            var stringTask = await client.GetStringAsync(QueryString);
            dynamic? bookQuery = JsonConvert.DeserializeObject(stringTask);
            if (bookQuery != null && bookQuery.items != null)
            {
                foreach (var book in bookQuery.items)
                {
                    string title = "";
                    string author = "";
                    string published = "";
                    string image = "https://www.ianrankin.net/wp-content/uploads/2018/07/missingbook.png";
                    string isbn = "";
                    string pagecount = "0";
                    string description = "";
                    try
                    {
                        title = book.volumeInfo.title;
                    }
                    catch (Exception e) { Console.Write(e.Message); }

                    try
                    {
                        author = book.volumeInfo.authors[0];
                    }
                    catch (Exception e) { Console.Write(e.Message); }

                    try
                    {
                        published = book.volumeInfo.publishedDate;
                    }
                    catch (Exception e) { Console.Write(e.Message); }
                    
                    try
                    {
                        image = book.volumeInfo.imageLinks.thumbnail;
                    }
                    catch (Exception e) { Console.Write(e.Message); }

                    try
                    {
                        isbn = book.volumeInfo.industryIdentifiers[0].identifier;
                    }
                    catch (Exception e) { Console.Write(e.Message); }

                    try
                    {
                        pagecount = book.volumeInfo.pageCount;
                    }
                    catch (Exception e) { Console.Write(e.Message); }

                    try
                    {
                        description = book.volumeInfo.description;
                    }
                    catch (Exception e) { Console.Write(e.Message); }

                    BookData b = new(title, author, published, image, isbn, pagecount, description);
                    books.Add(b);
                    if (!db.Books.Contains(b))
                    {
                        db.Add(b);
                    }
                }
            }
            await db.SaveChangesAsync();

            return books;
        }
    }
    public List<BookData> GetBookDataFromDbAsync(string title, string author)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BookDataDbContext>();
            List<BookData> l = db.Books.AsQueryable().Where(b => b.Title.Contains(title)).ToList<BookData>(); //|| b.Author.Contains(author)
            return l;
        }
    }

    public BookData GetBookByISBN(string isbn)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BookDataDbContext>();
            return db.Books.Find(isbn);
        }
    }
}
