using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface ITaskTypeRepository
    {
        public TaskType GetTaskTypeById(int id);
    }
}
