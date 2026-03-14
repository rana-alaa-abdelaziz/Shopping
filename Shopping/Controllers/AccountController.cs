using ITIEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Shopping.ViewModels;

namespace Shopping.Controllers
{

    public class AccountController : Controller
    {
        UserManager<App_User> userManager;
        RoleManager<IdentityRole> roleManager;
        SignInManager<App_User> signInManager;
        public AccountController(UserManager<App_User> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<App_User> _signInManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model) 
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new App_User
            {
                Email = model.Email,
                UserName = model.Email, 
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName
            }; var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)

            {
                await userManager.AddToRoleAsync(user, "User");

                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("Email", StringComparison.OrdinalIgnoreCase) ||
                        error.Description.Contains("Username", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("Email", error.Description);
                    }
                    else if (error.Description.Contains("Password", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                    else
                    {
                       
                        ModelState.AddModelError("Email", error.Description);
                    }
                }

            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Categories");
                    }

                    return RedirectToAction("Index", "Catalog");
                }
            }
            ModelState.AddModelError("", "Invalid username or password attempt.");

            return View(model);

        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Catalog");
        }
    }
}
