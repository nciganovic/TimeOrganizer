using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model;
using TimeOrganizer.Model.InterfaceRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Controllers
{
    public class HomeController : Controller
    { 
        private UserManager<ApplicationUser> userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("/")]
        [Authorize]
        public async Task<IActionResult> Index() {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            
            return new JsonResult(new {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            });
        }
    }
}
