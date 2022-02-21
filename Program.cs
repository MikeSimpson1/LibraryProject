using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using LibraryApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
      options.AccessDeniedPath = "/forbidden/";
      options.LoginPath = "/login/";
      options.Cookie.HttpOnly = true;
      options.Cookie.SameSite = SameSiteMode.Lax;
  })
  .AddGoogle(options =>
  {
      options.ClientId = "693345690045-4ajd0sr6uvtpt12bebaqldll7ptv3do6.apps.googleusercontent.com";
      options.ClientSecret = "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";
  });
builder.Services.AddHttpClient();
builder.Services.AddDbContext<BookDataDbContext>(
        options => options.UseNpgsql("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddDbContext<UserDbContext>(
        options => options.UseNpgsql("name=ConnectionStrings:DefaultConnection"));
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default","{controller=AccountService}/{action=Index}/{id?}");
});
app.Run();
