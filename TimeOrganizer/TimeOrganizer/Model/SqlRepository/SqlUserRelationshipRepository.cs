using System;
using System.Linq;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

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

            try
            {
                appDbContext.UserRelationships.Add(userRelationship);
                appDbContext.SaveChanges();
            }
            catch(Exception exp) {
                //Log exception
                return false;
            }

            return true;
        }
    }
}
