using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Data;

public class AppUser : IdentityUser
{
    public string? BooksWantToRead { get; set; }
    public string? BooksRead { get; set; }
    public string? BooksCurrentlyReading { get; set; }
}
