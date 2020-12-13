using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.ViewModel;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Services;
using Microsoft.AspNetCore.Authorization;

namespace TimeOrganizer.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private IMailService mailService;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IMailService mailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl)
        {
            if (ModelState.IsValid) {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);

                bool isEmailConfirmed = true;
                if (user != null && !user.EmailConfirmed && (await userManager.CheckPasswordAsync(user, loginViewModel.Password))) {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet.");
                    isEmailConfirmed = false;
                }

                var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

                if (result.Succeeded && isEmailConfirmed)
                {
                    if (Url.IsLocalUrl(ReturnUrl) && !String.IsNullOrEmpty(ReturnUrl)){
                        return Redirect(ReturnUrl);
                    }
                    else {
                        return RedirectToAction("index", "home");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            //Return Errors as JSON 
            //Later can be return View(model);
            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }

        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Email = registerViewModel.Email,
                    EmailConfirmed = false
                };

                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                
                if (result.Succeeded)
                {
                    var createRoleResult = await userManager.AddToRoleAsync(user, "User");

                    if (createRoleResult.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                        MailRequest mailRequest = new MailRequest
                        {
                            Subject = "Confirm your email",
                            Body = $"Click here to confirm your email address: {confirmationLink}",
                            ToEmail = user.Email
                        };

                        try
                        {
                            await mailService.SendEmailAsync(mailRequest);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(String.Empty, ex.Message);
                        }

                    }
                    else {
                        foreach (var error in createRoleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        //User name 'nciganovic@gmail.com' is already taken is possible answer 
                    }
                }

            }
          
            //Return Errors as JSON 
            //Later can be return View(model);
            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { Errors = invalidModelStateError });
        }

        [HttpGet]
        [AllowAnonymous] //TODO mayber unnecessery
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new JsonResult(new { Errors = $"User with Id = {userId} is not found." });
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return new JsonResult(new { message = $"{user.Email} is successfully confirmed." });
            }
            else
            {
                return new JsonResult(new { Errors = $"failed to confirm {user.Email}" });
            }
        }

    }
}
