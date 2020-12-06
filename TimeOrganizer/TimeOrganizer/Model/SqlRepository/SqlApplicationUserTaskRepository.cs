using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Dto;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlApplicationUserTaskRepository : IApplicationUserTaskRepository
    {
        private AppDbContext appDbContext;
        private int acceptedStatusId;
        private int pendingStatusId;

        public SqlApplicationUserTaskRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            acceptedStatusId = appDbContext.RelationshipStatuses.Where(x => x.Name == "Accepted").FirstOrDefault().Id;
            pendingStatusId = appDbContext.RelationshipStatuses.Where(x => x.Name == "Pending").FirstOrDefault().Id;
        }

        public ApplicationUserTask AcceptTaskInvite(string userId, int taskId)
        {
            ApplicationUserTask invite = appDbContext.ApplicationUserTask
                .Where(x => x.ApplicationUserId == userId
                && x.TaskId == taskId
                && x.RelationshipStatusId == pendingStatusId).FirstOrDefault();

            if (invite != null)
            {
                invite.RelationshipStatusId = acceptedStatusId;
                
                var edit = appDbContext.ApplicationUserTask.Attach(invite);
                edit.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                appDbContext.SaveChanges();

                return invite;
            }
            else {
                throw new Exception($"Task with Id = {taskId} , ApplicationUser Id = {userId} and pending status does not exist.");
            }

        }

        public ApplicationUserTask Create(string userId, int taskId)
        {
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

        public IEnumerable<TaskDto> ReadInvites(string userId)
        {
            var invites = appDbContext.ApplicationUserTask
                .Where(x => x.ApplicationUserId == userId 
                && x.RelationshipStatusId == pendingStatusId);

            var data = invites.Select(x => new TaskDto
            {
                Id = x.Task.Id,
                StartTime = x.Task.StartTime,
                EndTime = x.Task.EndTime,
                RelationshipStatusId = x.RelationshipStatusId,
                ApplicationUserId = x.Task.ApplicationUserId,
                Title = x.Task.Title,
                Description = x.Task.Description,
                TaskCreatorUsername = x.ApplicationUser.UserName,
                TotalUsersCount = x.Task.ApplicationUserTasks.Count(),
                TaskTypeName = x.Task.TaskType.Name
            }).ToList();

            return data;
        }

        public ApplicationUserTask RejectTaskInvite(string userId, int taskId)
        {
            ApplicationUserTask invite = appDbContext.ApplicationUserTask
                .Where(x => x.ApplicationUserId == userId
                && x.TaskId == taskId
                && x.RelationshipStatusId == pendingStatusId).FirstOrDefault();

            if (invite != null)
            {
                appDbContext.ApplicationUserTask.Remove(invite);
                appDbContext.SaveChanges();
                return invite;
            }
            else
            {
                throw new Exception($"Task with Id = {taskId} , ApplicationUser Id = {userId} and pending status does not exist.");
            }
        }
    }
}
