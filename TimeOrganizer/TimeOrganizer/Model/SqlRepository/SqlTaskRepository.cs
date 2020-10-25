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

        public IEnumerable<TaskDto> GetTask(string searchingUserId, DateTime startTime, DateTime endTime)
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
                .Where(x => x.SearchingUserId == searchingUserId && startTime <= x.StartTime && endTime >= x.EndTime)
                .ToList();

            return data;
        }
    }
}
