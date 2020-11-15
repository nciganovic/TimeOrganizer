using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.Model.InterfaceRepo;

namespace TimeOrganizer.Controllers
{
    [Authorize]
    public class UserRelationshipController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IUserRelationshipRepository userRelationshipRepository;

        public UserRelationshipController(UserManager<ApplicationUser> userManager, IUserRelationshipRepository userRelationshipRepository)
        {
            this.userManager = userManager;
            this.userRelationshipRepository = userRelationshipRepository;
        }

        [HttpPost]
        [Route("request/send")]
        public async Task<IActionResult> SendFriendRequest(string recivingUserId) {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            userRelationshipRepository.SendRequest(user.Id, recivingUserId);
            return new JsonResult(new { message = "success" });
        }

    }
}
