using System;

namespace Grader
{
    public class CodeProject
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CsvCases { get; set; }
        public string CsvExpectedOutput { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public bool IsPublished { get; set; }
    }
}