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
    public async Task<List<BookData>> GetBookDataAsync(string bookTitle, string startIndex)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BookDataDbContext>();


            List<BookData> books = new List<BookData>();
            bookTitle = bookTitle.Replace(" ", "+");

            string QueryString = "https://www.googleapis.com/books/v1/volumes?q=" + bookTitle + "&maxResults=20&startIndex=" + startIndex;
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
                    if (GetBookByISBN(b.Isbn_13) != null)
                    {
                        books.Add(GetBookByISBN(b.Isbn_13));
                    }
                    else
                    {
                        books.Add(b);
                    }
                    if (!(db.Books.AsQueryable().Where(bb => bb.Isbn_13.Contains(b.Isbn_13)).ToList<BookData>().Count() > 0))
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
            List<BookData> l = db.Books.AsQueryable().Where(b => b.Isbn_13.Contains(isbn)).ToList<BookData>(); //|| b.Author.Contains(author)
            if (l.Count() > 0)
            {
                return l.First();
            }
            return null;
        }
    }

    public BookData GetBookById(int id)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BookDataDbContext>();
            return db.Books.Find(id);
        }
    }

    public List<ReviewData> GetReviewsByBookId(string id)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ReviewDataDbContext>();
            return db.Reviews.AsQueryable().Where(b => b.BookID.Contains(id)).ToList<ReviewData>();
        }
    }

    public async Task<bool> AddBookReview(ReviewData review)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ReviewDataDbContext>();
            db.Add(review);
            await db.SaveChangesAsync();
            return true;
        }
    }

    public List<BookData> getUsersBooks(string booksString)
    {
        if (StringUtils.IsNullOrEmpty(booksString))
        {
            return new List<BookData>();
        }
        List<BookData> listOfBooks = new List<BookData>();
        string[] books = booksString.Split(',');
        foreach (var b in books)
        {
            listOfBooks.Add(GetBookById(int.Parse(b)));
        }
        return listOfBooks;
    }

    public bool isOnBookList(string list, string bookId)
    {
        if (StringUtils.IsNullOrEmpty(list))
        {
            return false;
        }
        string[] IDList = list.Split(',');
        int d = Array.IndexOf(IDList, bookId);
        return d > -1 ? true : false;
    }

    public string AddToBookList(string list, string value)
    {
        if (StringUtils.IsNullOrEmpty(list))
        {
            return value;
        }
        list += "," + value;
        return list;
    }

    public string RemoveFromBookList(string list, string value)
    {
        if (StringUtils.IsNullOrEmpty(list))
        {
            return "";
        }
        List<string> bookList = list.Split(',').ToList<string>();
        bookList.Remove(value);
        return string.Join(',',bookList);
    }
}
