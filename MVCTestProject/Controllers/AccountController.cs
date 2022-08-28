using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVCTestProject.Data;
using MVCTestProject.DataModels;
using MVCTestProject.Services;
using MVCTestProject.ViewModels.Account;

namespace MVCTestProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDatabaseManager<MVCTestProjectContext> _dbManager;

        public AccountController(IDatabaseManager<MVCTestProjectContext> dbManager)
        {
            _dbManager = dbManager;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _dbManager.GetUserByEmailAndPasswordAsync(model.Email, model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "CryptocurrencyListing");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _dbManager.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    user = new User()
                    {
                        Email = model.Email,
                        Password = model.Password,
                        RegistrationDate = DateTime.Now,
                    };
                    await _dbManager.AddUserAsync(user);
                    await Authenticate(model.Email); 
                    
                    return RedirectToAction("Index", "CryptocurrencyListing");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims,
                                    "ApplicationCookie",
                                    ClaimsIdentity.DefaultNameClaimType,
                                    ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Login", "Account");
        }
    }
}
