using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grader.Web.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class EnrollmentsController : ControllerBase
    //{
    //    private readonly IGradeBookRepository _repository;

    //    public EnrollmentsController(IGradeBookRepository repository)
    //    {
    //        _repository = repository;
    //    }
    //    // GET: api/Enrollments
    //    [HttpGet]
    //    public Task<IEnumerable<Enrollment>> Get()
    //    {
    //        return _repository.GetEnrollments();
    //    }

    //    // GET: api/Enrollments/5
    //    [HttpGet("{id}", Name = "Get")]
    //    public Task<Enrollment> Get(int id)
    //    {
    //        return _repository.GetEnrollment(id);
    //    }

    //    // POST: api/Enrollments
    //    [HttpPost]
    //    public Task<Enrollment> Post([FromBody] Enrollment value)
    //    {
    //        return _repository.AddEnrollment(value);
    //    }

    //    // PUT: api/Enrollments/5
    //    [HttpPut("{id}")]
    //    public Task<Enrollment> Put(int id, [FromBody] Enrollment value)
    //    {
    //        return _repository.UpdateEnrollment(value);
    //    }

    //    // DELETE: api/ApiWithActions/5
    //    [HttpDelete("{id}")]
    //    public Task Delete(int id)
    //    {
    //        return _repository.DeleteEnrollment(id);
    //    }
    //}
}
