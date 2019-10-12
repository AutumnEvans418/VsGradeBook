using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Grader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly IGradeBookRepository _gradeBookRepository;

        public SubmissionsController(IGradeBookRepository gradeBookRepository)
        {
            _gradeBookRepository = gradeBookRepository;
        }

        [HttpGet]
        public Task<IEnumerable<Submission>> GetSubmissions(Guid teacherCode)
        {
            return _gradeBookRepository.GetSubmissions(teacherCode);
        }

        [HttpPost]
        public Task<Submission> AddSubmission([FromBody] Submission submission)
        {
            return _gradeBookRepository.AddSubmission(submission);
        }
    }
}