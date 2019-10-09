using System;
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
        public async Task<CodeProject> GetProject(Guid studentCode, Guid teacherCode)
        {
            return await _repository.GetCodeProject(studentCode, teacherCode);
        }


        [HttpPost]
        public async Task<CodeProject> AddProject([FromBody] CodeProject project)
        {
            return await _repository.AddProject(project);
        }
        
    }
}