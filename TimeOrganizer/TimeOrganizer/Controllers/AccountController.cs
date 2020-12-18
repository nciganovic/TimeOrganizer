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
        private IMailRequestRepository mailRequestRepository;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IMailService mailService,
            IMailRequestRepository mailRequestRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailService = mailService;
            this.mailRequestRepository = mailRequestRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl)
        {
            if (ModelState.IsValid) {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This user does not exist");
                }
                else if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet.");
                }
                else if (!await userManager.CheckPasswordAsync(user, loginViewModel.Password))
                {
                    ModelState.AddModelError(string.Empty, "Wrong password");
                }
                else {
                    var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (Url.IsLocalUrl(ReturnUrl) && !String.IsNullOrEmpty(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("index", "home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt");
                    }
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
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
                            mailRequestRepository.Create(mailRequest);
                            return Ok(new { message = "User registered successfully, verify your account with email we sent you." });
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
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError(string.Empty, $"Email {user.Email} is already taken");
                        }
                        else {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

            }
          
            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return StatusCode(406, new { message = "User id or token id value does not exist." });
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return StatusCode(406, new { message = $"User with Id = {userId} is not found." });
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok(new { message = $"{user.Email} is successfully confirmed." });
            }
            else
            {
                return StatusCode(400, new { message = $"failed to confirm {user.Email}" } );
            }
        }

    }
}
