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
                try
                {
                    IEnumerable<ApplicationUserDto> userList = userRelationshipRepository.ReadSentRequests(user.Id);
                    return new JsonResult(new { userList = userList });
                }
                catch (Exception e) {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }

        //Get list of recived requests that are pending 
        [HttpGet]
        [Route("requests/read/recived")]
        public async Task<IActionResult> ReadRecivedFriendRequests() {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user != null) {
                try
                {
                    IEnumerable<ApplicationUserDto> userList = userRelationshipRepository.ReadRecivedRequests(user.Id);
                    return new JsonResult(new { userList = userList });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }

        //Accept request
        [HttpPost]
        [Route("request/accept")]
        public async Task<IActionResult> AcceptFriendRequest(string sendingUserId) {
            var recivingUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (recivingUser.Id != null && sendingUserId != null)
            {
                try
                {
                    var result = userRelationshipRepository.AcceptRequest(sendingUserId, recivingUser.Id);

                    if (result)
                    {
                        return new JsonResult(new { message = "Friend request accepted successfully" });
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "Failed to accept request");
                    }
                }
                catch (Exception e) {
                    ModelState.AddModelError(string.Empty, e.Message);
                }

            }
            else {
                if (recivingUser == null)
                {
                    ModelState.AddModelError(string.Empty, "User is null");
                }
                if (sendingUserId == null)
                {
                    ModelState.AddModelError(string.Empty, "sendingUserId value is null");
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }

        //TODO reject request

    }
}
