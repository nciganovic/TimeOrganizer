using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model
{
    public class SchoolType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<School> Schools { get; set; }
    }
}
