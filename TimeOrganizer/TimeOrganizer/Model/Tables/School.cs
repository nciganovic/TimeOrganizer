using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.Tables
{
    public class School
    {
        public int Id { get; set; }
        public int SchoolTypeId { get; set; }
        public SchoolType SchoolType { get; set; }
        public string Name { get; set; }
    }
}
