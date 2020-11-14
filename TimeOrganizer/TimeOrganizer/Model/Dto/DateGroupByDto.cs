using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.Dto
{
    public class DateGroupByDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public IEnumerable<TaskDto> Tasks { get; set; }
    }
}
