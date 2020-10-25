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
        public IEnumerable<TaskDto> GetTask(string authorId, DateTime startTime, DateTime endTime);
        public Task Create(CreateTaskViewModel createTaskViewModel);
    }
}
