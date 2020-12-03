using System;
using System.Collections.Generic;
using System.Linq;
using TimeOrganizer.Model.Dto;
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

        public IEnumerable<ApplicationUserDto> ReadRecivedRequests(string userId)
        {
            var requests = appDbContext.UserRelationships.Where(x => x.ApplicationUserId_Reciver == userId);

            var data = requests.Select(x => new ApplicationUserDto
            {
                Id = x.ApplicationUser_Sender.Id,
                Username = x.ApplicationUser_Sender.UserName
            }).ToList();

            return data;
        }

        public IEnumerable<ApplicationUserDto> ReadSentRequests(string userId)
        {
            var requests = appDbContext.UserRelationships.Where(x => x.ApplicationUserId_Sender == userId);
            
            var data = requests.Select(x => new ApplicationUserDto { 
                Id = x.ApplicationUser_Reciver.Id,
                Username = x.ApplicationUser_Reciver.UserName
            }).ToList();

            return data;
        }

        public bool SendRequest(string sendingUserId, string recivingUserId)
        {
            //Check if user is sending request to himself
            if (sendingUserId == recivingUserId)
            {
                throw new Exception("You cannot send yourself request");
            }

            //Check if this realtionship already exist but in reverse
            var checkIfRelationshipExists = appDbContext.UserRelationships.Where(x => x.ApplicationUserId_Reciver == sendingUserId && x.ApplicationUserId_Sender == recivingUserId).FirstOrDefault();
            if (checkIfRelationshipExists != null) {
                throw new Exception("Relationship already exists");
            }

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
