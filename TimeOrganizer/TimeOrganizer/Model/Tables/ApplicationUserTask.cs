using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.Tables
{
    public class ApplicationUserTask
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public int RelationshipStatusId { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
    }
}
