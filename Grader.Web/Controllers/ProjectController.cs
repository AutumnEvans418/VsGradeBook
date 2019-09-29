using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Grader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IGradeBookRepository _repository;

        public ProjectController(IGradeBookRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IEnumerable<CodeProject>> GetProjects(string classId)
        {
            return await _repository.GetCodeProjects(classId);
        }

        //[HttpGet]
        //public IActionResult GetStudentProjects(int studentId)
        //{
        //    return Ok(new StudentProjectsDto(new []{new StudentProjectSummaryDto(), new StudentProjectSummaryDto(), }));
        //}

        [HttpGet("{projectId}")]
        public ProjectSubmissionDto GetStudentSubmission(int projectId, int studentId)
        {
            return new ProjectSubmissionDto();
        }


        
    }
}