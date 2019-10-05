using System;
using System.Collections.Generic;

namespace Grader
{
    public class Submission
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string StudentName { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public double Grade { get; set; }
        public virtual IList<SubmissionFile> SubmissionFiles { get; set; }
    }
}