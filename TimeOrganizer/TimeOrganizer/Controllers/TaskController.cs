using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using TimeOrganizer.Model.Dto;
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
        private IApplicationUserTaskRepository applicationUserTaskRepository;

        public TaskController(ITaskRepository taskRepository, 
            UserManager<ApplicationUser> userManager, 
            IColorRepository colorRepository, 
            ITaskTypeRepository taskTypeRepository,
            IApplicationUserTaskRepository applicationUserTaskRepository)
        {
            this.taskRepository = taskRepository;
            this.userManager = userManager;
            this.colorRepository = colorRepository;
            this.taskTypeRepository = taskTypeRepository;
            this.applicationUserTaskRepository = applicationUserTaskRepository;
        }

        [Route("task/read")]
        public async Task<IActionResult> ReadTask(DateTime startTime, DateTime endTime) {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var data = taskRepository.Read(user.Id, startTime, endTime);
            return new JsonResult(data);
        }

        [HttpPost]
        [Route("task/create")]
        public async Task<IActionResult> CreateTask(CreateTaskViewModel createTaskViewModel)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            createTaskViewModel.TaskCreatorId = user.Id;

            if (ModelState.IsValid) {

                IEnumerable<TaskDto> tasksForCurrentDay = GetAllTasksForCurrentDay(createTaskViewModel.StartTime, user.Id);

                if (!taskRepository.CheckDateBounds(tasksForCurrentDay, createTaskViewModel.StartTime, createTaskViewModel.EndTime)) {
                    ModelState.AddModelError(string.Empty, $"Time bounds are not valid");
                }
                else if (colorRepository.GetColorById(createTaskViewModel.ColorId) == null)
                {
                    ModelState.AddModelError(string.Empty, $"Color with id = {createTaskViewModel.ColorId} does not exist.");
                }
                else if (taskTypeRepository.GetTaskTypeById(createTaskViewModel.TaskTypeId) == null) {
                    ModelState.AddModelError(string.Empty, $"Task type with id = {createTaskViewModel.TaskTypeId} does not exist.");
                }
                else {
                    try
                    {
                        var task = taskRepository.Create(createTaskViewModel);
                        applicationUserTaskRepository.Create(task.ApplicationUserId, task.Id);
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

        private IEnumerable<TaskDto> GetAllTasksForCurrentDay(DateTime date, string userId) 
        {
            DateTime currentDayStart = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime currentDayEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            IEnumerable<TaskDto> tasksForCurrentDay = taskRepository.Read(userId, currentDayStart, currentDayEnd);
            
            return tasksForCurrentDay;
        }
        
        [HttpPost]
        [Route("task/update")]
        public async Task<IActionResult> UpdateTask(UpdateTaskViewModel updateTaskViewModel) {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            updateTaskViewModel.TaskCreatorId = user.Id;

            if (ModelState.IsValid) {

                IEnumerable<TaskDto> tasksForCurrentDay = GetAllTasksForCurrentDay(updateTaskViewModel.StartTime, user.Id);

                if (!taskRepository.CheckDateBounds(tasksForCurrentDay, updateTaskViewModel.StartTime, updateTaskViewModel.EndTime))
                {
                    ModelState.AddModelError(string.Empty, $"Time bounds are not valid");
                }
                else if (colorRepository.GetColorById(updateTaskViewModel.ColorId) == null)
                {
                    ModelState.AddModelError(string.Empty, $"Color with id = {updateTaskViewModel.ColorId} does not exist.");
                }
                else if (taskTypeRepository.GetTaskTypeById(updateTaskViewModel.TaskTypeId) == null)
                {
                    ModelState.AddModelError(string.Empty, $"Task type with id = {updateTaskViewModel.TaskTypeId} does not exist.");
                }
                else
                {
                    try
                    {
                        var task = taskRepository.Update(updateTaskViewModel);
                        return new JsonResult(new { message = "task updated successfully" });                                                                                                      
                    }
                    catch (Exception exp)
                    {
                        ModelState.AddModelError(string.Empty, exp.Message);
                    }
                }
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }

        [HttpPost]
        [Route("task/delete")]
        public async Task<IActionResult> DeleteTask(int id) 
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var task = taskRepository.Read(user.Id, id);

            if (task != null)
            {
                try
                {
                    taskRepository.Delete(id);
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError(string.Empty, exp.Message);
                }
            }
            else {
                ModelState.AddModelError(string.Empty, $"Task with id = {id} and userId = {user.Id} does not exist");
            }

            var invalidModelStateError = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return new JsonResult(new { errors = invalidModelStateError });
        }

    }
}
