using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Data;

public class ReviewData
{
    public string? Date { get; set; }
    public string? User { get; set; }

    [Key]
    public int? Id { get; set; }
    public string? Description { get; set; }
    public string? BookID { get; set; }

    public ReviewData(string user, string date, string description, string bookID)
    {
        User = user;
        Date = date;
        Description = description;
        BookID = bookID;
    }
    public ReviewData()
    {
        User = "";
        Date = "";
        Description = "";
        BookID = "";
    }
}
