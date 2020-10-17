using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface ISchoolTypeRepository
    {
        public IEnumerable<SchoolType> AllSchoolTypes();
    }
}
