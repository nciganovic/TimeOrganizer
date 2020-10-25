using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlTaskTypeRepository : ITaskTypeRepository
    {
        private AppDbContext appDbContext;

        public SqlTaskTypeRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public TaskType GetTaskTypeById(int id)
        {
            var taskType = appDbContext.TaskTypes.Find(id);
            return taskType;
        }
    }
}
