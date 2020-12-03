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

        public bool AcceptRequest(string sendingUserId, string recivingUserId)
        {
            var request = appDbContext.UserRelationships
                .Where(x => x.ApplicationUserId_Sender == sendingUserId 
                && x.ApplicationUserId_Reciver == recivingUserId
                && x.RelationshipStatus.Name == "Pending")
                .FirstOrDefault();

            if (request == null)
            {
                throw new Exception($"Pending request between {sendingUserId} and {recivingUserId} does not exist");
            }
            
            RelationshipStatus acceptStatus = appDbContext.RelationshipStatuses
                .Where(x => x.Name == "Accepted")
                .FirstOrDefault();

            request.RelationshipStatusId = acceptStatus.Id;

            var editRequest = appDbContext.UserRelationships.Attach(request);
            editRequest.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            appDbContext.SaveChanges();

            return true;
        }

        public bool DeleteAcceptedRequest(string userId_1, string userId_2)
        {
            var requests = appDbContext.UserRelationships
                .Where(x => (x.ApplicationUserId_Sender == userId_1 && x.ApplicationUserId_Reciver == userId_2)
                || (x.ApplicationUserId_Sender == userId_2 && x.ApplicationUserId_Reciver == userId_1)
                && x.RelationshipStatus.Name == "Accepted")
                .FirstOrDefault();

            if (requests == null) {
                throw new Exception($"Relationship between {userId_1} and {userId_2} does not exist");
            }

            appDbContext.UserRelationships.Remove(requests);
            appDbContext.SaveChanges();

            return true;
        }

        public IEnumerable<ApplicationUserDto> ReadAcceptedRequests(string userId)
        {
            var requests = appDbContext.UserRelationships
                .Where(x => (x.ApplicationUserId_Reciver == userId 
                || x.ApplicationUserId_Sender == userId)
                && x.RelationshipStatus.Name == "Accepted").ToList();

            List<ApplicationUserDto> userList = new List<ApplicationUserDto>();

            foreach(var r in requests)
            {
                if (r.ApplicationUserId_Sender != userId)
                {
                    ApplicationUserDto applicationUser = new ApplicationUserDto
                    {
                        Id = r.ApplicationUserId_Sender,
                        Username = appDbContext.ApplicationUsers.Find(r.ApplicationUserId_Sender).UserName
                    };
                    userList.Add(applicationUser);
                }
                else {
                    //For some reason r.ApplicaitonUser_Reciver returns null
                    
                    ApplicationUserDto applicationUser = new ApplicationUserDto
                    {
                        Id = r.ApplicationUserId_Reciver,
                        Username = appDbContext.ApplicationUsers.Find(r.ApplicationUserId_Reciver).UserName
                    };
                    userList.Add(applicationUser);
                }
            }

            return userList;
        }

        public IEnumerable<ApplicationUserDto> ReadRecivedRequests(string userId)
        {
            var requests = appDbContext.UserRelationships
                .Where(x => x.ApplicationUserId_Reciver == userId 
                && x.RelationshipStatus.Name == "Pending");

            var data = requests.Select(x => new ApplicationUserDto
            {
                Id = x.ApplicationUser_Sender.Id,
                Username = x.ApplicationUser_Sender.UserName
            }).ToList();

            return data;
        }

        public IEnumerable<ApplicationUserDto> ReadSentRequests(string userId)
        {
            var requests = appDbContext.UserRelationships
                .Where(x => x.ApplicationUserId_Sender == userId 
                && x.RelationshipStatus.Name == "Pending");
            
            var data = requests.Select(x => new ApplicationUserDto { 
                Id = x.ApplicationUser_Reciver.Id,
                Username = x.ApplicationUser_Reciver.UserName
            }).ToList();

            return data;
        }

        public bool RejectRequest(string sendingUserId, string recivingUserId)
        {
            var request = appDbContext.UserRelationships
                .Where(x => x.ApplicationUserId_Sender == sendingUserId 
                && x.ApplicationUserId_Reciver == recivingUserId 
                && x.RelationshipStatus.Name == "Pending")
                .FirstOrDefault();

            if (request == null)
            {
                throw new Exception($"Pending request between {sendingUserId} and {recivingUserId} does not exist");
            }

            appDbContext.UserRelationships.Remove(request);
            appDbContext.SaveChanges();

            return true;
        }

        public bool SendRequest(string sendingUserId, string recivingUserId)
        {
            //Check if user is sending request to himself
            if (sendingUserId == recivingUserId)
            {
                throw new Exception("You cannot send yourself request");
            }

            //Check if this realtionship already exist but in reverse
            var checkIfRelationshipExists = appDbContext.UserRelationships
                .Where(x => x.ApplicationUserId_Reciver == sendingUserId
                && x.ApplicationUserId_Sender == recivingUserId)
                .FirstOrDefault();

            if (checkIfRelationshipExists != null) {
                throw new Exception("Relationship already exists");
            }

            RelationshipStatus pendingStatus = appDbContext.RelationshipStatuses.Where(x => x.Name == "Pending").FirstOrDefault();

            UserRelationship userRelationship = new UserRelationship
            {
                ApplicationUserId_Sender = sendingUserId,
                ApplicationUserId_Reciver = recivingUserId,
                RelationshipStatusId = pendingStatus.Id
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
