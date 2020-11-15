using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;
using System.Linq;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlUserRelationshipRepository : IUserRelationshipRepository
    {
        private AppDbContext appDbContext;

        public SqlUserRelationshipRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public bool SendRequest(string sendingUserId, string recivingUserId)
        {
            RelationshipStatus relationshipStatus = appDbContext.RelationshipStatuses.Where(x => x.Name == "Pending").FirstOrDefault();

            UserRelationship userRelationship = new UserRelationship
            {
                ApplicationUserId_Sender = sendingUserId,
                ApplicationUserId_Reciver = recivingUserId,
                RelationshipStatusId = relationshipStatus.Id
            };

            appDbContext.UserRelationships.Add(userRelationship);
            appDbContext.SaveChanges();
            
            return true;
        }
    }
}
