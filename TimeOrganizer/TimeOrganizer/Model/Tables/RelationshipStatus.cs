using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.Tables
{
    public class RelationshipStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserRelationship> UserRelationships { get; set; }
    }
}
