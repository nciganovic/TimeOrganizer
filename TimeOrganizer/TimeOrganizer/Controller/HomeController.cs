using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model;
using TimeOrganizer.Model.InterfaceRepo;

namespace TimeOrganizer.Controller
{
    public class HomeController
    {
        private ISchoolTypeRepository schoolTypeRepository;

        public HomeController(ISchoolTypeRepository schoolTypeRepository)
        {
            this.schoolTypeRepository = schoolTypeRepository;
        }

        [HttpGet]
        [Route("/")]
        public JsonResult Index() {
            IEnumerable<SchoolType> allSchoolTypes = schoolTypeRepository.AllSchoolTypes();
            JsonResult jsonResult = new JsonResult(allSchoolTypes);
            return jsonResult;
        }
    }
}
