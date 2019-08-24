using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.WebUI.Identity;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _mailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender mailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailSender = mailSender;
        }
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // generate token
                var createtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackurl = Url.Action("ConfirmEmail", "Account", new
                {
                    userId=user.Id,
                    token= createtoken
                });

                await _mailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayın", $"Lütfen hesabınızı onaylaman için linke <a href='http://localhost:63762{callbackurl}'>tıklayınız.</a>");
                // send email

                return RedirectToAction("Login", "Account");
            }


            ModelState.AddModelError("", "Bilinmeyen hata oluştu lütfen tekrar deneyiniz.");
            return View(model);
        }



        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? "~/";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                ViewBag.ErrorMessage="Bu kullanıcı ile daha önce hesap oluşturulmamış.";
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Hesabınızı onaylayın.");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            ModelState.AddModelError("", "Kullanıcı adı ve ya parola yanlış");
            return View(model);
        }


        public async Task<IActionResult>Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId==null || token==null)
            {
                TempData["message"] = "Geçersiz token";
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user==null)
            {
                TempData["message"] = "Geçersiz kullanıcı";
                return View();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if(result.Succeeded)
            {
                TempData["message"] = "Hesabını onaylandı";

                return View();

            }
            TempData["message"] = "Hesabınız onaylanmadı";

            return View();
        }
    }
}