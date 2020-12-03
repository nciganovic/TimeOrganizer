using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Dto;

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

            if (user != null && recivingUserId != null)
            {
                try
                {
                    bool result = userRelationshipRepository.SendRequest(user.Id, recivingUserId);

                    if (result)
                    {
                        return new JsonResult(new { message = "success" });
                    }
                    else
                    {
                        return new JsonResult(new { message = "failed to send friend request" });
                    }
                }
                catch (Exception e) {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            else {
                if (user == null) {
                    ModelState.AddModelError(string.Empty, "User is null");
                }
                if (recivingUserId == null) {
                    ModelState.AddModelError(string.Empty, "recivingUserId value is null");
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }

        //Get list of sent request that are pending 
        [HttpGet]
        [Route("requests/read/sent")]
        public async Task<IActionResult> ReadSentFriendRequests() {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user != null) {
                IEnumerable<ApplicationUserDto> userList = userRelationshipRepository.ReadSentRequests(user.Id);
                return new JsonResult(new { userList = userList });
            }

            var invalidModelStateErrror = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateErrror });
        }

        //TOOD get list of recived requests that are pending 
        //TODO accept request
        //TODO reject request

    }
}
