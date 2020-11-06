using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
