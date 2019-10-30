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
        private readonly IPlagiarismService _plagiarismService;

        public SubmissionsController(IGradeBookRepository gradeBookRepository, IPlagiarismService plagiarismService)
        {
            _gradeBookRepository = gradeBookRepository;
            _plagiarismService = plagiarismService;
        }

        [HttpGet]
        public Task<IEnumerable<Submission>> GetSubmissions(Guid teacherCode)
        {
            return _gradeBookRepository.GetSubmissions(teacherCode);
        }

        [HttpPost]
        public async Task<Submission> AddSubmission([FromBody] Submission submission)
        {
            var result = await _gradeBookRepository.AddSubmission(submission);
            await _plagiarismService.Check(result.ProjectId);
            return result;
        }
    }
}