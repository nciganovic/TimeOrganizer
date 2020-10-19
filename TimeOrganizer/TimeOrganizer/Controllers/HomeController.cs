using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model;
using TimeOrganizer.Model.InterfaceRepo;
using Microsoft.AspNetCore.Authorization;

namespace TimeOrganizer.Controllers
{
    public class HomeController : Controller
    {
        private ISchoolTypeRepository schoolTypeRepository;

        public HomeController(ISchoolTypeRepository schoolTypeRepository)
        {
            this.schoolTypeRepository = schoolTypeRepository;
        }

        [HttpGet]
        [Route("/")]
        [Authorize]
        public JsonResult Index() {
            IEnumerable<SchoolType> allSchoolTypes = schoolTypeRepository.AllSchoolTypes();
            JsonResult jsonResult = new JsonResult(allSchoolTypes);
            return jsonResult;
        }
    }
}
