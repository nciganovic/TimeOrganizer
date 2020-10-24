﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.Tables
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int SchoolId { get; set; }

        public School School { get; set; }

        public List<ApplicationUserTask> ApplicationUserTasks { get; set; }
    
        public List<Task> Tasks { get; set; }
    }
}
