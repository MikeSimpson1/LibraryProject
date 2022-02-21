using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using LibraryApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<BookDataDbContext>(
        options => options.UseNpgsql("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddDbContext<UserDbContext>(
        options => options.UseNpgsql("name=ConnectionStrings:DefaultConnection"));
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
