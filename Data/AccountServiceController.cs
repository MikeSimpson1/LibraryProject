namespace LibraryApplication.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class AccountServiceController : Controller
{
    public AccountServiceController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, NavigationManager navManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.navManager = navManager;
    }

    public UserManager<AppUser> userManager { get; }
    public SignInManager<AppUser> signInManager { get; }
    public NavigationManager navManager { get; set; }

    [HttpPost]
    public async void RegisterUser(AppUser appUser, string password)
    {
        var user = new AppUser { UserName = appUser.UserName, Email = appUser.Email };
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await signInManager.CheckPasswordSignInAsync(user, password, false);
            Redirect("Index", 0);
        }
        Redirect("Register", 1);
    }

    private void Redirect(string? address, int? id)
    {
        if (id == 0)
        {
            navManager.NavigateTo("/" + address + "/" + id);
        }
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExternalLogin()
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "AccountService", new { ReturnUrl = "/signin-google" });
        var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return new ChallengeResult("Google", properties);
    }
}
