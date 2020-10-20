﻿using Microsoft.AspNetCore.Mvc;
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

namespace TimeOrganizer.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private ISchoolRepository schoolRepository;

        public AccountController(UserManager<ApplicationUser> userManager, ISchoolRepository schoolRepository)
        {
            this.userManager = userManager;
            this.schoolRepository = schoolRepository;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login()
        {
            JsonResult jsonResult = new JsonResult(new { });
            return jsonResult;
        }

        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                //Check if school id exists
                var school = schoolRepository.GetSchoolById(registerViewModel.SchoolId);

                if (school == null) {
                    return new JsonResult(new { errors = new { errorMessage = "This school does not exist." } });
                }

                var user = new ApplicationUser
                {
                    UserName = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Email = registerViewModel.Email,
                    SchoolId = registerViewModel.SchoolId, //check if school id exists
                    EmailConfirmed = true //auto confirm email for now 
                };

                var result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    return new JsonResult(new { Message = "success" });
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
    }
}
