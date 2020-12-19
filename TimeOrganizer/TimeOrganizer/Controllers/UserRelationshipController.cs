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
        public async Task<IActionResult> SendFriendRequest(string recivingUserId)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user != null && recivingUserId != null)
            {
                try
                {
                    bool result = userRelationshipRepository.SendRequest(user.Id, recivingUserId);

                    if (result)
                    {
                        return Ok(new { message = "request sent successfully" });
                    }
                    else
                    {
                        return StatusCode(400, new { message = "failed to send friend request" });
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            else
            {
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User is null");
                }
                if (recivingUserId == null)
                {
                    ModelState.AddModelError(string.Empty, "recivingUserId value is null");
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
        }

        //Get list of sent request that are pending 
        [HttpGet]
        [Route("requests/read/sent")]
        public async Task<IActionResult> ReadSentFriendRequests()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                try
                {
                    IEnumerable<ApplicationUserDto> userList = userRelationshipRepository.ReadSentRequests(user.Id);
                    return Ok(new { userList = userList });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
        }

        //Get list of recived requests that are pending 
        [HttpGet]
        [Route("requests/read/recived")]
        public async Task<IActionResult> ReadRecivedFriendRequests()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                try
                {
                    IEnumerable<ApplicationUserDto> userList = userRelationshipRepository.ReadRecivedRequests(user.Id);
                    return Ok(new { userList = userList });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
        }

        //Accept request
        [HttpPost]
        [Route("request/accept")]
        public async Task<IActionResult> AcceptFriendRequest(string sendingUserId)
        {
            var recivingUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (recivingUser.Id != null && sendingUserId != null)
            {
                try
                {
                    var result = userRelationshipRepository.AcceptRequest(sendingUserId, recivingUser.Id);

                    if (result)
                    {
                        return Ok(new { message = "Friend request accepted successfully" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to accept request");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }

            }
            else
            {
                if (recivingUser == null)
                {
                    ModelState.AddModelError(string.Empty, "recivingUser is null");
                }
                if (sendingUserId == null)
                {
                    ModelState.AddModelError(string.Empty, "sendingUserId value is null");
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
        }

        //Reject request
        [HttpPost]
        [Route("request/reject")]
        public async Task<IActionResult> RejectFriendRequest(string sendingUserId)
        {
            var recivingUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (recivingUser.Id != null && sendingUserId != null) {
                try
                {
                    var result = userRelationshipRepository.RejectRequest(sendingUserId, recivingUser.Id);

                    if (result)
                    {
                        return Ok(new { message = "Friend request rejected successfully" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to reject request");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            else
            {
                if (recivingUser == null)
                {
                    ModelState.AddModelError(string.Empty, "recivingUser is null");
                }
                if (sendingUserId == null)
                {
                    ModelState.AddModelError(string.Empty, "sendingUserId value is null");
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
        }

        //Read all accepted requests aka list of friends
        [HttpGet]
        [Route("requests/accepted")]
        public async Task<IActionResult> ReadAcceptedRequests() {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                try
                {
                    IEnumerable<ApplicationUserDto> userList = userRelationshipRepository.ReadAcceptedRequests(user.Id);
                    return Ok(new { userList = userList });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { message = invalidModelStateError });
        }

        [HttpPost]
        [Route("request/remove")]
        public async Task<IActionResult> RemoveAcceptedRelationship(string userIdToDelete)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (currentUser.Id != null && userIdToDelete != null)
            {
                try
                {
                    var result = userRelationshipRepository.DeleteAcceptedRequest(currentUser.Id, userIdToDelete);

                    if (result)
                    {
                        return Ok(new { message = "Friend removed successfully" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to remove friend");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            else
            {
                if (currentUser == null)
                {
                    ModelState.AddModelError(string.Empty, "currentUser is null");
                }
                if (userIdToDelete == null)
                {
                    ModelState.AddModelError(string.Empty, "userIdToDelete value is null");
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return StatusCode(406, new { errors = invalidModelStateError });
        }
    }
}
