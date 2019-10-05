using System;
using System.Collections.Generic;

namespace Grader
{


    public class CodeProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CsvCases { get; set; }
        public string CsvExpectedOutput { get; set; }
        public DateTimeOffset DueDate { get; set; }
       // public virtual IList<Submission> Submissions { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public Guid StudentCode { get; set; }
        public Guid TeacherCode { get; set; }
        public CodeProject()
        {
            DateCreated = DateTimeOffset.Now;
            StudentCode = Guid.NewGuid();
            TeacherCode = Guid.NewGuid();
        }
    }
}