namespace LibraryApplication.Data;

public class BookData
{
    public string? PublishedDate { get; set; }

    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ImageLink { get; set; }

    public BookData(string title, string author, string publishedDate, string imageLink)
    {
        Title = title;
        Author = author;
        PublishedDate = publishedDate;
        ImageLink = imageLink;

    }
}
