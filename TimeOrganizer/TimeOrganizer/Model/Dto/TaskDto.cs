using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string ColorName { get; set; }
        public string TaskTypeName { get; set; }
        public string SearchingUserId { get; set; }
        public string TaskCreatorUsername { get; set; }
    }
}
