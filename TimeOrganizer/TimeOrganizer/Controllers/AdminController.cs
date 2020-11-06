using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TimeOrganizer.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("roles/create")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel) {
            if (ModelState.IsValid) 
            {
                IdentityRole identityRole = new IdentityRole { Name = createRoleViewModel.Name };
                IdentityResult identityResult = await roleManager.CreateAsync(identityRole);

                if (identityResult.Succeeded) {
                    return new JsonResult(new { message = $"Role {createRoleViewModel.Name} created successfully" });
                }

                foreach (var error in identityResult.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var invalidModelStateErrror = ModelState.Select(x => x.Value.Errors).Where(y => y.Count() > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateErrror }) ;
        }

        [HttpPost]
        [Route("users/roles/update")]
        public async Task<IActionResult> UpdateUserRoles(List<string> roles, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                //Delete all current roles
                var allRoles = await roleManager.Roles.ToListAsync();
                foreach (var role in allRoles) {
                    var result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }

                foreach (var role in roles) {
                    var result = await userManager.AddToRoleAsync(user, role);
                    
                    if (!result.Succeeded) {
                        ModelState.AddModelError(string.Empty, $"Failed to add {role} to user");                    
                    }
                }
                
            }
            else {
                ModelState.AddModelError(string.Empty, $"User with id {userId} cannot be found.");
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count() > 0).ToList();

            if (invalidModelStateError.Count == 0) {
                return new JsonResult(new { message = "Roles successfully added." });
            }

            return new JsonResult(new { errors = invalidModelStateError });
        }
    }
}
