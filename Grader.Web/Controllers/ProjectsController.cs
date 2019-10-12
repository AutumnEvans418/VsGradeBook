using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Grader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IGradeBookRepository _repository;

        public ProjectsController(IGradeBookRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("Project")]
        public async Task<IActionResult> GetProject(Guid studentCode, Guid teacherCode)
        {
            return Ok(await _repository.GetCodeProject(studentCode, teacherCode));
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects(string apiKey)
        {
            if (GradeBookRepositoryHttpClient.ApiKey == apiKey)
            {
                return Ok(await _repository.GetCodeProjects());
            }
            return Unauthorized();
        }


        [HttpPost]
        public async Task<CodeProject> AddProject([FromBody] CodeProject project)
        {
            return await _repository.AddProject(project);
        }

    }
}