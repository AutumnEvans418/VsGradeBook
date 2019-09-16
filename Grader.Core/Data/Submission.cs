using System;

namespace Grader
{
    public class Submission
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int StudentId { get; set; }
        public bool IsSubmitted { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? SubmissionDate { get; set; }
        public virtual CodeProject CodeProject { get; set; }
        public virtual Person Student { get; set; }
        public DateTimeOffset? DateGraded { get; set; }
        public DateTimeOffset? GradingDate { get; set; }
    }
}