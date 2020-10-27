using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Utilities;

namespace TimeOrganizer.ViewModel
{
    public class CreateTaskViewModel
    {
        [Required]
        [EndGreatedThenStartDate("EndTime", "End time is smaller then Start time")]
        [FutureDate]
        [DatesAreSameDay("EndTime")]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, 5)]
        public int Priority { get; set; }

        [Required]
        [Range(1, int.MaxValue)] //instead of this i can check if this int is in color table as primary key 
        public int ColorId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TaskTypeId { get; set; }

        public string TaskCreatorId { get; set; }
    }
}
