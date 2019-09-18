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
    public class PeopleController : ControllerBase
    {
        private readonly IGradeBookRepository _gradeBookRepository;

        public PeopleController(IGradeBookRepository gradeBookRepository)
        {
            _gradeBookRepository = gradeBookRepository;
        }
        [HttpGet]
        public Task<int> GetPersonId(string userName)
        {
            return _gradeBookRepository.GetPersonId(userName);
        }
    }
}