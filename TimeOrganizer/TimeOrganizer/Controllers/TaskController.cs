using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.SqlRepository;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.ViewModel;

namespace TimeOrganizer.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private ITaskRepository taskRepository;
        private IColorRepository colorRepository;
        private ITaskTypeRepository taskTypeRepository;

        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager, IColorRepository colorRepository, ITaskTypeRepository taskTypeRepository)
        {
            this.taskRepository = taskRepository;
            this.userManager = userManager;
            this.colorRepository = colorRepository;
            this.taskTypeRepository = taskTypeRepository;
        }

        [Route("task/read")]
        public async Task<IActionResult> ReadTask(DateTime startTime, DateTime endTime) {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var data = taskRepository.GetTask(user.Id, startTime, endTime);
            return new JsonResult(data);
        }

        [HttpPost]
        [Route("task/create")]
        public async Task<IActionResult> CreateTask(CreateTaskViewModel createTaskViewModel)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            createTaskViewModel.TaskCreatorId = user.Id;

            if (ModelState.IsValid) {
               
                if (colorRepository.GetColorById(createTaskViewModel.ColorId) == null)
                {
                    ModelState.AddModelError(string.Empty, $"Color with id = {createTaskViewModel.ColorId} does not exist.");
                }
                else if (taskTypeRepository.GetTaskTypeById(createTaskViewModel.TaskTypeId) == null) {
                    ModelState.AddModelError(string.Empty, $"Task type with id = {createTaskViewModel.TaskTypeId} does not exist.");
                }
                else {
                    try
                    {
                        taskRepository.Create(createTaskViewModel);
                        return new JsonResult(new { message = "task created successfully" });
                        //return new JsonResult(new { task = task});
                    }
                    catch (Exception exp) {
                        ModelState.AddModelError(string.Empty, exp.Message);
                    }
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }
    }
}
