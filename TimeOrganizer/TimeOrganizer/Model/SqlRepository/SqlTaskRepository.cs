using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TimeOrganizer.Model.Dto;
using TimeOrganizer.Model.InterfaceRepo;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlTaskRepository : ITaskRepository
    {
        private AppDbContext appDbContext;

        public SqlTaskRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
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
                    SearchingUserId = y.ApplicationUserId
                })
                .Where(x => x.SearchingUserId == searchingUserId && startTime <= x.StartTime && endTime >= x.EndTime)
                .ToList();

            return data;
        }
    }
}
