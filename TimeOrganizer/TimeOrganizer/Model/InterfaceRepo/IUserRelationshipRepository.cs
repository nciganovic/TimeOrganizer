using TimeOrganizer.Model.Tables;
using System.Collections.Generic;
using TimeOrganizer.Model.Dto;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface IUserRelationshipRepository 
    {
        public bool SendRequest(string sendingUserId, string recivingUserId);
        public IEnumerable<ApplicationUserDto> ReadSentRequests(string userId);
    }
}
