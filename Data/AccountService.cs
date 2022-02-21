using Microsoft.AspNetCore.Identity;

namespace LibraryApplication.Data
{
    public class AccountService
    {
        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {

        }
        public Boolean RegisterUser(AppUser appUser)
        {
            return true;
        }
    }
}
