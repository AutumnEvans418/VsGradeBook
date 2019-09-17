using Microsoft.AspNetCore.Mvc;

namespace Grader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {

        [HttpPut]
        public Submission UpdateSubmission([FromBody] Submission submission)
        {
            return new Submission();
        }

        [HttpPost]
        public Submission AddSubmission([FromBody] Submission submission)
        {
            return new Submission();
        }
    }
}