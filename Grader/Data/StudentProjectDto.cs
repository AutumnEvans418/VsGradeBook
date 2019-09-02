using System;

namespace Grader
{
    public class StudentProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? DateGraded { get; set; }

        public bool IsBeingGraded { get; set; }
        public string Status { get; set; }
    }
}