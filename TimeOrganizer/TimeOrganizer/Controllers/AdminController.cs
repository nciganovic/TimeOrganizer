using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TimeOrganizer.Controllers
{
    [Authorize]
    [Route("admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;

        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
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
    }
}
