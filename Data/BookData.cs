using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Data;

public class BookData
{
    public string? PublishedDate { get; set; }

    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ImageLink { get; set; }
    public string? Isbn_13 { get; set; }
    [Key]
    public int? Id { get; set; }
    public string? Pagecount { get; set; }
    public string? Description { get; set; }

    public BookData(string title, string author, string publishedDate, string imageLink, string isbn_13, string? pagecount, string description)
    {
        Title = title;
        Author = author;
        PublishedDate = publishedDate;
        ImageLink = imageLink;
        Isbn_13 = isbn_13;
        Pagecount = pagecount;
        Description = description;

    }
}
