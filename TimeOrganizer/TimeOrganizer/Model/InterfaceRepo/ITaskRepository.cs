using System;
using System.Collections.Generic;
using System.Linq;
using TimeOrganizer.Model.Dto;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.ViewModel;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface ITaskRepository
    {
        public Task ReadSingle(string applicationUserId, int taskId);
        public IEnumerable<TaskDto> ReadList(string applicationUserId, DateTime startTime, DateTime endTime, int excludeTaskId = -1);
        public IList<DateGroupByDto> ReadByDate(string applicationUserId, DateTime startTime, DateTime endTime);
        public Task Create(CreateTaskViewModel createTaskViewModel);
        public bool CheckDateBounds(IEnumerable<TaskDto> tasks, DateTime taskStartTime, DateTime taskEndTime);
        public Task Update(UpdateTaskViewModel updateTaskViewModel);
        public Task Delete(string userId, int taskId);
        public ApplicationUserTask InviteToTask(string sendingUserId, string recivingUserId, int taskId);
    }
}
