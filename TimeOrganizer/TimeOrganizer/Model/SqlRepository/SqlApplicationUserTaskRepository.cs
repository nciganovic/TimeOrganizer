using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlApplicationUserTaskRepository : IApplicationUserTaskRepository
    {
        private AppDbContext appDbContext;
        public SqlApplicationUserTaskRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;        
        }

        public ApplicationUserTask Create(string userId, int taskId)
        {
            int acceptedStatusId = appDbContext.RelationshipStatuses.Where(x => x.Name == "Accepted").FirstOrDefault().Id;

            ApplicationUserTask aut = new ApplicationUserTask
            {
                TaskId = taskId,
                ApplicationUserId = userId,
                RelationshipStatusId = acceptedStatusId
            };

            appDbContext.ApplicationUserTask.Add(aut);
            appDbContext.SaveChanges();

            return aut;
        }
    }
}
