using System;

namespace Grader
{
    public enum StudentProjectStatus
    {
        Todo,
        InProgress,
        Submitted
    }

    public class LoginDto
    {
        public string Name { get; set; }
        public string CourseCode { get; set; }
    }

    public class StudentProjectSummaryDto
    {
        public StudentProjectSummaryDto()
        {
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? DateGraded { get; set; }
        public bool IsBeingGraded { get; set; }

        public bool HasSubmission { get; set; }
        public bool SubmissionPublished { get; set; }
        public StudentProjectStatus Status => GetStatus();

        private StudentProjectStatus GetStatus()
        {
            if (!HasSubmission)
            {
                return StudentProjectStatus.Todo;
            }
            else if (!SubmissionPublished)
            {
                return StudentProjectStatus.InProgress;
            }

            return StudentProjectStatus.Submitted;
        }

        public DateTimeOffset? DateSubmitted { get; set; }
    }
}