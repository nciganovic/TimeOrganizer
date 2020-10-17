using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.InterfaceRepo;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlSchoolTypeRepository : ISchoolTypeRepository
    {
        private AppDbContext appDbContext;

        public SqlSchoolTypeRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IEnumerable<SchoolType> AllSchoolTypes()
        {
            var data = appDbContext.SchoolTypes.Select(x => new SchoolType
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return data;
        }
    }
}
