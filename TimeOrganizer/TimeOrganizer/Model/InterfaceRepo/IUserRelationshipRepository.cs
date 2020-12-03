using TimeOrganizer.Model.Tables;
using System.Collections.Generic;
using TimeOrganizer.Model.Dto;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface IUserRelationshipRepository 
    {
        public bool SendRequest(string sendingUserId, string recivingUserId);
        public IEnumerable<ApplicationUserDto> ReadSentRequests(string userId);
        public IEnumerable<ApplicationUserDto> ReadRecivedRequests(string userId);
        public bool AcceptRequest(string sendingUserId, string recivingUserId);
        public bool RejectRequest(string sendingUserId, string recivingUserId);
    }
}
