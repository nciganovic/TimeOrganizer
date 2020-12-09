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
        //TODO Renaming methods Reading and GroupBy
        private AppDbContext appDbContext;
        private int acceptedStatusId;
        private int pendingStatusId;

        public SqlTaskRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            acceptedStatusId = appDbContext.RelationshipStatuses.Where(x => x.Name == "Accepted").FirstOrDefault().Id;
            pendingStatusId = appDbContext.RelationshipStatuses.Where(x => x.Name == "Pending").FirstOrDefault().Id;
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

        public Task Read(string applicationUserId, int taskId)
        {
            ApplicationUserTask appTask = appDbContext.ApplicationUserTask
                .Where(x => x.TaskId == taskId 
                && x.ApplicationUserId == applicationUserId 
                && x.RelationshipStatusId == acceptedStatusId)
                .FirstOrDefault();

            if (appTask != null)
            {
                var task = appDbContext.Tasks.Find(taskId);
                return task;
            }
            else {
                return null; //TODO try to throw new exception and remove on others like Delete()
            }

            
        }

        public IEnumerable<TaskDto> Read(string applicationUserId, DateTime startTime, DateTime endTime, int excludeTaskId = -1)
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
                    ApplicationUserId = y.ApplicationUserId,
                    TaskCreatorUsername = x.ApplicationUser.UserName,
                    TotalUsersCount = x.ApplicationUserTasks.Count(),
                    RelationshipStatusId = y.RelationshipStatusId
                })
                .Where(x => x.ApplicationUserId == applicationUserId && startTime <= x.StartTime && endTime >= x.EndTime && x.Id != excludeTaskId && x.RelationshipStatusId == acceptedStatusId )
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

        public Task Delete(string userId, int taskId)
        {
            Task task = Read(userId, taskId);

            if (task != null)
            {
                //check if author or other user is deleting task

                ApplicationUserTask applicationUserTask = appDbContext.ApplicationUserTask
                    .Where(x => x.ApplicationUserId == userId 
                    && x.TaskId == taskId)
                    .FirstOrDefault();
                appDbContext.ApplicationUserTask.Remove(applicationUserTask);
                appDbContext.SaveChanges();

                ApplicationUserTask checkIfAnyoneHasTask = appDbContext.ApplicationUserTask
                    .Where(x => x.TaskId == taskId)
                    .FirstOrDefault();

                if (checkIfAnyoneHasTask == null) {
                    appDbContext.Tasks.Remove(task);
                    appDbContext.SaveChanges();
                }

            }
            else {
                throw new Exception($"Task with id = {taskId} does not exist");
            }

            return task;
        }

        public IList<DateGroupByDto> ReadGroupByDateExtended(string applicationUserId, DateTime startTime, DateTime endTime)
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
                    ApplicationUserId = y.ApplicationUserId,
                    TaskCreatorUsername = x.ApplicationUser.UserName,
                    RelationshipStatusId = y.RelationshipStatusId
                })
                .Where(x => x.ApplicationUserId == applicationUserId && startTime <= x.StartTime && endTime >= x.EndTime && x.RelationshipStatusId == acceptedStatusId)
                .OrderBy(x => x.StartTime)
                .ToList();

            IList<DateGroupByDto> list = data
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    ColorName = x.ColorName,
                    ApplicationUserId = x.ApplicationUserId,
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
                        ApplicationUserId = y.ApplicationUserId,
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

        public IList<DateGroupByDto> ReadGroupByDate(string applicationUserId, DateTime startTime, DateTime endTime)
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
                    ApplicationUserId = y.ApplicationUserId,
                    TaskCreatorUsername = x.ApplicationUser.UserName,
                    RelationshipStatusId = y.RelationshipStatusId
                })
                .Where(x => x.ApplicationUserId == applicationUserId && startTime <= x.StartTime && endTime >= x.EndTime && x.RelationshipStatusId == acceptedStatusId)
                .OrderBy(x => x.StartTime)
                .ToList();

            IList<DateGroupByDto> list = data
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    ColorName = x.ColorName,
                    ApplicationUserId = x.ApplicationUserId,
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

        public ApplicationUserTask InviteToTask(string sendingUserId, string recivingUserId, int taskId)
        {
            var relationship = appDbContext.UserRelationships
                .Where(x => ((x.ApplicationUserId_Sender == sendingUserId
                && x.ApplicationUserId_Reciver == recivingUserId)
                || (x.ApplicationUserId_Sender == recivingUserId
                && x.ApplicationUserId_Reciver == sendingUserId))
                && x.RelationshipStatusId == acceptedStatusId);

            if (relationship != null)
            {
                ApplicationUserTask aut = new ApplicationUserTask
                {
                    TaskId = taskId,
                    ApplicationUserId = recivingUserId,
                    RelationshipStatusId = pendingStatusId
                };

                Task task = Read(sendingUserId, taskId);

                if (task != null) {
                    DateTime startOfADay = task.StartTime.Date;
                    DateTime endOfADay = task.StartTime.Date.AddDays(1);

                    IEnumerable<TaskDto> tasks = Read(recivingUserId, startOfADay, endOfADay);

                    if (CheckDateBounds(tasks, task.StartTime, task.EndTime))
                    {
                        appDbContext.ApplicationUserTask.Add(aut);
                        appDbContext.SaveChanges();

                        return aut;
                    }
                    else
                    {
                        throw new Exception($"User with id {recivingUserId} has something already scheculed during {task.StartTime} and {task.EndTime}");
                    }
                }
                else{
                    throw new Exception($"Task with {taskId} does not exist.");
                }
                
            }
            else {
                throw new Exception($"No relationship between {sendingUserId} and {recivingUserId}");
            }
        }
    }
}
