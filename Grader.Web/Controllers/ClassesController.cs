using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IGradeBookRepository _gradeBookRepository;

        public ClassesController(IGradeBookRepository gradeBookRepository)
        {
            _gradeBookRepository = gradeBookRepository;
        }

        [HttpGet]
        public Task<IEnumerable<Class>> GetClasses(int personId)
        {
            return _gradeBookRepository.GetClasses(personId);
        }
    }
}