using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Models.ViewModels.AccountViewModel;

namespace NewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model )
        {

            if (ModelState.IsValid)
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);
                
                var result = await _userService.RegisterUser(new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    FullName = model.FullName,
                    Age = model.Age,
                    PasswordHash = passwordHash
                });

                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Register");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel() { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userFromDb = await _userService.GetUserByEmail(model.Email);

                if (userFromDb != null)
                {
                    var passwordHash = _userService.GetPasswordHash(model.Password);
                    if (passwordHash.Equals(userFromDb.PasswordHash))
                    {
                        await Authenticate(userFromDb);

                        return string.IsNullOrEmpty(model.ReturnUrl)
                            ? RedirectToAction("Index", "Home")
                            : Redirect(model.ReturnUrl);
                    }
                }
            }
            return View(model);
        }

        private async Task Authenticate(UserDto user)
        {
            const string authType = "ApplicationCookie";
            var claims = new List<Claim>
            {
                new Claim (ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim (ClaimsIdentity.DefaultRoleClaimType, (await _roleService.GetRoleByUserId(user.Id)).Name),
                new Claim("age", user.Age.ToString())

            };
            var identity = new ClaimsIdentity(claims, authType, ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");

        }
        public IActionResult NoAccessRights(LoginViewModel model)
        {
            
            return View(model);
        }
    }
}
