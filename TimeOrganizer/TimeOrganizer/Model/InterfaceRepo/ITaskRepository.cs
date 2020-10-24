using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Dto;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface ITaskRepository
    {
        public IEnumerable<TaskDto> GetTask(string authorId, DateTime startTime, DateTime endTime);
    }
}
