using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.Model.Dto;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface IApplicationUserTaskRepository
    {
        public ApplicationUserTask Create(string userId, int taskId);
        public IEnumerable<TaskDto> ReadInvites(string userId);
        public ApplicationUserTask AcceptTaskInvite(string userId, int taskId);
        public ApplicationUserTask RejectTaskInvite(string userId, int taskId);
    }
}
