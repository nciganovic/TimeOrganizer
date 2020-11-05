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
        public Task Read(string authorId, int taskId);
        public IEnumerable<TaskDto> Read(string authorId, DateTime startTime, DateTime endTime);
        public Task Create(CreateTaskViewModel createTaskViewModel);
        public bool CheckDateBounds(IEnumerable<TaskDto> tasks, DateTime taskStartTime, DateTime taskEndTime);
        public Task Update(UpdateTaskViewModel updateTaskViewModel);
        public Task Delete(int id);
    }
}
