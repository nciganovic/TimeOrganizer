using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlColorRepository : IColorRepository
    {
        private AppDbContext appDbContext;

        public SqlColorRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Color GetColorById(int id)
        {
            var color = appDbContext.Colors.Find(id);
            return color;
        }

    }
}
