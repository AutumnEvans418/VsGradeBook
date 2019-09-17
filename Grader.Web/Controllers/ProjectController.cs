using Microsoft.AspNetCore.Mvc;

namespace Grader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudentProjects(int studentId)
        {
            return Ok(new StudentProjectsDto(new []{new StudentProjectSummaryDto(), new StudentProjectSummaryDto(), }));
        }

        [HttpGet("{projectId}")]
        public ProjectSubmissionDto GetStudentSubmission(int projectId, int studentId)
        {
            return new ProjectSubmissionDto();
        }


        
    }
}