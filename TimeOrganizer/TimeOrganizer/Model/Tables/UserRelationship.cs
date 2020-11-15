using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.Tables
{
    public class UserRelationship
    {
        public string ApplicationUserId_Sender { get; set; }
        
        public virtual ApplicationUser ApplicationUser_Sender { get; set; }
        
        
        public string ApplicationUserId_Reciver { get; set; }
        
        public virtual ApplicationUser ApplicationUser_Reciver { get; set; }
        
        public int RelationshipStatusId { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
    }
}
