using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FinanceApp.DataAccessLayer;
using FinanceApp.ViewModels;

namespace FinanceApp.Controllers
{
    public class LoginController: Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly UserDbContext dbContext;
        private readonly DatabaseMethods _dbMethods;

        public LoginController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            UserDbContext dbContext,
            DatabaseMethods dbMethod)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.dbContext = dbContext;
            _dbMethods = dbMethod;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? message = null)
        {
            if(message == null)
            {
                ViewData["message"] = message;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLogin model)
        {
            UserAccount user = _dbMethods.GetUser(model);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Account not found, please try again or register a new account";
                return View(model);
            }
            
            bool hashedPass = Helpers.PasswordHasher.VerifyPassword(model.Password, user.Password);

            if (!hashedPass)
            {
                ViewBag.ErrorMessage = "Invalid username or password please try again";
                return View(model);
            }
            else
            {
                //create account
            }
            
            return View(model);
        }
        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegister model)
        {
            if (ModelState.IsValid)
            {
                if (_dbMethods.RegisterUser(model))
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewBag.ErrorMessage = "Username or Email already exists. Please choose a different one.";
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ChallengeResult ExternalLogin(string provider, string? returnURL = null)
        {
            var redirectURL = Url.Action("RegisterExternalUser", values: new { returnURL });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectURL);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterExternalUser(string? returnURL = null, string? remoteError = null)
        {
            returnURL = returnURL ?? Url.Action("Index", "Home");
            var message = "";

            if (remoteError != null)
            {
                message = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login", routeValues: new { message });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                message = "Error loading external login information.";
                return RedirectToAction("Login", routeValues: new { message });
            }

            var externalLoginResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);

            //The account already exists
            if(externalLoginResult.Succeeded)
            {
                return LocalRedirect(returnURL);
            }

            string email = "";

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            }
            else
            {
                message = "Error while reading the email from the provider.";
                return RedirectToAction("Login", routeValues: new { message });
            }

            var userAccount = new UserAccount() { Email = email, IsSetup = false };
            dbContext.UserAccounts.Add(userAccount);
            await dbContext.SaveChangesAsync();

            var usuario = new IdentityUser() { Email = email, UserName = email };

            var createUserResult = await userManager.CreateAsync(usuario);
            if(!createUserResult.Succeeded)
            {
                message = createUserResult.Errors.First().Description;
                return RedirectToAction("Login", routeValues: new { message });
            }

            var addLoginResult = await userManager.AddLoginAsync(usuario, info);

            if(addLoginResult.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: false, info.LoginProvider);
                return LocalRedirect(returnURL);
            }

            message = "There was an error while logging you in.";
            return RedirectToAction("Login", routeValues: new { message });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
