using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface IUserRelationshipRepository 
    {
        public bool SendRequest(string sendingUserId, string recivingUserId);
    }
}
