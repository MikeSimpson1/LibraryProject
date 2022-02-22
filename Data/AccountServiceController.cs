namespace LibraryApplication.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

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

    public async Task<IActionResult> Logout()
    {
        if (signInManager.IsSignedIn(User))
        {
            await signInManager.SignOutAsync();
        }
        return LocalRedirect("/");
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExternalLogin()
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "AccountService", new { ReturnUrl = "/" });
        var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return new ChallengeResult("Google", properties);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (remoteError != null)
        {
            // display error
            return LocalRedirect(returnUrl);
        }
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            //error
            return LocalRedirect(returnUrl);
        }
        var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (email!= null)
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };

                    await userManager.CreateAsync(user);
                }

                await userManager.AddLoginAsync(user, info);
                await signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }
        }
        return LocalRedirect(returnUrl);
    }
}
