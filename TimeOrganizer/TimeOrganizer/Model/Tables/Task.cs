using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.Tables
{
    public class Task
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }
        public List<ApplicationUserTask> ApplicationUserTasks { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
