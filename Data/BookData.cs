namespace LibraryApplication.Data;

public class BookData
{
    public string? PublishedDate { get; set; }

    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ImageLink { get; set; }
    public string? ISBN_13 { get; set; }
    public int? PageCount { get; set; }
    public string? Description { get; set; }

    public BookData(string title, string author, string publishedDate, string imageLink, string isbn, int pagecount, string description)
    {
        Title = title;
        Author = author;
        PublishedDate = publishedDate;
        ImageLink = imageLink;
        ISBN_13 = isbn;
        PageCount = pagecount;
        Description = description;

    }
}
