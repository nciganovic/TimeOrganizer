using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlSchoolRepository : ISchoolRepository
    {
        private AppDbContext appDbContext;

        public SqlSchoolRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public School GetSchoolById(int id)
        {
            School school = appDbContext.School.Find(id);
            return school;
        }
    }
}
