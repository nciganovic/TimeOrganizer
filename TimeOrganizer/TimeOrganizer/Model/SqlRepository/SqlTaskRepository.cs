using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using TimeOrganizer.Model.Dto;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.ViewModel;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlTaskRepository : ITaskRepository
    {
        private AppDbContext appDbContext;

        public SqlTaskRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Task Create(CreateTaskViewModel createTaskViewModel)
        {
            Task task = new Task() {
                StartTime = createTaskViewModel.StartTime,
                EndTime = createTaskViewModel.EndTime,
                Title = createTaskViewModel.Title,
                Description = createTaskViewModel.Description,
                TaskTypeId = createTaskViewModel.TaskTypeId,
                ColorId = createTaskViewModel.ColorId,
                Priority = createTaskViewModel.Priority,
                ApplicationUserId = createTaskViewModel.TaskCreatorId,
            };

            appDbContext.Tasks.Add(task);
            appDbContext.SaveChanges();
            return task;
        }

        public Task Read(string applicaitonUserId, int taskId)
        {
            ApplicationUserTask appTask = appDbContext.ApplicationUserTask.Where(x => x.TaskId == taskId && x.ApplicationUserId == applicaitonUserId).FirstOrDefault();

            if (appTask != null)
            {
                var task = appDbContext.Tasks.Find(taskId);
                return task;
            }
            else {
                return null;
            }

            
        }

        public IEnumerable<TaskDto> Read(string searchingUserId, DateTime startTime, DateTime endTime, int excludeTaskId = -1)
        {
            var data = appDbContext.Tasks.Join(appDbContext.ApplicationUserTask, 
                x => x.Id, 
                y => y.TaskId, 
                (x, y) => new TaskDto { 
                    Id = x.Id,
                    ColorName = x.Color.Name,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Description = x.Description,
                    Priority = x.Priority,
                    TaskTypeName = x.TaskType.Name,
                    Title = x.Title,
                    SearchingUserId = y.ApplicationUserId,
                    TaskCreatorUsername = x.ApplicationUser.UserName
                })
                .Where(x => x.SearchingUserId == searchingUserId && startTime <= x.StartTime && endTime >= x.EndTime && x.Id != excludeTaskId)
                .OrderBy(x => x.StartTime)
                .ToList();

            return data;
        }

        public bool CheckDateBounds(IEnumerable<TaskDto> tasks, DateTime taskStartTime, DateTime taskEndTime) {
            bool isTaskTimeValid = true;

            foreach (var task in tasks) {
                if (taskStartTime >= task.StartTime && taskStartTime < task.EndTime) {
                    isTaskTimeValid = false;
                    break;
                }
                if (taskEndTime > task.StartTime && taskEndTime <= task.EndTime) {
                    isTaskTimeValid = false;
                    break;
                }
                if (taskStartTime < task.StartTime && taskEndTime > task.EndTime) {
                    isTaskTimeValid = false;
                    break;
                }
            }

            return isTaskTimeValid;
        }

        public Task Update(UpdateTaskViewModel updateTaskViewModel)
        {
            Task task = new Task()
            {
                Id = updateTaskViewModel.Id,
                StartTime = updateTaskViewModel.StartTime,
                EndTime = updateTaskViewModel.EndTime,
                Title = updateTaskViewModel.Title,
                Description = updateTaskViewModel.Description,
                TaskTypeId = updateTaskViewModel.TaskTypeId,
                ColorId = updateTaskViewModel.ColorId,
                Priority = updateTaskViewModel.Priority,
                ApplicationUserId = updateTaskViewModel.TaskCreatorId,
            };

            var editBook = appDbContext.Tasks.Attach(task);
            editBook.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            appDbContext.SaveChanges();
            return task;
        }

        public Task Delete(int id)
        {
            Task task = appDbContext.Tasks.Find(id);

            if (task != null)
            {
                appDbContext.Remove(task);
                appDbContext.SaveChanges();
            }
            else {
                throw new Exception($"Task with id = {id} does not exist");
            }

            return task;
        }

        public IList<DateGroupByDto> ReadGroupByDateExtended(string searchingUserId, DateTime startTime, DateTime endTime)
        {
            var data = appDbContext.Tasks.Join(appDbContext.ApplicationUserTask,
                x => x.Id,
                y => y.TaskId,
                (x, y) => new TaskDto
                {
                    Id = x.Id,
                    ColorName = x.Color.Name,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Description = x.Description,
                    Priority = x.Priority,
                    TaskTypeName = x.TaskType.Name,
                    Title = x.Title,
                    SearchingUserId = y.ApplicationUserId,
                    TaskCreatorUsername = x.ApplicationUser.UserName
                })
                .Where(x => x.SearchingUserId == searchingUserId && startTime <= x.StartTime && endTime >= x.EndTime)
                .OrderBy(x => x.StartTime)
                .ToList();

            IList<DateGroupByDto> list = data
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    ColorName = x.ColorName,
                    SearchingUserId = x.SearchingUserId,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Description = x.Description,
                    Priority = x.Priority,
                    TaskCreatorUsername = x.TaskCreatorUsername,
                    TaskTypeName = x.TaskTypeName,
                    Title = x.Title
                })
                .GroupBy(x => x.StartTime.Date)
                .Select(x => new DateGroupByDto{ 
                    Count = x.Count(),
                    Date = x.Key,
                    Tasks = x.Select(y => new TaskDto {
                        Id = y.Id,
                        ColorName = y.ColorName,
                        SearchingUserId = y.SearchingUserId,
                        StartTime = y.StartTime,
                        EndTime = y.EndTime,
                        Description = y.Description,
                        Priority = y.Priority,
                        TaskCreatorUsername = y.TaskCreatorUsername,
                        TaskTypeName = y.TaskTypeName,
                        Title = y.Title
                    })
                })
                .ToList();

            return list;
        }

        public IList<DateGroupByDto> ReadGroupByDate(string searchingUserId, DateTime startTime, DateTime endTime)
        {
            var data = appDbContext.Tasks.Join(appDbContext.ApplicationUserTask,
                x => x.Id,
                y => y.TaskId,
                (x, y) => new TaskDto
                {
                    Id = x.Id,
                    ColorName = x.Color.Name,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Description = x.Description,
                    Priority = x.Priority,
                    TaskTypeName = x.TaskType.Name,
                    Title = x.Title,
                    SearchingUserId = y.ApplicationUserId,
                    TaskCreatorUsername = x.ApplicationUser.UserName
                })
                .Where(x => x.SearchingUserId == searchingUserId && startTime <= x.StartTime && endTime >= x.EndTime)
                .OrderBy(x => x.StartTime)
                .ToList();

            IList<DateGroupByDto> list = data
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    ColorName = x.ColorName,
                    SearchingUserId = x.SearchingUserId,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Description = x.Description,
                    Priority = x.Priority,
                    TaskCreatorUsername = x.TaskCreatorUsername,
                    TaskTypeName = x.TaskTypeName,
                    Title = x.Title
                })
                .GroupBy(x => x.StartTime.Date)
                .Select(x => new DateGroupByDto
                {
                    Count = x.Count(),
                    Date = x.Key,
                })
                .ToList();

            return list;
        }
    }
}
