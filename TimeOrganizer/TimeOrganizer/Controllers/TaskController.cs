using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Controllers
{
    public class TaskController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private ITaskRepository taskRepository;

        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            this.taskRepository = taskRepository;
            this.userManager = userManager;

        }

        [Authorize]
        [Route("task/read")]
        public async Task<IActionResult> ReadTask(DateTime startTime, DateTime endTime) {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var data = taskRepository.GetTask(user.Id, startTime, endTime);
            return new JsonResult(data);
        }
    }
}
